using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeddlerMove : MonoBehaviour
{
    private float _clampingAngle = 90f;
    public bool isVisible = false;
    [SerializeField]private Camera _mainCamera;
    [SerializeField]private Transform _itSelf;
    [SerializeField]private Transform _cameraTransform;
    [SerializeField] private Transform[] PeddlerPoints;
    [SerializeField] private Transform Cecilio;
    [SerializeField] private GameObject pressE;
    [SerializeField] private GameObject UpgradesMenu;
    [SerializeField] private AudioSource disapearS;
    [SerializeField] private AudioSource music;

    [SerializeField] private AudioSource pop;


    private bool OnReach;
    [SerializeField]private float rangeReach;
    private bool moved; 
    [SerializeField]LayerMask whatIsPlayer;

    private void Awake()
    {
        moved = false;
              
    }
    private void Start()
    {
        music.Play();
    }

    private void Update()
    {
        
        if (Vector3.Angle(_itSelf.position - _cameraTransform.position, _cameraTransform.forward) > _clampingAngle)
            return;
        Vector2 screenPosition = _mainCamera.WorldToViewportPoint(_itSelf.position);
        if (screenPosition.x >= 0 && screenPosition.x <= 1 && screenPosition.y >= 0 && screenPosition.y <= 1)
        {
            isVisible = true;
            print("Peddlervisible");
            Visible();
        }
        else
        {
            isVisible = false;
            
            Novisible();
        }
    }

    private void Visible()
    {
        music.volume = 1;  
        OnReach = Physics.CheckSphere(transform.position, rangeReach, whatIsPlayer);
        if (OnReach)
        {
            pressE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                UpgradesMenu.SetActive(true);
                pop.Play();
                
            }
            if (OnReach && Input.GetKey(KeyCode.Backspace)) { UpgradesMenu.SetActive(false);} 

        }
        else
        {
            UpgradesMenu.SetActive(false);
            pressE.SetActive(false);
        }

        transform.LookAt(new Vector3(Cecilio.position.x, transform.position.y, Cecilio.position.z));
        moved = false;
    }
    void Novisible()
    {
        music.volume = .2f;
        if (!moved)
        {
            StartCoroutine(MovingFromPosition());
        }
        

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(.65f, 0, .45f, 0.4f);
        Gizmos.DrawSphere(transform.position,rangeReach);
    }

    
    IEnumerator MovingFromPosition()
    {
        disapearS.Play();
        yield return new WaitForSeconds(.2f);
        int movePos = Random.Range(0, (PeddlerPoints.Length));
        Vector3 movePosV = new Vector3(PeddlerPoints[movePos].position.x, 1.5f, PeddlerPoints[movePos].position.z);
        transform.position = movePosV;
        moved = true;
    }
}

