using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Missions_Manager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] AttackCombosController attackCombosController;
    [SerializeField] public GameObject currentMission;
    [Header("Mission Stats")]
    [SerializeField] private float acomplishedMissions;
    [SerializeField] private GameObject[] MissionDeliverys;
    [SerializeField] private float totalMissions;
    [SerializeField] private int maxPizzaOrder;
    [SerializeField] private int orderOfPizza;
    [SerializeField] private Text orderOFPizzaText;


    [Header("Time")]
    [SerializeField] private float timeForDelivery;
    [SerializeField] private Text timeforDeliveryText;
    [SerializeField] private float generalTimeForMissions;
    [SerializeField] private float punishmentForTime;
    public bool startMissions;
    private float lastPizzaOrder;

    //Brujula
    [Header("Brujula")]
    [SerializeField]UpgradesSystem upgradesSystem;
    [SerializeField] private GameObject brujula;
    [SerializeField] private Transform target;
    [SerializeField] private Transform PizzaGiver;


    private bool timerRuning;
    private bool allMissionsFinished;
    private bool OncurrentMission;
    private SphereCollider colliderS;
    [SerializeField] private float moneyPerPizza; 
    [SerializeField] private float givenMoney;
    [SerializeField] private float maxTip;
    
    
    
    //Effects
    [SerializeField] private ParticleSystem missionEffect;
    MissionDirection direction;
    [SerializeField ] private AudioSource missionDoneSound;
    [SerializeField] private AudioSource missionDoneTalk;
    private bool talkReproduced;
    [SerializeField] private AudioSource[] MissionSounds;
    private int ClientVoices;

    [Header("TextEffects")]
    [SerializeField] private TMP_Text missionDataText;
    [SerializeField] private Animator textAnim;
    [SerializeField] private AudioSource StartSound;
    private float lastFaultPizza;

    MissionIndicator missionIndicator;

    [Header("MissionTexts")]
    [SerializeField] private GameObject dN;
    [SerializeField] private Text ClientNameText;
    [SerializeField] private Text MissionDirectionText;
    [SerializeField] private string[] Names;

    

    [Header("ClientManagement")]
    [SerializeField] private Transform[] transformClientHouse;
    [SerializeField] private GameObject[] clientModel;
    private Transform clientPosition;
    [SerializeField] private Transform playerPosition;
    [SerializeField]private bool clientTouchPlayer;
    private int randClientModel;
    [SerializeField]private float clientSpeed;
    [SerializeField] private float clienteRange;
    [SerializeField] LayerMask whatisPlayer;
    private bool clientisReciving;
    Vector3 inicialPos;
    private bool blindToClient;

    private bool Missionfailed;

    private void Awake()
    {
        colliderS = GetComponent<SphereCollider>();
        missionIndicator = GameObject.Find("Missionindicator").GetComponent<MissionIndicator>();
    }
    private void Start()
    {
        
        startMissions = false;
        OncurrentMission = false;
        brujula.SetActive(false);
        talkReproduced = false; //this is for mussolini to not reapte things
        generalTimeForMissions = timeForDelivery;
        clientisReciving = false;
    }
    private void Update()
    {

        if (clientisReciving) {clientModel[randClientModel].SetActive(true); ClientAction(); }
        else 
        {
            //clientModel[randClientModel].SetActive(false);
            inicialPos = new Vector3(transform.position.x, 1.8f, transform.position.z + 12);
            clientModel[randClientModel].transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            clientModel[randClientModel].transform.position = Vector3.Lerp(clientModel[randClientModel].transform.position, inicialPos, clientSpeed * Time.deltaTime);
            
        }
        UIStatus();
        if (gameManager.TotalMoneyFromMissions > 100) { maxPizzaOrder = 5; }
        if (gameManager.PizzasInBackPack <= 0) 
        {
            Vector3 directionb = PizzaGiver.position - brujula.transform.position;
            Quaternion rotation = Quaternion.LookRotation(directionb);
            brujula.transform.rotation = rotation;
        }
        else
        {
            Vector3 directionb = target.position - brujula.transform.position;
            Quaternion rotation = Quaternion.LookRotation(directionb);
            brujula.transform.rotation = rotation;
        }

        if (!OncurrentMission && !allMissionsFinished && startMissions) //empezar mision!
            
        {
            dN.SetActive(true);
            StartSound.Play();
            clientModel[randClientModel].SetActive(true);
            Missionfailed = false;
            givenMoney = moneyPerPizza;
            if (upgradesSystem.FaryOn) {missionIndicator.Indicator.SetActive(true);}            
            if (upgradesSystem.CompassOn) { brujula.SetActive(true);}           
            missionEffect.Play();
            OncurrentMission=true;
            timerRuning = true;
            int MissionActiveNumber = Random.Range(0, MissionDeliverys.Length);   
            currentMission = MissionDeliverys[MissionActiveNumber];
            direction = currentMission.gameObject.GetComponent<MissionDirection>();
            Debug.Log(direction.missionDirection);
            ChoseName();
            currentMission.SetActive(true);
            colliderS.transform.position = currentMission.transform.position;
            orderOfPizza = Random.Range(1, maxPizzaOrder);
            lastPizzaOrder = orderOfPizza;
            ClientVoices = Random.Range(0, 3);
            
        }
        if (!Missionfailed) { CheckforProgress(); }
        if (timerRuning)
        {
            timeForDelivery -= Time.deltaTime;
            if (timeForDelivery <= 0) {timerRuning = false; Debug.Log("TimeOut"); givenMoney = 0; StopMission(); MissionSounds[3].Play(); }
        }
    }
    void ChoseName()
    {
        int nameOn = Random.Range(0,Names.Length);
        ClientNameText.text = "" + Names[nameOn];
        MissionDirectionText.text =""+direction.missionDirection;
    }
    void CheckforProgress()
    {
        if(orderOfPizza <= 0 && acomplishedMissions < totalMissions && OncurrentMission ) //cumplir una mision
        {
            MissionDirectionText.text = string.Empty;
            timeForDelivery = Mathf.Round(timeForDelivery);
            gameManager.MoneyFromMissions += (givenMoney * lastPizzaOrder + ((maxTip * (timeForDelivery / generalTimeForMissions))) - attackCombosController.FaulPizza);
            gameManager.TotalMoneyFromMissions += (givenMoney * lastPizzaOrder + ((maxTip * (timeForDelivery / generalTimeForMissions))) - attackCombosController.FaulPizza);
            //gameManager.MoneyFromMissions = gameManager.MoneyFromMissions + (givenMoney*lastPizzaOrder + (gameManager.MoneyFromMissions + (maxTip * (timeForDelivery/generalTimeForMissions)))-attackCombosController.FaulPizza);
            //gameManager.TotalMoneyFromMissions = gameManager.MoneyFromMissions + (givenMoney * lastPizzaOrder + (gameManager.MoneyFromMissions + (maxTip * (timeForDelivery / generalTimeForMissions))) - attackCombosController.FaulPizza);
            gameManager.MoneyFromMissions = Mathf.Round(gameManager.MoneyFromMissions);
            MissionData();
            timerRuning =false;
            lastPizzaOrder = 0;
            acomplishedMissions++;
            OncurrentMission = false;
            startMissions = false;
            timeForDelivery = generalTimeForMissions;
            missionEffect.Stop();
            brujula.SetActive(false);
            missionDoneSound.Play();
            dN.SetActive(false);
            attackCombosController.FaulPizza = 0;


            if (!talkReproduced) {missionDoneTalk.Play(); talkReproduced = true; }
        }
        
        if (acomplishedMissions == totalMissions) { allMissionsFinished = true; Debug.Log("areaCompletada"); missionEffect.Stop(); totalMissions = 0; }
    }
    void MissionData()
    {
        lastFaultPizza = (givenMoney * lastPizzaOrder + ((maxTip * (timeForDelivery / generalTimeForMissions))));
        lastFaultPizza = Mathf.Round(lastFaultPizza);
        missionDataText.text = lastFaultPizza + " for Pizza -"+(attackCombosController.FaulPizza)+"for pizza quality";
        textAnim.SetTrigger("text");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && startMissions)
        {
            blindToClient = false;
            clientModel[0].SetActive(true);
            float zpos = transform.position.z;
            float xpos = transform.position.x;
            randClientModel = Random.Range(0, 0);
            inicialPos = new Vector3(xpos, 1.7f, zpos + 5);          
            clientModel[randClientModel].transform.position = inicialPos;
            clientisReciving = true;

            //RemovePizzaFromPlayer(); //Temporary to see if is working
        }
        else if(other.gameObject.name == "Missionindicator")
        {
            missionIndicator.regreso = true; 
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && startMissions) { blindToClient = true; }
            
    }
    void RemovePizzaFromPlayer()
    {
        float pizzaInBP = orderOfPizza;
        for (int i = 0; i < gameManager.PizzasInBackPack; i++)
        {          
            orderOfPizza--;
            if (orderOfPizza < 0) { orderOfPizza = 0; }
        }
        gameManager.PizzasInBackPack = gameManager.PizzasInBackPack - pizzaInBP;
        if (gameManager.PizzasInBackPack < 0) { gameManager.PizzasInBackPack = 0;}
        if (orderOfPizza > 0) { MissionSounds[ClientVoices].Play(); } //Sonido cuando llegas sin pizzas o cuando te faltan todavia

    }
    private void ClientAction()
    {

        clientTouchPlayer = Physics.CheckSphere(clientModel[0].transform.position, clienteRange, whatisPlayer);
        if (blindToClient)
        {
            
            inicialPos = new Vector3(transform.position.x,1.8f, transform.position.z + 12);
            clientModel[randClientModel].transform.position = Vector3.Lerp(clientModel[randClientModel].transform.position, inicialPos, clientSpeed * Time.deltaTime);
        }
        if (clientisReciving)
        {
                       
            if (clientTouchPlayer)
            {
                RemovePizzaFromPlayer();
                clientisReciving = false;               
                //clientModel[0].SetActive(false);
                
            }
            else
            {
                
                Vector3 missionPositionToclient = new Vector3(currentMission.transform.position.x, 2, currentMission.transform.position.z);
                clientModel[randClientModel].transform.position = Vector3.Lerp(clientModel[randClientModel].transform.position, missionPositionToclient, clientSpeed * Time.deltaTime);
                clientModel[randClientModel].transform.LookAt(new Vector3(playerPosition.position.x, clientModel[randClientModel].transform.position.y, playerPosition.position.z));
            }

        }
        
       
        
    }

    void UIStatus()
    {
        timeforDeliveryText.text = "" + timeForDelivery;
        orderOFPizzaText.text = "" + orderOfPizza;
    }
    public void StopMission() 
    {
        Missionfailed = true;
        timeForDelivery =0;
        orderOfPizza=0;
        timerRuning = false;
        lastPizzaOrder = 0;
        acomplishedMissions++;
        OncurrentMission = false;
        startMissions = false;
        timeForDelivery = generalTimeForMissions;
        missionEffect.Stop();
        brujula.SetActive(false);
        

    }

}
