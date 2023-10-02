using System.Collections;
using UnityEngine;

public class SingleButtonSystem : MonoBehaviour
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
    public bool Lock = false;
    public bool Pressed = false;

   
    private bool movingToDestination = false;
    private bool playerinrange;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool canPress = true;
    
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
        
        initialPosition = doorBlock.position;
        targetPosition = initialPosition + new Vector3(offset.x, offset.y, 0);

       
        cameraMovement = FindObjectOfType<CameraMovement>();
        gameManager = FindObjectOfType<GameManager>();
        buddySystem = FindObjectOfType<BuddySystem>();

        blockTravel = Vector2.Distance(initialPosition, targetPosition);
        camTravel = Vector2.Distance(transform.position,initialPosition);

        blockDelay = 2.5f; //Delaying block movement -> Compensating Cam movement (4f in the brackets is hard-coded)

        camDelay = (blockTravel / moveSpeed) + 2f;  //Delay cam return -> Waiting for the block to finish moving

       
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F) && playerinrange && isActivePlayer)
        {
            if(!movingToDestination && canPress && !Pressed)
            {
                

                StartCoroutine(BlockDelay(targetPosition, blockDelay));
                
                movingToDestination = true;
                canPress = false;
                Pressed = true;

            }
            
            else if (!movingToDestination && canPress)
            {
                movingToDestination = true;
                StartCoroutine(MoveDoor(targetPosition));
                canPress = false;

            }
            else if (movingToDestination && canPress && !Lock)
            {
                returnDoor();
                canPress = false;
            }
            
            
            
        }
    }

  

    private void returnDoor()
    {
        if (doorBlock.position == targetPosition)
        {
            StartCoroutine(MoveDoor(initialPosition));
        }
        else
        {
            StartCoroutine(MoveDoor(targetPosition));
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

        canPress = true;
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
}
