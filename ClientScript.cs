using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientScript : MonoBehaviour
{
    private bool hasOpen;
    private Animator animator;

    private void Start()
    {
        hasOpen = false;
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Door")
        {
            if (hasOpen)
            {
                hasOpen=false;
                animator.SetTrigger("Exit");
            }
            else
            {
                animator.SetTrigger("Enter");
                hasOpen = true;
            }
        }

    }




}
