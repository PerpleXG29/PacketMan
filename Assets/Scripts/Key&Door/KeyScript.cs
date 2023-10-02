using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [Header("Reference :")]
    public GameObject KeyPad; // Reference to the Door GameObject
    [Space(5)]


    [Header("Card Move Effect :")]
    public float distance = 0.5f;
    [Space(3)]
    public float speed = 1f;
    private Vector2 initialPosition;
    private Vector2 tempPos = new Vector2();


    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        tempPos.x = initialPosition.x;
        tempPos.y = initialPosition.y + Mathf.Sin(Time.fixedTime * Mathf.PI * speed) * distance;
        transform.position = tempPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
           
            if (KeyPad != null)
            {
                // Get the DoorScript component from the referenced door
                DoorScript doorScript = KeyPad.GetComponent<DoorScript>();

       
                if (doorScript != null)
                {
                    doorScript.canOpen = true;
                }
                else
                {
                    Debug.LogWarning("The referenced door does not have a DoorScript component.");
                }
            }
            else
            {
                Debug.LogWarning("The 'KeyPad' reference is not set.");
            }

            // Destroy the key object
            Destroy(gameObject);
        }
    }
}
