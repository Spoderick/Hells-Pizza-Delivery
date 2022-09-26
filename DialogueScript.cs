using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text nameCharacter;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField]private BoxCollider boxCollider;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool RepeatForever;
    [SerializeField] private string CharacterName;
    [SerializeField] private string[] dialogue;
    [SerializeField] private AudioSource pop;


    //Voice
    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioSource soundStart;
    
    [SerializeField]private bool haveAudio;
    [SerializeField] private float textSpeed;

    
    [SerializeField] private GameObject pressEtext;
    [SerializeField] private GameObject Player;
    Animator cecilio;
    private int currenttext;
    private int MaxStrings;
    private bool onrange;
    private bool textvaild;
    private bool endededialogue;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        cecilio = Player.GetComponent<Animator>();
    }
    void Start()
    {

        MaxStrings = dialogue.Length;
        dialogueBox.SetActive(false); 
        nameCharacter.text = string.Empty;
        
        textvaild = true;
        endededialogue = true;
        pressEtext.SetActive(false);
        dialogueText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !textvaild)
        {
            if(dialogueText.text == dialogue[currenttext])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogue[currenttext];
                
                
               
            }

        }

        if (Input.GetKeyDown(KeyCode.E) && onrange && textvaild && !endededialogue)
        {
            if (haveAudio) { sound.Play(); }
            textvaild = false;
            pressEtext.SetActive(false);
            StartDialogue();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            endededialogue = false;
            if (isInteractable)
            {
                pressEtext.SetActive(true);
                onrange = true;
                pop.Play();
                
            }
            else
            {
                if (haveAudio) { sound.Play(); }
                StartDialogue();
                textvaild = false;
                dialogueBox.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currenttext = 0;
            dialogueText.text = string.Empty;
            if (isInteractable)
            {
                pressEtext.SetActive(false);
                onrange = false;

            }
        }
    }


    void StartDialogue()
    {
        nameCharacter.text = CharacterName;
        dialogueText.text = string.Empty;
        soundStart.Play();
        dialogueBox.SetActive(true);
        cecilio.SetFloat("Blend", 0);
        Player.GetComponent<CaracterMovement>().enabled = false;
        currenttext =0;
        StartCoroutine(TypeEffect());
        
    }

    IEnumerator TypeEffect()
    {
        foreach (char c in dialogue[currenttext].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(currenttext < dialogue.Length-1)
        {
            currenttext++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeEffect());
        }
        else
        {
            
            if (!RepeatForever)
            {
                Player.GetComponent<CaracterMovement>().enabled = true;
                dialogueBox.SetActive(false);
                Destroy(gameObject);
                //gameObject.SetActive(false);///////

            }
            else
            {
                endededialogue = true;
                Player.GetComponent<CaracterMovement>().enabled = true;
                dialogueBox.SetActive(false);
                textvaild = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        Gizmos.DrawCube(transform.position, new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z));
    }



}
