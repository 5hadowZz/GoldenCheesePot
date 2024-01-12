using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [TextArea(1, 3)]
    public string[] lines;

    [SerializeField]
    private bool canTalk;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
        }
    }


    private void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.E))
        {
            DialogueMgr.Instance.ShowDialogue(lines);
        }
    }
}
