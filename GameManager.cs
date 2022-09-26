using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private float pizzasInBackPack;
    [SerializeField] private float maxPizzasInBackPack;
    [SerializeField] private Text numberOfBackpackPizzas;


    [SerializeField] private Text moneyfromMissionsText;   
    [SerializeField] private float moneyFromMissions;
    [SerializeField] private float totalMoneyFromMissions;
    public float TotalMoneyFromMissions
    {
        get { return totalMoneyFromMissions; }
        set { totalMoneyFromMissions = value;}
    }


    [SerializeField] private RectTransform boxCounterUi;
    [SerializeField] private float revivalCost;
    //
    [SerializeField] private float moneyFromDemons;
    [SerializeField] private Text textmoneyDemons;
    //BrujulaParaPizzas
    [SerializeField] 
    private Camera cam;
    //GameTime
    private bool Paused;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject UpgradesMenu;
    [SerializeField] AudioSource trumpets;
    //Game Map
    private bool Map;
    [SerializeField] GameObject MapMenu;

    //FadeInBlack
    [SerializeField] private Image blackScreen;
    private float alphaBlack = 1f;
    public float PizzasInBackPack   
    {
        get
        {
            return pizzasInBackPack;
        }
        set
        {
            pizzasInBackPack = value;
        }
    } //propiedad de las pizzas que van en la mochila
    public float MaxPizzasInBackPack
    {
        get 
        { 
            return maxPizzasInBackPack;
        }
        set
        {
            maxPizzasInBackPack = value;
        }
    } //maximo de pizza
    public float MoneyFromMissions
    {
        get { return moneyFromMissions;}
        set { moneyFromMissions = value;}
    }
    public float RevivalCost
    {
        get { return revivalCost;}
        set { revivalCost = value;}
    }
    public float MoneyFromDemons
    {
        get { return moneyFromDemons; }
        set { moneyFromDemons = value; }
    }


    [SerializeField]private bool toblack;
    
    private void Awake()
    {
        boxCounterUi = GameObject.Find("PizzaBox01").GetComponent<RectTransform>();       
    }
    private void Start()
    {
        toblack = true;
        PauseMenu.SetActive(false);
        Paused = false;
        MapMenu.SetActive(false);
        Map = false;
        Time.timeScale = 1;
        toblack = false;
        
    }
    private void Update()
    {      
        if (alphaBlack > 0 && !toblack)
        {
            var tempcolorb = blackScreen.color;
            tempcolorb.a = (alphaBlack -= Time.deltaTime);
            blackScreen.color = tempcolorb;
        }

        if (alphaBlack < 1 && toblack)
        {
            var tempcolorb = blackScreen.color;
            tempcolorb.a = (alphaBlack += Time.deltaTime);
            blackScreen.color = tempcolorb;
        }

              
        //Modularlo con funciones.
        Mathf.Round(moneyFromMissions);
        numberOfBackpackPizzas.text = "" + pizzasInBackPack;
        moneyfromMissionsText.text = "" + moneyFromMissions;
        textmoneyDemons.text = ""+ moneyFromDemons;
        CheckBoxes();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Paused) { PauseGame(); }
            else { UnpauseGame(); }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!Map) { MapGame(); }
            else { UnmapGame(); }
        }
        if (moneyFromMissions < 0) { moneyFromMissions = 0; }

    }
    void CheckBoxes()
    {
        if (pizzasInBackPack > 0 && pizzasInBackPack <= maxPizzasInBackPack)
        {
            boxCounterUi.sizeDelta = new Vector2(boxCounterUi.sizeDelta.x, (43 * pizzasInBackPack));
        }
        if(pizzasInBackPack <= 0) { boxCounterUi.sizeDelta = new Vector2(boxCounterUi.sizeDelta.x, 0); }
        if (pizzasInBackPack == 1) { boxCounterUi.sizeDelta = new Vector2(boxCounterUi.sizeDelta.x, 43); }
    }
    void PauseGame()
    {
        trumpets.Stop();
        UpgradesMenu.SetActive(false);
        Paused = true;
        Time.timeScale = 0;
        cam.GetComponent<AudioListener>().enabled = false;
        PauseMenu.SetActive(true);
    }
    public void UnpauseGame()
    {
        trumpets.Play();
        Paused = false;
        Time.timeScale = 1;
        cam.GetComponent<AudioListener>().enabled = true;
        PauseMenu.SetActive(false);
    }

    void MapGame()
    {
        Map = true;
        MapMenu.SetActive(true);
    }

    void UnmapGame()
    {
        Map = false;
        MapMenu.SetActive(false);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exitgame()
    {
        Application.Quit();
    }

    public void ToBlack()
    {
        toblack = true;
        var tempcolor = blackScreen.color;
        tempcolor.a = alphaBlack;
        blackScreen.color = tempcolor;
    }
    public void BlackOut()
    {
        toblack = false;
        var tempcolor = blackScreen.color;
        tempcolor.a = alphaBlack;
        blackScreen.color = tempcolor;
    }




    

}
