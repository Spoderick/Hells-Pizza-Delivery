using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    AttackCombosController attackCombosController;

    [Header("Bullet Properties")]

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float baseBulletSpeed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float baseAngularSpeed;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float secondsToDie;

    private bool returning;



    private bool enemydetected;
    LayerMask whatisEnemy;
    [SerializeField]private float rangeBullet;
    private Transform closestEnemy;

    public Transform shootpoint;
    private Transform Target;
    public Transform SourceTarget;
    [SerializeField] private ParticleSystem hitparticle;
    [SerializeField] private LayerMask layerMask;
    [SerializeField]

    
    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "DamageDealer" && attackCombosController.IsParryng)
        {
            CollideWithParry();
            print("Me diste");
            CollideWithSomething();
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (attackCombosController.IsParryng)
            {
                CollideWithParry();
                print("Me diste");
                CollideWithSomething();
            }
            else
            {
                print("te di");
                attackCombosController.Playerhealth -= bulletDamage;
                CollideWithSomething();
                Destroy(gameObject);
            }
           

        }
        
        else if (collision.gameObject.tag != "Spawner")
        {
            CollideWithSomething();
            Debug.Log(collision.name);
        }
       
        
       
    }
    private void Awake()
    {
        attackCombosController = GameObject.Find("Cecilio").GetComponent<AttackCombosController>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.identity;
        bulletSpeed = baseBulletSpeed;
        angularSpeed = baseAngularSpeed;
        Target = GameObject.Find("Cecilio").GetComponent<Transform>();
        StartCoroutine(TimeAlive());
    }
    void Update()
    {
        
      if (returning)
      {
         secondsToDie = 5;
         Vector3 enemy = new Vector3(SourceTarget.position.x,transform.position.y,SourceTarget.position.z);  
         transform.position = Vector3.Lerp(transform.position, enemy, bulletSpeed*Time.deltaTime);
        }
      else
      { 
            Vector3 roteZ = new Vector3(Target.position.x,0, Target.position.z);
            var q = Quaternion.LookRotation(roteZ);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, angularSpeed * Time.deltaTime);
            transform.position += shootpoint.forward * bulletSpeed * Time.deltaTime;
            
      }
      
        

    }


    void CollideWithSomething()
    {
        
        Instantiate(hitparticle, transform.position, new Quaternion(0,0,-transform.rotation.z,0));
        Destroy(gameObject, .2f);
    }
    void CollideWithParry()
    {
        transform.gameObject.tag = "Bullet";
        returning = true;
        angularSpeed = 180;
        bulletSpeed = 15;
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, .2f, 0, 0.2f);
        Gizmos.DrawSphere(transform.position,rangeBullet);
    }

    IEnumerator TimeAlive()
    {
        yield return new WaitForSeconds(secondsToDie);
        Destroy(gameObject);
    }


}
