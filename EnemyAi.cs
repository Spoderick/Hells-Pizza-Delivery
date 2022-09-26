using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    Animator anim;


    public LayerMask whatisGround, whatisPlayer;
    //Health And Stats
    [Header("Health Stats")]
    [SerializeField] private float Health = 100f;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject HealthBarUi;
    [SerializeField] private Slider slider;
    [SerializeField] private Transform cameraPosition;
    //damage and die
    [Header("Damage")]
    public float DamageToPlayer;
    [SerializeField] private bool isRanged;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] Transform bulletFireHole;
    
    [Header("Money")]
    [SerializeField] private float moneyGiven;
    [SerializeField] GameManager gameManager;

    [Header("Patrolling Stats")]
    [SerializeField] private Transform[] patrollingPositions;
    //Patrullar o Quedarse quiero
    Vector3 walkPoint;
    bool walkingPoint;
    //[SerializeField] private float walkrange; //rango de caminado
    [SerializeField] private bool stayinplace;

    //Atacar
    public float timeBetweenAttacks;
    bool alreadyAtacking;
    [SerializeField] private AudioSource attackSound;
    

    //TEST  
    public AttackCombosController damage;
    Bullet_Script bulletS;
    bool hitbyBullet;
    
    public float attackSpeed;

    //estados
    public float rangodeVista, rangodeataque;
    public bool playerInSight, playerInAttack;

    //effects
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Transform posForParticle;
    //Sounds
    [SerializeField] AudioSource[] EnemySounds;
 
    [SerializeField] GameObject damageText;


    private void Awake()
    {
        //player = GameObject.Find("Player").transform; //Nombre del player(Jugador)
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player=GameObject.Find("Cecilio").GetComponent<Transform>();
        
    }
    private void Start()
    {
        hitbyBullet = false;
        Health = maxHealth;
        slider.value = CalculateHealth();
        HealthBarUi.SetActive(false);
        cameraPosition = GameObject.Find("ViewPointCanvas").GetComponent<Transform>(); //Nombre de la camara o el objeto que va a seguir
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "DamageDealer") // Nombre del tag de la Pala/Arma
        {
            TakeDamage();
            
        }
        else if (other.gameObject.tag == "Bullet")
        {
            print("Me la regresaste culero");
            hitbyBullet = true;
            Destroy(other.gameObject);
            TakeDamage();
            
        }
    }

    void Update()
    {
        
        playerInSight = Physics.CheckSphere(transform.position, rangodeVista, whatisPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, rangodeataque, whatisPlayer);
        if (!playerInSight && !playerInAttack)
        {

            if (stayinplace)
            {
                anim.SetBool("Walking", false);
                agent.SetDestination(transform.position);   
            }
            else
            {              
                Patroling();
                anim.SetBool("Walking", true); //nombre del bool de walking    
            }
                   
        }
        if (playerInSight && !playerInAttack)
        {               
            ChasePlayer();
            anim.SetBool("Walking", true); //nombre del bool de walking           
            
        }
        if (playerInSight && playerInAttack)
        {           
            AttackPlayer();
            anim.SetBool("Walking", false); //nombre del bool de walking
        }
        
        slider.value = CalculateHealth();
        slider.transform.LookAt(cameraPosition.position);
        if (Health < maxHealth)
        {
            HealthBarUi.SetActive(true);
        }
        
        
    }
    
    float CalculateHealth()
    {
        return Health / maxHealth;
    } 
    private void Patroling()
    {
        
        if (!walkingPoint)
        {
            SearchWalkPoint();
        }
        if (walkingPoint)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceTowalkPoint = walkPoint - transform.position;
        print(distanceTowalkPoint);
        //Llego al punto de caminado
        if (distanceTowalkPoint.magnitude < 1)
        {
            walkingPoint = false;
        }
        else 
        { walkingPoint = true; }

    }
    private void SearchWalkPoint()
    {
        int randPos = Random.Range(0,patrollingPositions.Length);
        float randomZ = patrollingPositions[randPos].position.z ;
        float randomX = patrollingPositions[randPos].position.x ;
        
        walkPoint = new Vector3(randomX, transform.position.y,randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisGround))
        {
            
            walkingPoint = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        
    }
    private void AttackPlayer()
    {
        Vector3 TargetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        //Nose mueve para atacar
        agent.SetDestination(transform.position);
        transform.LookAt(TargetPosition);
        if (!alreadyAtacking)
        {
            anim.SetTrigger("Attack");//nombre del trigger para atacar
            alreadyAtacking = true;
            
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        
    }
    private void ResetAttack()
    {
        alreadyAtacking = false;
        
    }   
    public void TakeDamage()
    {
        Instantiate(hitEffect, posForParticle.position, Quaternion.identity);
        Health -= damage.AttackDamage;
        DamageNumbers indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageNumbers>();
        
        //transform.position = new Vector3(transform.position.x,transform.position.y,(transform.position.z-.5f));
        int randSound = Random.Range(0,2);
        print(randSound);
        EnemySounds[randSound].Play();
        if (damage.HeavyAttack)
        {
            anim.SetTrigger("damage"); //nombre del trigger para daño a este enemigo
            Health -= (damage.AttackDamage+4);
            
            indicator.SetDamageText(damage.AttackDamage+4);

        }
        else
        {
            indicator.SetDamageText(damage.AttackDamage);
        }
        if (hitbyBullet) { Health -= 100f; anim.SetTrigger("damage"); indicator.SetDamageText(-100); }
        if (Health <= 0)
        {
            
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            //animacion de muerte
            Invoke(nameof(DestroyEnemy), .4f);
            EnemySounds[2].Play();
        }
        else if (Health < 50) { timeBetweenAttacks = .4f; }
        
    }
    private void DestroyEnemy()
    {
        gameManager.MoneyFromDemons += moneyGiven;
        Destroy(this.gameObject);
    }

    public void PlayerDamage()
    {
        if (playerInAttack)
        {
            attackSound.Play();
            if (!isRanged)
            {
                damage.Playerhealth = damage.Playerhealth - DamageToPlayer;
                Debug.Log("Daño al jugador");
            }
            else
            {
                
                muzzleFlash.Play();
                var currentBullet = Instantiate(enemyBullet, bulletFireHole.position, Quaternion.identity);
                bulletS = currentBullet.gameObject.GetComponent<Bullet_Script>();
                bulletS.shootpoint = bulletFireHole;
                bulletS.SourceTarget = gameObject.GetComponent<Transform>();
                


            }
        }
    }


}
