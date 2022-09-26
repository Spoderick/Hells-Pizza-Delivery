using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    [SerializeField]Transform Player;
    [SerializeField]Transform RayGeneration;
    public Vector3 PosCam;
    [SerializeField] private float SmoothValue = .12f;
    private RaycastHit hit;
    [SerializeField] private float range;
    public Transform cameraR;



    // Update is called once per frame
    void Update()
    {
        //CamCheck();
        Vector3 PosicionDeseada = Player.position + PosCam;
        Vector3 PosicionSuavizada = Vector3.Lerp(transform.position, PosicionDeseada, SmoothValue);
        transform.position = PosicionSuavizada;
        Debug.DrawRay(Player.position, PosCam,Color.green);
    

    }
    



}
