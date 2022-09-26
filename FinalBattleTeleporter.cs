using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattleTeleporter : MonoBehaviour
{
    [SerializeField] private GameObject presE;
    [SerializeField] private Transform endPos;
    [SerializeField] private AudioSource pop;
    [SerializeField] float moneyforenter;

    [SerializeField] private AudioSource TeleportSound;
    [SerializeField] private AudioSource[] Devilsvoice;
    [SerializeField] private AudioSource[] fartNoises;
    [SerializeField] private Animator ceci;
    

    [SerializeField] private float secondsToTeleport;
    [SerializeField]private CharacterController control;
    [SerializeField]private CaracterMovement c;
    [SerializeField]GameManager g;
    private bool Onreach;


    private void Awake()
    {
        
    }
    void Start()
    {
        presE.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && Onreach && g.MoneyFromMissions >=600)
        {
            StartCoroutine(Teleport());
            g.MoneyFromMissions -= 150;
            c.enabled = false;
            Onreach = false;
            presE.SetActive(false);
            ceci.SetFloat("Blend", 0);

        }
        else if (Input.GetKeyDown(KeyCode.E) && Onreach && g.MoneyFromMissions < 600)
        {
            int pedo = Random.Range(0, fartNoises.Length);
            fartNoises[pedo].Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            pop.Play();
            presE.SetActive(true);
            Onreach = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            presE.SetActive(false);
            Onreach = false;
        }
    }

    IEnumerator Teleport()
    {
        g.ToBlack();
        int rand = Random.Range(0, Devilsvoice.Length);
        Devilsvoice[rand].Play();
        yield return new WaitForSeconds(secondsToTeleport);      
        TeleportSound.Play();
        control.enabled = false;
        control.transform.position = endPos.position;
        control.enabled = true;
        c.enabled = true;
        g.BlackOut();
    }


        


}
