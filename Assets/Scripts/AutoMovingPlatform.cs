using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovingPlatform : MonoBehaviour
{

    public Vector2 offset = new Vector2(0f, 0f);
    [Space(5)]
    public float moveSpeed = 5f;
    [Space(5)]
    public float moveDelay = 0f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isMoving;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + new Vector3(offset.x, offset.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == initialPosition && !isMoving)
        {

            StartCoroutine(BlockDelay(targetPosition, moveDelay));
        }
        else if(transform.position == targetPosition && !isMoving)
        {

            StartCoroutine(BlockDelay(initialPosition, moveDelay));
        }
        else if(transform.position != targetPosition && transform.position != initialPosition) 
        {
            isMoving = true;
        }
    }

    IEnumerator MoveDoor(Vector3 target)
    {
        Vector3 currentPos = transform.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(currentPos, target);

        while (transform.position != target)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(currentPos, target, fractionOfJourney);

            yield return null;
        }

        isMoving = false;
    }

    IEnumerator BlockDelay(Vector3 target, float delay)
    {
       
        yield return new WaitForSeconds(delay);

        StartCoroutine(MoveDoor(target));
        
    }
}
