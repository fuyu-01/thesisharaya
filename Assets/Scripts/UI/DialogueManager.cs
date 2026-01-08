using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Animator animator;

    private Queue<string> sentences;
    private NPCDialogueTrigger currentNPC;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(string npcName, List<string> dialogueSentences, NPCDialogueTrigger npcTrigger)
    {
        currentNPC = npcTrigger;

        animator.SetBool("IsOpen", true);
        nameText.text = npcName;

        sentences.Clear();
        foreach (string sentence in dialogueSentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
{
    Debug.Log("➡ DisplayNextSentence called");

    if (dialogueText == null)
    {
        Debug.LogError("❌ dialogueText is NULL!");
        return;
    }

    if (sentences == null)
    {
        Debug.LogError("❌ sentences queue is NULL!");
        return;
    }

    if (sentences.Count == 0)
    {
        Debug.Log("✅ Dialogue ended!");
        EndDialogue();
        return;
    }

    string sentence = sentences.Dequeue();
    dialogueText.text = sentence;
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

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("Dialogue Ended.");

        if (currentNPC != null)
            currentNPC.OnDialogueEnd();

        currentNPC = null;
    }
}
