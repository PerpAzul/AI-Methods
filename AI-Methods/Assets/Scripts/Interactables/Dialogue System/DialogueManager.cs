using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    [SerializeField] private GameObject promptMessage;
    //[SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator characterAnimator;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private static readonly int isTalking = Animator.StringToHash("IsTalking");

    public bool isInDialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
        isInDialogue = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        promptMessage.SetActive(false);
        textAnimator.SetBool(IsOpen, true);
        characterAnimator.SetBool(isTalking, true);
        isInDialogue = true;
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
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    void EndDialogue()
    {
        textAnimator.SetBool(IsOpen, false);
        characterAnimator.SetBool(isTalking, false);
        isInDialogue = false;
        promptMessage.SetActive(true);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
}
