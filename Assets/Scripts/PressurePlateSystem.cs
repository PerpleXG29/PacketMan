using System.Collections;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour
{
    [Header("References :")]
    // References to other game objects and components
    public GameObject TargetBlock;
    [Space(5)]
    private Transform platform;
    private CameraMovement cameraMovement;
    private BuddySystem buddySystem;

    [Header("Button Properties :")]
    // Movement related variables
    public Vector2 offset = new Vector2(0f, 0f); // Offset from the initial position
    [Space(5)]
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    public float moveSpeed = 5f; // Adjust this as needed
    [Space(5)]


    [Header("Development :")]
    // Player interaction variables
    public int minCount; // Minimum number of players needed
    private int playerCount = 0; // Current number of players on the pressure plate
    private bool countCheck = false;
    public bool Triggered = false;
   

    // Delays

    private float blockDelay = 2.5f;
    private float blockTravel;
    private float camDelay;


    private void Start()
    {
        platform = TargetBlock.transform;
        
        initialPosition = platform.position;
        // Calculate the target position based on offsets
        targetPosition = initialPosition + new Vector3(offset.x, offset.y, 0);

        cameraMovement = FindObjectOfType<CameraMovement>();
        buddySystem = FindObjectOfType<BuddySystem>();

        blockTravel = Vector2.Distance(initialPosition, targetPosition);
        camDelay = (blockTravel / moveSpeed) + 2f;

    }

    private void Update()
    {
        
        if (countCheck)
        {
            if (!Triggered)
            {
                cameraMovement.CameraTarget = platform.transform;
                StartCoroutine(BlockDelay(blockDelay));
             
                
            }
            else
            {
                MovePlatform(targetPosition);
            }
           
        }
        else
        {
            MovePlatform(initialPosition);
        }
    }

    private void MovePlatform(Vector3 destination)
    {
        platform.position = Vector3.MoveTowards(platform.position, destination, Time.deltaTime * moveSpeed);
    }

    // OnTriggerEnter2D is called when a player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
            playerCount++;

            Debug.Log(playerCount);
            if (playerCount >= minCount)
            {
                countCheck = true;
            }
            else if (playerCount < minCount)
            {
                countCheck = false;
            }
        }
    }

    // OnTriggerExit2D is called when a player exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            playerCount--;
            if (playerCount >= minCount)
            {
                countCheck = true;
            }
            else if (playerCount < minCount)
            {
                countCheck = false;
            }
        }
    }

    // Coroutine to trigger the platform with a delay
    IEnumerator BlockDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        MovePlatform(targetPosition);
        StartCoroutine(CamDelay(camDelay));

        Triggered = true;

    }

    IEnumerator CamDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        cameraMovement.CameraTarget = buddySystem.players[buddySystem.activePlayerIndex].transform;
    }

    
}
