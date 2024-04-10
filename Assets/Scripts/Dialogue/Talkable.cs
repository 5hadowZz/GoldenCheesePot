using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    public E_DialogueNPC npc;
    [TextArea(1, 3)]
    public string[] lines;
    [TextArea(1, 3)]
    public string[] delegateQuestLines;     // 发派任务时的对话
    [TextArea(1, 3)]
    public string[] notCompleteQuestLines;   // 未完成任务时的对话
    [TextArea(1, 3)]
    public string[] completeQuestPreLines;   // 完成任务时 点击完成任务按钮前 的对话
    [TextArea(1, 3)]
    public string[] completeQuestLines;     // 完成任务 点击按钮后 的对话
    [TextArea(1, 3)]
    public string[] questMaxLines;   // 任务接满了时的对话
    [TextArea(1, 3)]
    public string[] questAllCompleteLines;   // 该NPC所有任务都完成时的对话


    private bool canTalk;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !UIMgr.Instance.questPanel.activeInHierarchy && !DialogueMgr.Instance.dialoguePanel.activeInHierarchy)
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
            DialogueMgr.Instance.ShowDialogue(npc, lines, () => { GetComponent<Questable>().CheckQuest(); });
            DialogueMgr.Instance.dialogueNPC = gameObject;
        }
    }
}
