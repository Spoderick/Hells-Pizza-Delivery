using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MissionStarter : MonoBehaviour
{
    
    [SerializeField] private AudioSource PopSound;
    [SerializeField] private GameObject presE;

    [SerializeField]Missions_Manager Missions;

    private bool activeState;


    private void Start()
    {
        activeState = false;
        presE.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && activeState)
        {
            Missions.startMissions = true;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Missions.startMissions == false)
        {
            presE.SetActive(true);
            PopSound.Play();
            activeState = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        presE.SetActive(false);
        activeState = false;
    }
}
