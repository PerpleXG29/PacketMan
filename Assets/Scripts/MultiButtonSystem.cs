using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MultiButtonSystem : MonoBehaviour
{
    // Button properties
    [Header("Button Properties :")]
    public GameObject targetBlock;
    private Transform doorBlock;
    [Space(5)]
    public Vector2 offset = new Vector2(0f, 0f);
    [Space(5)]
    public float moveSpeed = 5f;

    // Button interaction control
    [Space(10)]
    [Header("Button Interaction Control :")]
    private bool Lock = false;
    private bool Pressed = false;

    
    private bool playerinrange;
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    public bool canPress = true;
    private bool isForward = true;


    private bool isActivePlayer = false; //-> Checking if the object colliding with the button is the active player

    // References to other components
    private GameManager gameManager;
    private CameraMovement cameraMovement;
    private BuddySystem buddySystem;

    // Movement delays and distances
    private float blockDelay;
    private float camDelay;
    private float blockTravel;
    private float camTravel;


    private void Start()
    {
        doorBlock = targetBlock.transform;
        
        
        cameraMovement = FindObjectOfType<CameraMovement>();
        gameManager    = FindObjectOfType<GameManager>();
        buddySystem    = FindObjectOfType<BuddySystem>();
    }

    private void Update()
    {
       

        isMoving();

        currentPosition = doorBlock.position;

        if (isForward)
        {
            targetPosition = currentPosition + new Vector3(offset.x, offset.y, 0);
        }
        else if (!isForward)
        {
            targetPosition = currentPosition - new Vector3(offset.x, offset.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.F) && playerinrange && isActivePlayer)
        {
            
            blockTravel = Vector2.Distance(currentPosition, targetPosition);
            camTravel = Vector2.Distance(transform.position, currentPosition);

            blockDelay = 2.5f; //Delaying block movement -> Compensating Cam movement (4f in the brackets is hard-coded)
            camDelay = (blockTravel / moveSpeed) + 2f;  //Delay cam return -> Waiting for the block to finish moving


            if (canPress && !Pressed)
            {
                Debug.Log("First Time Press");
                StartCoroutine(BlockDelay(targetPosition, blockDelay));
                
                
                Pressed = true;

            }

            else if (canPress && Pressed)
            {
              
                StartCoroutine(MoveDoor(targetPosition));
                
                
            }
        }
    }

    IEnumerator MoveDoor(Vector3 target)
    {
        Vector3 currentPos = doorBlock.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(currentPos, target);

        while (doorBlock.position != target)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            doorBlock.position = Vector3.Lerp(currentPos, target, fractionOfJourney);

            yield return null;
        }


        if (isForward)
        {
            isForward = false;
        }

        else if (!isForward)
        {
            isForward = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerinrange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == buddySystem.players[buddySystem.activePlayerIndex])
        {
            isActivePlayer = true;
        }
        else
        {
            isActivePlayer = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerinrange = false;
        }
    }


    IEnumerator BlockDelay(Vector3 target, float delay)
    {
        cameraMovement.CameraTarget = doorBlock.transform;
        yield return new WaitForSeconds(delay);

        StartCoroutine(MoveDoor(target));
        StartCoroutine(CamDelay(camDelay));
    }

    IEnumerator CamDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        cameraMovement.CameraTarget = buddySystem.players[buddySystem.activePlayerIndex].transform;
    }

   
    private void isMoving()
    {
        if (doorBlock.transform.position != currentPosition && doorBlock.transform.position != targetPosition)
        {
            canPress = false;
        }
        else
        {
            canPress = true;
        }
    }
}









