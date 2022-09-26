using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCombosController : MonoBehaviour
{
    [Header("Scripts related")]
    private Animator animator;
    private int combo;
    public bool atacando;
    public bool animacionAtaque;
    
    [SerializeField]private float Nataques;
    [SerializeField]private BoxCollider swordCollider;    
    public bool HeavyAttack;
    [Header("Health Stats")]
    //Player Health
    [SerializeField]private Slider slider;
    [SerializeField]private float playerhealth;
    [SerializeField] private float maxHealth;
    private float currentPlayerHealth;
    public float Playerhealth
    {
        get { return playerhealth; }
        set { playerhealth = value; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    [Header("Attack Stats")]
    //Stats
    [SerializeField]private float attackDamage;
    public float AttackDamage
    {
        set { attackDamage = value; }
        get { return attackDamage; }
    }
    private bool canAttack;
    [Header("PizzaLife")]
    [SerializeField] private float pizzaLife; //quantity of hits
    private float totalpizzalife;
    private float faultPizza;
    public float FaulPizza
    {
        set { faultPizza = value; }
        get { return faultPizza; }
    }
    [SerializeField] private Image pizzaBoxes;
    [SerializeField] private Image pizzaContainer;
    private float colorValue;
    //
    [SerializeField] Camera cam;
    [SerializeField] Transform RespawnPoint;
    [SerializeField] CharacterController control;
    
    CaracterMovement movement;
    [SerializeField]LayerMask IgnoreMe;

    GameManager gameManager;
    [SerializeField] private bool isParryng;      
    public bool IsParryng
    {
        set { isParryng = value; }
        get { return isParryng; }
    }
    private bool alreadyDeath;
    [SerializeField] private Missions_Manager missions;    
    //effects
    [SerializeField] private SkinnedMeshRenderer CecilioMaterial;
    [SerializeField] private ParticleSystem bloodSplatter;
    [SerializeField] private Transform bloodpos;
    //Sounds
    [SerializeField] private AudioSource[]HitSounds;
    [SerializeField] private AudioSource hit;
    [SerializeField] private AudioSource deathSound;
    public Vector3 MousePosition { get; private set; } //Posicion 3D del mouse
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movement = GetComponent<CaracterMovement>();
    }
    void Start()
    {   
        alreadyDeath = false;
        canAttack = true;
        playerhealth = maxHealth;
        currentPlayerHealth = playerhealth;
        swordCollider.enabled = false;
        totalpizzalife = pizzaLife;
        colorValue = 1;
    }   
    public void StartCombo()
    {
        atacando = false;       
        if (combo < 3)
        {
            Nataques++;
            combo++;           
        }     
    }
   
    public void Combos()
    {
        if (Input.GetMouseButtonDown(0) && !atacando && canAttack)
        {

            RotaralMouse();
            atacando = true;
            animator.SetTrigger("" +combo);
            HitSounds[combo].Play();
            RestaConsecutiva();

        }
        if (Input.GetMouseButtonDown(1) && !atacando && canAttack) 
        {
            canAttack = false;
            HeavyAttack=true;
            RotaralMouse();
            animator.SetTrigger("Parryng");          
        }
    }
    public void FinishAnim()
    {
        animator.SetTrigger("idle");       
        atacando = false;
        combo = 0;       
    }
    [SerializeField] private void StartBlocking()
    {
        
        atacando = true;
        isParryng = true;
        HitSounds[2].Play();
    }
    [SerializeField] private void FinishBlocking()
    {
        canAttack=true;
        HeavyAttack = false;
        isParryng =  false;
        atacando = false;        
    }
    void RotaralMouse()
    {
        Ray ray = cam.ScreenPointToRay(MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 1100f, ~IgnoreMe))
        {
            
            
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }
    void Update()
    {
        if (playerhealth <= 0)
        {
            Death();
        }
        MousePosition = Input.mousePosition;
        Combos();
        AtaqueApuntado();             
        slider.value = ((playerhealth*100)/maxHealth);
        if (playerhealth < currentPlayerHealth) //condicion cuando Cecilio pierde vida 
        { 
            StartCoroutine(GettingHit()); 
            currentPlayerHealth = playerhealth;
            PizzaLossesHealth();            
        }
        if (combo <= 0f)
        {
            animacionAtaque = false;
        }
    }
    public void PizzaRestore()
    {
        faultPizza = 0;
        pizzaLife = totalpizzalife;
        colorValue = 1;
        pizzaBoxes.color = new Color(1, colorValue, colorValue, 1);
        pizzaContainer.color = new Color(1, colorValue, colorValue, 1);
    }
    void PizzaLossesHealth()
    {
        if(gameManager.PizzasInBackPack <= 0)
        {
            //no hay pizza que quitar
            //sonido
        }
        else
        {
            pizzaLife--;
            faultPizza++;
            if (pizzaLife <= 0 || faultPizza >= 5)
            {
               faultPizza = 5;
               
            }
            else
            {
                float colorpercentage = (1 / totalpizzalife);                
                float pizzaLifePercent = ((pizzaLife * 100) / totalpizzalife);
                print(pizzaLifePercent);
                colorValue -= colorpercentage;
                pizzaBoxes.color = new Color(1, 0, 0,1);
                pizzaBoxes.color = new Color(1,colorValue,colorValue, 1);
                pizzaContainer.color = new Color(1,colorValue,colorValue, 1);                                
            }
        }
    



    }
    [SerializeField] private void WeaponCollisionOn()
    {
        swordCollider.enabled = true;
    }
    [SerializeField] private void WeaponCollisionOff()
    {
        swordCollider.enabled = false;
        
    }
    void AtaqueApuntado()
    {        
        if (Nataques > 0)
        {        
            animacionAtaque = true;         
        }       
    }
    void RestaConsecutiva()
    {
        if (Nataques <= 0)
        {
            Nataques = 0;
            animacionAtaque = false;
        }
        else if (Nataques > 0)
        {
            Nataques--;
        }
        else if (combo <= 0)
        {
            animacionAtaque=false;
        }                   
    }
    IEnumerator GettingHit()

    {
        if (playerhealth <= 0)
        {
            Death();
        }

        else
        {
            Instantiate(bloodSplatter, bloodpos.position,Quaternion.identity);
            hit.Play();
            CecilioMaterial.material.SetFloat("redfloat", .65f);
            cam.fieldOfView = 59.5f;
            yield return new WaitForSeconds(.1f);
            cam.fieldOfView = 60;
            CecilioMaterial.material.SetFloat("redfloat", 0);
        }        
    }
    void Death()
    {
       if (!alreadyDeath)
        {
            alreadyDeath = true;
            deathSound.Play();
            //missions.StopMission();
            gameManager.PizzasInBackPack = 0;
            pizzaBoxes.color = new Color(1, 1, 1, 1);
            pizzaContainer.color = new Color(1, 1, 1, 1);
            StartCoroutine(Wait());
        }                 
    }

    IEnumerator Wait()
    {

        gameManager.ToBlack();
        yield return new WaitForSeconds(1.5f);
        control.enabled = false;
        playerhealth = maxHealth;
        gameManager.MoneyFromMissions = gameManager.MoneyFromMissions - gameManager.RevivalCost;
        control.transform.position = new Vector3(RespawnPoint.position.x, RespawnPoint.position.y, RespawnPoint.position.z);
        control.enabled = true;
        control.enabled = true;
        gameManager.BlackOut();
        alreadyDeath = false;
    }
    


}
