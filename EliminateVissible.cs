using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateVissible : MonoBehaviour
{
    [SerializeField]CamaraFollow cam;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            cam.PosCam = new  Vector3(0,3.4f,-3);
            cam.cameraR.transform.Rotate(-37,0,0);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            cam.PosCam = new Vector3(0, 14, -8);
            cam.cameraR.transform.Rotate(37, 0, 0);
        }
    }


}   
