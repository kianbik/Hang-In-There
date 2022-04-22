using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredScript : MonoBehaviour
{

    //Components
    Animator injuredAnimator;

    //Player
    PlayerMovement player;

    //Picked Up Variables
    public bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        injuredAnimator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            PickedUp();
        }
    }

    void PickedUp()
    {
        transform.position = player.carryLocation.transform.position;
        transform.rotation = player.carryLocation.transform.rotation;
        injuredAnimator.SetBool("PickedUp", true);

    }
}
