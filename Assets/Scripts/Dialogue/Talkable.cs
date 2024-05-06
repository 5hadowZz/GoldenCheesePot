using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    public E_DialogueNPC npc;
    public GameObject ButtonE;
    [TextArea(1, 3)]
    public string[] randomLines;            // 随机闲聊
    [TextArea(1, 3)]
    public string[] delegateQuestLines_First;     // 发派第1个任务时的对话
    [TextArea(1, 3)]
    public string[] delegateQuestLines_Second;     // 发派第2个任务时的对话
    [TextArea(1, 3)]
    public string[] delegateQuestLines_Third;     // 发派第3个任务时的对话
    [TextArea(1, 3)]
    public string[] delegateQuestLines_Fourth;     // 发派第4个任务时的对话

    private bool canTalk;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !UIMgr.Instance.questPanel.activeInHierarchy && !DialogueMgr.Instance.dialoguePanel.activeInHierarchy)
        {
            canTalk = true;
            ButtonE.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
            ButtonE.SetActive(false);
        }
    }


    private void Update()
    {
        if (canTalk && Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Questable>().CheckQuest();

            DialogueMgr.Instance.dialogueNPC = gameObject;
        }
    }


    /// <summary>
    /// 弹出闲聊对话
    /// </summary>
    public void RandomChat()
    {
        string lines = randomLines[Random.Range(0, randomLines.Length)];
        DialogueMgr.Instance.ShowDialogue(npc, new string[] { lines });
    }
}
