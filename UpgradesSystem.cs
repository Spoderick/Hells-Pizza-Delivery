using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesSystem : MonoBehaviour
{
    [Header("ScripsComunication")]
    [SerializeField] GameManager gameManager;
    [SerializeField] AttackCombosController attackCombosController;
    [SerializeField] CaracterMovement characterController;
    [SerializeField] Missions_Manager missionsManager;

    [Header("Buttons")]
    [SerializeField] private Button[] UpgradeButtons;

    [SerializeField] private Text[] textPrices; 
    [SerializeField] private float[] upgradePrizes;

    [SerializeField] private Text[] currentValuetext;
    [SerializeField] private Text[] endValuetext;

    [Header("Carry Capacity")]
    [SerializeField]private int carryCapacityLevel;
    [SerializeField]private int startingCarryCapacityValueIncreasement;
    [SerializeField] private Image[] LevelBalls;

    [Header("Life Upgrade")]
    [SerializeField] private float lifeIncreasement;
    [SerializeField] private int lifeLevel;
    [SerializeField] private float startingLifeValueIncreasement;
    [SerializeField] private Image[] lifeLevelBalls;
    [Header("Compass Upgrade")]
    [SerializeField] private bool compassOn;
    [SerializeField] private Image compassLevelBalls;
    
    public bool CompassOn
    {
        get { return compassOn; }
            set { compassOn = value; }
    }
    [Header("Fary Upgrade")]
    [SerializeField] private bool faryOn;
    [SerializeField] private Image faryLevelBall;
    public bool FaryOn
    {
        get { return faryOn; }
        set { faryOn = value; }
    }
    [Header("DashN Upgrade")]
    [SerializeField] private int startingDashNValueIncreasement;
    [SerializeField] private Image dashLevelBall;
    [Header("DashN Upgrade")]
    [SerializeField] private float dashNewCooldown;
    [SerializeField] private float startingDashCValueIncreasement;
    [SerializeField] private Image dashCoolLevelBall;
    [Header("Attack Upgrade")]
    [SerializeField] private float attackIncreasement;
    [SerializeField] private int attackLevel;
    [SerializeField] private float startingattackValueIncreasement;
    [SerializeField] private Image[] attackLevelBalls;
    [Header("Attack Upgrade")]
    [SerializeField] private float speedIncreasement;
    [SerializeField] private int speedLevel;
    [SerializeField] private float startingspeedValueIncreasement;
    [SerializeField] private Image[] speedLevelBalls;




    //UI

    [Header("Money")]
    //Money
    [SerializeField]private float money;

    [Header("Button Effects")]
    [SerializeField] private AudioSource[] SonidosUpgrades;
    [SerializeField] private AudioSource[] fartArray;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateText();
        
        money = gameManager.MoneyFromDemons;
    }

    void CheckForLevels()
    {
        if(carryCapacityLevel == 3) 
        {
            UpgradeButtons[0].gameObject.SetActive(false);
            endValuetext[0].text="---";
        }
        if (lifeLevel == 3)
        {
            UpgradeButtons[1].gameObject.SetActive(false);
            endValuetext[1].text = "---";
        }
        if (compassOn) { UpgradeButtons[2].gameObject.SetActive(false); }
        if (faryOn) { UpgradeButtons[3].gameObject.SetActive(false); }
        if (attackLevel == 3)
        {
            UpgradeButtons[6].gameObject.SetActive(false);
            endValuetext[4].text = "---";
        }
        if (speedLevel == 2)
        {
            UpgradeButtons[7].gameObject.SetActive(false);
            endValuetext[5].text = "---";
        }
    }
    void UpdateText()
    {
        textPrices[0].text = "" + upgradePrizes[0];
        textPrices[1].text = "" + upgradePrizes[1];
        textPrices[2].text = "" + upgradePrizes[2];
        textPrices[3].text = "" + upgradePrizes[3];
        textPrices[4].text = "" + upgradePrizes[4];
        textPrices[5].text = "" + upgradePrizes[5];
        textPrices[6].text = "" + upgradePrizes[6];
        textPrices[7].text = "" + upgradePrizes[7];

    }



    public void CarryUpgrade()
    {
        
        if (money > upgradePrizes[0])
        {
            UpgradeComplete();
            startingCarryCapacityValueIncreasement++;
            currentValuetext[0].text = "" + startingCarryCapacityValueIncreasement;
            endValuetext[0].text = "" + (startingCarryCapacityValueIncreasement + 1);
            gameManager.MaxPizzasInBackPack++;
            gameManager.MoneyFromDemons -= upgradePrizes[0];
            upgradePrizes[0] += 30;
            LevelBalls[carryCapacityLevel].color = new Color(0, 255, 0, 255);
            carryCapacityLevel +=1;
            CheckForLevels();

        }
        else
        {
            
            NotEnoughMoney();
        }
    }
    public void LifeUpgrade()
    {
        
        if (money > upgradePrizes[1])
        {
            UpgradeComplete();
            startingLifeValueIncreasement +=lifeIncreasement;
            currentValuetext[1].text = "" + startingLifeValueIncreasement;
            endValuetext[1].text = "" + (startingLifeValueIncreasement + lifeIncreasement);
            attackCombosController.MaxHealth += lifeIncreasement;
            attackCombosController.Playerhealth += lifeIncreasement;
            gameManager.MoneyFromDemons -= upgradePrizes[1];
            upgradePrizes[1] += 35;
            lifeLevelBalls[lifeLevel].color = new Color(0, 255, 0, 255);
            lifeLevel += 1;
            CheckForLevels();

        }
        else
        {
            
            NotEnoughMoney();
        }
    }
    public void CompassUpgrade()
    {
        if (money > upgradePrizes[2])
        {
            UpgradeComplete();
            compassOn = true;
            compassLevelBalls.color = new Color(0, 255, 0, 255);
            CheckForLevels();
        }
        else
        {
            NotEnoughMoney();
        }
    }
     public void FaryUpgrade()
    {
        if (money > upgradePrizes[3])
        {
            UpgradeComplete();
            faryOn = true;
            faryLevelBall.color = new Color(0, 255, 0, 255);
            CheckForLevels();
        }
        else
        {
            NotEnoughMoney();
        }
    }
    public void DashNUpgrade()
    {

        if (money > upgradePrizes[4])
        {
            UpgradeComplete();
            currentValuetext[2].text = "" + (startingDashNValueIncreasement+1);
            endValuetext[2].text = "";
            characterController.MaxDashes++;
            gameManager.MoneyFromDemons -= upgradePrizes[4];           
            dashLevelBall.color = new Color(0, 255, 0, 255);
            UpgradeButtons[4].gameObject.SetActive(false);

        }
        else
        {

            NotEnoughMoney();
        }
    }
    public void DashCUpgrade()
    {

        if (money > upgradePrizes[5])
        {
            UpgradeComplete();
            currentValuetext[3].text = "" + (dashNewCooldown);
            endValuetext[3].text = "";
            characterController.GeneralDashCoolDown = 1f;
            gameManager.MoneyFromDemons -= upgradePrizes[5];
            dashCoolLevelBall.color = new Color(0, 255, 0, 255);
            UpgradeButtons[5].gameObject.SetActive(false);

        }
        else
        {

            NotEnoughMoney();
        }
    }
    public void AttackUpgrade()
    {

        if (money > upgradePrizes[6])
        {
            UpgradeComplete();
            startingattackValueIncreasement +=attackIncreasement;
            currentValuetext[4].text = "" + startingattackValueIncreasement;
            endValuetext[4].text = "" + (startingattackValueIncreasement+attackIncreasement);
            attackCombosController.AttackDamage += attackIncreasement;
            gameManager.MoneyFromDemons -= upgradePrizes[6];
            upgradePrizes[6] += 50;
            attackLevelBalls[attackLevel].color = new Color(0, 255, 0, 255);
            attackLevel += 1;
            CheckForLevels();

        }
        else
        {

            NotEnoughMoney();
        }
    }
    public void SpeedUpgrade()
    {
        if (money > upgradePrizes[7])
        {
            UpgradeComplete();
            startingspeedValueIncreasement += speedIncreasement;
            currentValuetext[5].text = "" + startingspeedValueIncreasement;
            endValuetext[5].text = "" + (startingspeedValueIncreasement + speedIncreasement);
            characterController.BaseSpeed += speedIncreasement;
            gameManager.MoneyFromDemons -= upgradePrizes[7];
            upgradePrizes[7] += 70;
            speedLevelBalls[speedLevel].color = new Color(0, 255, 0, 255);
            speedLevel += 1;
            CheckForLevels();
        }
        else
        {

            NotEnoughMoney();
        }

    }
    void UpgradeComplete()
    {
        int UpgradeSound = Random.Range(0, SonidosUpgrades.Length);
        SonidosUpgrades[UpgradeSound].Play();
    }

    void NotEnoughMoney()
    {
        int fartNoise = Random.Range(0,fartArray.Length);
        fartArray[fartNoise].Play();
    }

    public void QuitMenu()
    {
        gameObject.SetActive(false);
    }
}

