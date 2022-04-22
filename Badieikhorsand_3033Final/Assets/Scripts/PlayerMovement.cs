using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    //Movement variables
    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float rotationSens = 4;

    //Components
    Rigidbody rigidbody;
    Animator playerAnimator;
    PlayerInput playerInput;
    public GameObject carryLocation;
    public PauseMenuScript pauseMenu;
    
    //Movement references
    public Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector3 moveDirectionY = Vector3.zero;
    float playerRotation;

    //Game Manager
    
    //Carrying
    public bool isCarrying;
    public bool interactionPressed;
    public GameObject attachedInjured;

    //Hash
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isCarryingHash = Animator.StringToHash("IsCarrying");
    public readonly int isDaedHash = Animator.StringToHash("Dead");

    //Lose And Win
    public GameObject winCanvas;
    public GameObject LoseCanvas;
    float timer = 10.0f;
    public bool isAlive = true;
    bool audioPlayed = false;
    public int numberOfInjuredInMap;

    //Audio
    public AudioSource audioSrc;
    public AudioClip deathAudio;
    public AudioClip dropAudio1;
    public AudioClip dropAudio2;

    //UI
    public TextMeshProUGUI timerText;
    public GameObject timerUI;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        numberOfInjuredInMap = GameObject.FindGameObjectsWithTag("Injured").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuScript.isGamePaused && isAlive)
        {

            Move();


        }
 
        if ((playerInput.actions["Pause"].triggered))
        {
            pauseMenu.Pause();
        }
        if ((playerInput.actions["Interaction"].triggered))
        {
            interactionPressed = true;
        }
        else
            interactionPressed = false;

        if (numberOfInjuredInMap == 0)
        {
            winCanvas.SetActive(true);
            isAlive = false;
        }
    }

    public void Move()
    {
        moveDirection = transform.forward * inputVector.y;
        moveDirectionY = transform.right * inputVector.x;
        Vector3 movementDirection = moveDirection * (walkSpeed * Time.deltaTime);
        transform.position += movementDirection;
        transform.Rotate(Vector3.up * rotationSens * inputVector.x * Time.deltaTime);
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }
    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
    public void OnInteraction(InputValue value)
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
      

        //Entering In Injured Zone
        if (other.gameObject.tag == "Injured")
        {
            if (!isCarrying)
            {
                other.GetComponent<InjuredScript>().isPickedUp = true;
                attachedInjured = other.gameObject;
                isCarrying = true;
                other.gameObject.tag = "Untagged";
                playerAnimator.SetBool(isCarryingHash, isCarrying);
               
            }
            
        }
        if (other.gameObject.tag == "Tent")
        {
            if (isCarrying)
            {
                Destroy(attachedInjured);
                numberOfInjuredInMap--;
                isCarrying = false;
                playerAnimator.SetBool(isCarryingHash, isCarrying);

                int rand = Random.Range(1, 3);
                if (rand == 1)
                    audioSrc.clip = dropAudio1;
                if (rand == 2)
                    audioSrc.clip = dropAudio2;
                if (rand == 3)
                    audioSrc.clip = dropAudio1;

                audioSrc.Play();
            }
          
            
        }
    }

    private void OnTriggerStay(Collider other)
    {

        
        //Entering Toxic Fog
        if (other.gameObject.tag == "Fog")
        {
            if (timer > 0) 
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                audioSrc.clip = deathAudio;
                if (!audioSrc.isPlaying && !audioPlayed)
                {
                    audioSrc.Play();
                    audioPlayed = true;
                }
                
                isAlive = false;
                playerAnimator.SetTrigger("Dead");
                LoseCanvas.SetActive(true);
                timerUI.SetActive(false);
            }
            timerUI.SetActive(true);
            timerText.text = ((int)timer).ToString();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Fog")
        {
            timer = 10;
            timerUI.SetActive(false);
        }
    }
}
