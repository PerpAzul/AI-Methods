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

    [Header("Typewriter")]
    [Tooltip("Characters revealed per second.")]
    [SerializeField] private float charsPerSecond = 60f;

    [Tooltip("If true, pressing E while text is typing will instantly finish the current sentence.")]
    [SerializeField] private bool allowSkipTyping = true;

    private bool isTyping = false;
    private string currentSentence = "";

    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private static readonly int isTalking = Animator.StringToHash("IsTalking");

    public bool isInDialogue;

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
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // If we are mid-type and user triggers "next", finish instantly instead
        if (allowSkipTyping && isTyping)
        {
            FinishTypingInstantly();
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        isTyping = false;

        textAnimator.SetBool(IsOpen, false);
        characterAnimator.SetBool(isTalking, false);
        isInDialogue = false;
        promptMessage.SetActive(true);
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        float delay = (charsPerSecond <= 0f) ? 0f : 1f / charsPerSecond;

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;

            if (delay > 0f)
                yield return new WaitForSeconds(delay);
            else
                yield return null;
        }

        isTyping = false;
    }

    private void FinishTypingInstantly()
    {
        StopAllCoroutines();
        dialogueText.text = currentSentence;
        isTyping = false;
    }
}
