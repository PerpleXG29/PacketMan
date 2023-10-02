using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool canMove = true;
    public bool canSwitch = true;
    public bool textUp = false;

    // Reference to the BuddySystem script
    private BuddySystem buddySystem;

    // Reference to the CameraMovement script
    private CameraMovement cameraMovement;
    

    private void Start()
    {
     
        buddySystem = FindObjectOfType<BuddySystem>();
        cameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void Update()
    {
        // Check if CameraTarget is not equal to players[activePlayerIndex].transform from BuddySystem
        if (cameraMovement.CameraTarget != buddySystem.players[buddySystem.activePlayerIndex].transform)
        {
            // Set canSwitch and canMove to false
            canSwitch = false;
            canMove = false;
            
           
        }

        if (textUp)
        {
            canSwitch = false;
            canMove = false;
        }

        else
        {
            // Set canSwitch and canMove to true when they are equal
            canSwitch = true;
            canMove = true;
        }


    }
}
