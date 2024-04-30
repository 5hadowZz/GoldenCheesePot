using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Well : MonoBehaviour
{
    public GameObject ButtonE;
    [TextArea]
    public string[] notes;

    private bool canInteract;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ButtonE.SetActive(true);
            canInteract = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ButtonE.SetActive(false);
            canInteract = false;
        }
    }


    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Note, notes);
            DialogueMgr.Instance.dialogueText.DOComplete();
        }
    }
}
