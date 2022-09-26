using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClient : MonoBehaviour
{
    private Animator m_Animator;
    private bool doorOpen;
    private BoxCollider boxCol;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        doorOpen = false;
        boxCol.size = new Vector3(3.5f, 4.4f, 0);
        boxCol.center = new Vector3(1.2f, 1.1f, .09f);
        boxCol.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Client")
        {
            if (!doorOpen)
            {
                m_Animator.SetBool("DoorOpen", true);
                
                

            }
            else
            {
                m_Animator.SetBool("DoorOpen", false);
                doorOpen = false;
                boxCol.size = new Vector3(3.5f, 4.4f, 0);
                boxCol.center = new Vector3(1.2f, 1.1f, .09f);
            }
            
        }
    }
   
    

    [SerializeField] void BoxON()
    {
        boxCol.enabled = true;
    }    
    [SerializeField] void BoxOff()
    {
        boxCol.enabled = false;
    }
    [SerializeField] void DoorOpen()
    {
        boxCol.size = new Vector3(.3f, 4.4f, 3.5f);
        boxCol.center = new Vector3(0, .85f, 1.1f);
        boxCol.enabled = true;
        doorOpen = true;
    }
    
}
