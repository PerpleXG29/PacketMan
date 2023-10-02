using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float originalGravity;
    public float speed = 8f;
    public float jumpPower = 16f;

    private bool isFacingRight = true;

    public bool isActivePlayer = false; //Reference to the player-switcher

    private Transform playerTransform;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private CameraMovement cameraMovement;
    private GameManager gameManager;


    private void Awake()
    {
        originalGravity = rb.gravityScale;
        playerTransform = transform;

        gameManager = FindObjectOfType<GameManager>();
        cameraMovement = FindObjectOfType<CameraMovement>();
    }

    void Update()
    {
        if (!isActivePlayer) return; //*Safety*

        horizontal = Input.GetAxisRaw("Horizontal"); //Pressing A and D

        if(Input.GetButtonDown("Jump") && IsGrounded() && gameManager.canMove == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
           
        }

        Flip();
    }

    private void FixedUpdate()
    {
        
        if(cameraMovement.CameraTarget != playerTransform || gameManager.textUp == true)
        {
            rb.velocity = Vector2.zero;
           
        }
        
        if (!isActivePlayer)
        {
            rb.velocity = Vector2.zero; // Stop player movement when not active
            rb.gravityScale = 30f; //Boosted Gravity to compensate Vector2.Zero slowing down fall rate
            return;
        }

        else if (isActivePlayer)
        {
            rb.gravityScale = originalGravity;//*Safety*
        }

        if (gameManager.canMove == true)
        {

            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        }
    }

    private bool IsGrounded()
    {
       
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;

            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Make the player a child of the platform                      // These OnCollision Functions are there to make the active player
            transform.parent = collision.transform;                         // a child of the platform its standing on
        }                                                                   //
    }                                                                       // For some reason if they arent the child of the platform, they dont
                                                                            // follow the platform....
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Remove the player from being a child of the platform
            transform.parent = null;
        }
    }
}
