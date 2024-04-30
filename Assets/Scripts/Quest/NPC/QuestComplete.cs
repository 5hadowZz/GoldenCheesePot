using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestComplete : MonoBehaviour
{
    private Questable questable;
    private Talkable talkable;

    /// <summary>
    /// 完成任务的Btn点击后的函数
    /// </summary>
    public void OnQuestCompleted()
    {
        questable = DialogueMgr.Instance.dialogueNPC.GetComponent<Questable>();
        talkable = questable.talkable;

        // 完成任务的按钮消失
        UIMgr.Instance.questComplete.SetActive(false);

        // 显示任务完成后的对话
        DialogueMgr.Instance.dialoguePanel.SetActive(false);
        DialogueMgr.Instance.ShowDialogue(talkable.npc, talkable.completeQuestLines, () =>
        {
            // 删除任务的Mgr中存储、UI显示、背包中的物品
            questable.RemoveQuest();
            // 根据NPC和任务给予奖励
            GetReward(talkable.npc);
        });
    }


    // 给予奖励
    public void GetReward(E_DialogueNPC npc)
    {
        switch (npc)
        {
            case E_DialogueNPC.Grandma:
                questable.GetComponent<QuestReward_Grandma>().GetReward(questable.curQuestIndex);
                break;

            case E_DialogueNPC.Bear:
                questable.GetComponent<QuestReward_Bear>().GetReward(questable.curQuestIndex);
                break;

            case E_DialogueNPC.Fox:
                questable.GetComponent<QuestReward_Fox>().GetReward(questable.curQuestIndex);
                break;
        }
    }
}
