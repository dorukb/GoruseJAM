using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    DialogueManager manager;
    private void Awake()
    {
        manager = FindObjectOfType<DialogueManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
        }

        Destroy(gameObject);
    }
    public void TriggerDialogue()
    {
        manager.StartDialogue(dialogue);
    }
}
