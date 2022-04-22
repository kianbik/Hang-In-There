using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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


    //Carrying
    public bool isCarrying;
    public bool interactionPressed;

    //Hash
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isCarryingHash = Animator.StringToHash("IsCarrying");

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuScript.isGamePaused)
        {

            Move();


        }
        if ((playerInput.actions["Pause"].triggered))
        {
            pauseMenu.Pause();
        }
    }

    public void Move()
    {
        moveDirection = transform.forward * inputVector.y;
        moveDirectionY = transform.right * inputVector.x;
        Vector3 movementDirection = moveDirection * (walkSpeed * Time.deltaTime);
        transform.position += movementDirection;
        transform.Rotate(Vector3.up * rotationSens * inputVector.x);
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }
    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
    public void OnInteraction(InputValue value)
    {
        interactionPressed = value.isPressed;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Entering Toxic Fog
        if(other.gameObject.tag == "Fog")
        {
            Debug.Log("Entered Fog");
        }

        //Entering In Injured Zone
        if (other.gameObject.tag == "Injured")
        {
            if (playerInput.actions["Interaction"].triggered && !isCarrying)
            {
                other.GetComponent<InjuredScript>().isPickedUp = true;
                
                isCarrying = true;
                playerAnimator.SetBool(isCarryingHash, isCarrying);
            }
            
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Injured")
        {
            if (playerInput.actions["Interaction"].triggered && !isCarrying)
            {
                other.GetComponent<InjuredScript>().isPickedUp = true;
                
                isCarrying = true;
                playerAnimator.SetBool(isCarryingHash, isCarrying);
            }
         
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Fog")
        {
            Debug.Log("Exit from Fog");
        }
    }
}
