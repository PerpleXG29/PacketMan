using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Queue<string> sentences;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;


    public Animator animator;
    private GameObject dialogueSystem;
    private GameManager gameManager;
    void Start()
    {
        dialogueSystem = GameObject.Find("DialogueSystem");
        gameManager = FindObjectOfType<GameManager>();


        sentences = new Queue<string>();
        dialogueSystem.SetActive(false);



    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueSystem.SetActive(true);

        //animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        gameManager.textUp = true;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {

            sentences.Enqueue(sentence);

        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            gameManager.textUp = false;

            dialogueSystem.SetActive(false);
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
}
