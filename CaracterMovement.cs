using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaracterMovement : MonoBehaviour
{ 
    CharacterController control;

    [Header("Character Values")]
    private float Speed; //Player Speed
    [SerializeField] private float baseSpeed;
    public float BaseSpeed
    {
        get { return baseSpeed; }
        set { baseSpeed = value; }
    }
    [SerializeField] private float speedAttacking;
    [SerializeField] private float DodgeSpeed; //PlayerDodgeSpeed
    [SerializeField] private float DodgeTime; //Player time of getting the dodgespeed
    [Header("Camara To player")]
    [SerializeField] private Camera Camera;

    [Header("Dash Vartiables")]
    //Dashes variables
    [SerializeField] private float generalDashCoolDown;
    [SerializeField] private float DashCoolDown; //time beteewn dashes
    [SerializeField] private AudioSource DashSound;
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] private GameObject DirectionDash;
    [SerializeField] private Image DashViewer;
    [SerializeField] private bool DashOnCourse; //if a dash is on course or not
    public float GeneralDashCoolDown
    {
        get { return generalDashCoolDown; }
        set { generalDashCoolDown = value; }
    }
    [SerializeField] private float DashCounter; //number of dashes
    [SerializeField] private float maxDashes;  //Max Dashes 
    [SerializeField] private Text textDashcounter;
    public float MaxDashes
    {
        get { return maxDashes; }
        set { maxDashes = value; }
    }

    
    private Animator animator; //Animator

    [Header("Effects and Sounds")]
    [SerializeField] private AudioSource[] movementSound;
    

    [SerializeField] private AttackCombosController Attacking;
    public Vector2 mouseVector { get; private set; } //posicion 2D del mouse
    public Vector3 MousePosition { get; private set; } //Posicion 3D del mouse

    private void Awake()
    {
        control = GetComponent<CharacterController>();   
        animator = GetComponent<Animator>();
        
    }
    private void Start()
    {
        
        
        dashParticle.Stop();

    }
    void Update()
    {
        Vector3 gravity = new Vector3(0,-9.81f,0);
        control.Move(gravity * Time.deltaTime);
        DashDirection();
        transform.position -= -transform.up * (9.8f * Time.deltaTime);
        textDashcounter.text = "" + DashCounter;
        CharacterMovement();

        if (Input.GetKeyDown("space") && DashCounter > 0 && !DashOnCourse)
        {
            DashOnCourse = true;
            DashSound.Play();
            StartCoroutine(SecondDash()); //cambiar a dash o a second dash
            StartCoroutine(DashAnimation());           
        }
        if (DashCounter < MaxDashes)
        {
            CoolDownAction();
        }

    }
    void DashDirection()
    {

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveX, 0, moveZ);
        DirectionDash.transform.forward = movement;

    }
    void CharacterMovement()
    {
        
        var targetVector = new Vector3(mouseVector.x, 0, mouseVector.y); //vector del mouse
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        MousePosition = Input.mousePosition;
        mouseVector = new Vector2(moveX, moveZ);
        //Capturo en variables el movimiento de mis Axis con los botones designados a cada cordenada (wasd,arriba,abajo,Derecha,Izqierda)
        Vector3 movement = new Vector3(moveX, 0, moveZ);
        control.Move(movement * Speed * Time.deltaTime);
        //Aplico esas variables al movimiento de mi personaje.
        if (movement != Vector3.zero && Attacking.animacionAtaque == false)
        {
            transform.forward = movement;
            Speed = baseSpeed;
            //if (Attacking.animacionAtaque)
            //{
                //RotaralMouse();
            //}
            //else {transform.forward = movement; }
            animator.SetFloat("Blend", 1f);
                  
        }
        else if (Attacking.animacionAtaque) { Speed = speedAttacking; }
        else
        {
            animator.SetFloat("Blend", 0f); //animacion a Idle
            
            
        }

    }
   
    void RotaralMouse()
    {
        Ray ray = Camera.ScreenPointToRay(MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 500f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
            
        }
    }
    IEnumerator DashAnimation()
    {
        dashParticle.Play();
        Debug.Log("Dash");
        //Physics.IgnoreLayerCollision(9, 10, true);
        animator.SetBool("Dash", true);
        yield return new WaitForSeconds(.3f);
        animator.SetBool("Dash", false);
        DashOnCourse = false;
        //Physics.IgnoreLayerCollision(9, 10, false);
        Debug.Log("Dash terminado");
        dashParticle.Stop();


    }
    
    private void CoolDownAction()
    {
        if (DashCoolDown > 0)
        {
            DashViewer.rectTransform.sizeDelta = new Vector2(100,100);
            var tempcolor = DashViewer.color;
            tempcolor.a = .2f;
            DashViewer.color = tempcolor;   
            DashCoolDown -= Time.deltaTime;
            
        }
        if (DashCoolDown <= 0)
        {
            DashViewer.rectTransform.sizeDelta = new Vector2(130, 130);
            var tempcolor = DashViewer.color;
            tempcolor.a = 1f;
            DashViewer.color = tempcolor;
            DashCoolDown = GeneralDashCoolDown; //Tiempo de dash "CoolDown" 
            DashCounter++;
        }
        
    }
   
    [SerializeField] void WalkSounds()
    {
        int walksound = Random.Range(0, movementSound.Length);
        movementSound[walksound].Play();
    }
   
    IEnumerator SecondDash() //Este es el bueno
    {
        DashOnCourse = true;
        float startTime = Time.time;
        DashCounter--;
        
        
        while (Time.time < startTime + DodgeTime)
        {
            control.Move(DirectionDash.transform.forward * Speed * DodgeSpeed * Time.deltaTime);
            yield return null;
        }
        
    }


}
