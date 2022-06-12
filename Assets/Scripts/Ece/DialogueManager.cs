using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Dialogue;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogParent;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public AudioSource dialogueSource;
    public float delayBtwLines = 1f;

    public bool autoAdvance = true;
    private Queue<Line> sentences;

    private Movement player;

    private Dialogue currDialog;

    void Start()
    {
        dialogParent.SetActive(false);
        sentences = new Queue<Line>();

        player = FindObjectOfType<Movement>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        //player.DisablePlayerMovement();
        currDialog = dialogue;
        dialogParent.SetActive(true);
        sentences.Clear();

        foreach (var line in dialogue.lines)
        {
            sentences.Enqueue(line);
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

        var line = sentences.Dequeue();
        
        nameText.text = line.speaker;
        StopAllCoroutines();

        float audioDur = 0f;
        if(line.clip == null)
        {
            dialogueSource.Stop();
        }
        else
        {
            audioDur = line.clip.length;
            dialogueSource.PlayOneShot(line.clip);
        }
        StartCoroutine(TypeSentence(line.sentence, audioDur + delayBtwLines));
    }

    IEnumerator TypeSentence (string sentence, float minWaitTime)
    {
        dialogueText.text = "";
        float waitedTime = 0f;
        float delay = 0.07f;
         
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            waitedTime += delay;
            yield return new WaitForSeconds(delay);
        }

        float timeToWait = minWaitTime - waitedTime;

        Debug.LogFormat("Took {0} seconds to display, should wait {1} secs for dialog to finish.", waitedTime, timeToWait);
        if(timeToWait > 0)
        {
            yield return new WaitForSecondsRealtime(timeToWait);
        }

        if (autoAdvance)
        {
            DisplayNextSentence();
        }
    }

    void EndDialogue()
    {
        dialogParent.SetActive(false);

        if (currDialog.isLast)
        {
            FindObjectOfType<GameOverController>().OnGameOver();
            // game over!
            // player enable "space" input
            // show "Space to Scream" on UI
            Debug.Log("game over");
        }

        currDialog = null;
        //player.EnablePlayerMovement();
    }
}
