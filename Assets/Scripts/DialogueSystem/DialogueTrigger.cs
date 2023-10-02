using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("UI Elements :")]
    public Dialogue dialogue;
    [Space(8)]

    [Header("Development :")]
    public bool invisible;
    [Space(5)]
    public bool repeating;

    private bool inRange;

    public bool ActiveTrigger = false;
    private BuddySystem buddySystem;
    private DialogueManager dialogueManager;
    private bool invisibleBool; // Safety //

    public Image[] characterImages; // Add this variable for character images

    private int currentImageIndex = 0; // Add this variable to track the current image index

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        buddySystem = FindObjectOfType<BuddySystem>();

        // Disable all character images at the beginning
        foreach (var characterImage in characterImages)
        {
            characterImage.enabled = false;
        }
    }

    public void Update()
    {
        if (inRange && ActiveTrigger)
        {
            if (!repeating && invisible && !invisibleBool)
            {
                TriggerDialogue();
                invisibleBool = true;

                // Enable the first character image
                currentImageIndex = 0; // Reset the current index
                if (characterImages.Length > 0)
                {
                    characterImages[currentImageIndex].enabled = true;
                }
            }

            if (repeating && Input.GetKeyDown(KeyCode.F) && !invisible)
            {
                TriggerDialogue();

                // Enable the first character image
                currentImageIndex = 0; // Reset the current index
                if (characterImages.Length > 0)
                {
                    characterImages[currentImageIndex].enabled = true;
                }
            }

            if (!repeating && Input.GetKeyDown(KeyCode.F) && !invisible)
            {
                TriggerDialogue();

                // Enable the first character image
                currentImageIndex = 0; // Reset the current index
                if (characterImages.Length > 0)
                {
                    characterImages[currentImageIndex].enabled = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                NextSentence();
                Debug.Log("NextSentence");

                // Disable the current character image
                if (currentImageIndex < characterImages.Length)
                {
                    characterImages[currentImageIndex].enabled = false;
                }

                // Increment the current index
                currentImageIndex++;

                // Check if we reached the end of the array
                if (currentImageIndex >= characterImages.Length)
                {
                    currentImageIndex = 0; // Reset to the beginning
                }

                // Enable the next character image if it exists
                if (currentImageIndex < characterImages.Length)
                {
                    characterImages[currentImageIndex].enabled = true;
                }
            }
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void NextSentence()
    {
        FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == buddySystem.players[buddySystem.activePlayerIndex])
        {
            ActiveTrigger = true;
        }
        else
        {
            ActiveTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
