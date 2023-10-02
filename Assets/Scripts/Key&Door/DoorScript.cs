using System.Collections;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("Button Properties :")]
    public GameObject targetBlock;
    [Space(5)]
    public Vector2 offset = new Vector2(0f, 0f);
    [Space(5)]
    public float moveSpeed = 2.0f;
    [Space(5)]

    [Header("Button Interaction Control :")]
    public bool canOpen = false;
    public bool moved = false;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isInteracting = false;
    private Transform block;
    private BuddySystem buddySystem;
    private bool isActivePlayer;

    private void Start()
    {

        block = targetBlock.transform;

        // Store the initial position of the door
        initialPosition = block.position;
        // Calculate the target position based on the offset
        targetPosition = initialPosition + new Vector3(offset.x, offset.y, 0);
        buddySystem = FindObjectOfType<BuddySystem>();
    }

    private void Update()
    {
        // Check if the door can open based on whether the player is interacting with it
        if (isInteracting && Input.GetKeyDown(KeyCode.F) && isActivePlayer)
        {
            if (canOpen && !moved)
            {
                Debug.Log("Opening Door");
                StartCoroutine(MoveDoor(targetPosition));

            }
         
        }
        else if(!canOpen && isInteracting) 
        {

            Debug.Log("Key Not Found");

        }
        
           
        
    }

    IEnumerator MoveDoor(Vector3 target)
    {
        Vector3 currentPos = block.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(currentPos, target);

        while (block.position != target)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            block.position = Vector3.Lerp(currentPos, target, fractionOfJourney);

            yield return null;
        }

        moved = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an object tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set the "isInteracting" variable to true
            isInteracting = true;
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
        // Check if the collision is with an object tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set the "isInteracting" variable to false
            isInteracting = false;
        }
    }
}
