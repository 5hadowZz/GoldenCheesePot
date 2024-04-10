using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questable : MonoBehaviour
{
    public List<Quest> quests;
    public string NpcQuestBgResPath;
    [HideInInspector]
    public Talkable talkable;
    [HideInInspector]
    public Quest curQuest;      // 任务完成时的当前任务
    [HideInInspector]
    public int curQuestIndex;   // 任务完成时的当前任务索引


    private void Start()
    {
        talkable = GetComponent<Talkable>();
    }


    /// <summary>
    /// 循环遍历NPC所拥有的任务 检测每个任务的状态 来显示合适的对话
    /// 每次NPC对话结束 调用该回调检测任务完成、领取、分发等操作
    /// </summary>
    public void CheckQuest()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questStatus == E_QuestStatus.Accepted)
            {
                // 检测到accepted的任务已完成 显示一个button 点击之后执行完成任务的逻辑
                if (CheckQuestCompleted(quests[i]))
                {
                    curQuest = quests[i];
                    curQuestIndex = i;
                    DialogueMgr.Instance.ShowDialogue(talkable.npc, talkable.completeQuestPreLines);
                    UIMgr.Instance.questComplete.SetActive(true);
                }
                else
                {
                    DialogueMgr.Instance.ShowDialogue(talkable.npc, talkable.notCompleteQuestLines);
                }
                break;
            }

            else if (quests[i].questStatus == E_QuestStatus.Waiting)
            {
                DialogueMgr.Instance.ShowDialogue(talkable.npc, talkable.delegateQuestLines, () =>
                {
                    DelegateQuest(quests[i]);
                });


                break;
            }

            else if (i == quests.Count - 1)
            {
                // 执行所有任务完成后该有的事
                DialogueMgr.Instance.ShowDialogue(talkable.npc, talkable.questAllCompleteLines);
            }
        }
    }


    /// <summary>
    /// 检查某任务所需在背包中是否达到
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    private bool CheckQuestCompleted(Quest quest)
    {
        Dictionary<string, int> itemDic = new Dictionary<string, int>();

        // 遍历背包 往tempDic中添加东西   最后检测dic中物品是否>=quest中所需物品
        foreach (Item item in BagMgr.Instance.items)
        {
            if (item == null)
            {
                continue;
            }

            if (itemDic.ContainsKey(item.info.itemName))
            {
                ++itemDic[item.info.itemName];
            }
            else
            {
                itemDic.Add(item.info.itemName, 1);
            }
        }

        for (int i = 0; i < quest.items.Count; i++)
        {
            // 如果背包中没有任务所需物品  或者  背包中物品数量小于任务所需物品数量  返回false 即未完成任务
            if (!itemDic.ContainsKey(quest.items[i].info.itemName))
            {
                return false;
            }
            else
            {
                foreach (string itemName in itemDic.Keys)
                {
                    if (itemName == quest.items[i].info.itemName)
                    {
                        if (itemDic[itemName] < quest.itemNums[i])
                            return false;
                    }
                }
            }
        }

        return true;
    }


    /// <summary>
    /// 委派任务
    /// </summary>
    public void DelegateQuest(Quest quest)
    {
        if (QuestMgr.Instance.quests.Count >= 3)
        {
            // 弹出任务已达上限对话框
            DialogueMgr.Instance.ShowDialogue(talkable.npc, talkable.questMaxLines);
            return;
        }


        quest.questStatus = E_QuestStatus.Accepted;     // 改变任务状态
        QuestMgr.Instance.quests.Add(quest);             // 添加到全局任务字典中

        // 任务面板添加一项任务的UI显示
        UIMgr.Instance.AddQuest(quest);
    }


    /// <summary>
    /// 完成任务时 删除任务的Mgr中存储、UI显示、背包中的物品
    /// </summary>
    public void RemoveQuest()
    {
        // NPC的该任务状态改为完成
        curQuest.questStatus = E_QuestStatus.Completed;

        // UI中移除相应任务显示
        int index = QuestMgr.Instance.quests.IndexOf(curQuest);
        UIMgr.Instance.RemoveQuest(index);

        // 任务管理器中移除任务
        QuestMgr.Instance.quests.Remove(curQuest);

        // 背包中移除物品
        RemoveItems();

        // 置空目前可以提交的任务
        curQuest = null;
    }


    /// <summary>
    /// 删除背包中所需的任务物品
    /// </summary>
    private void RemoveItems()
    {
        if (curQuest == null)
            return;

        // 大循环 物品种类
        for (int i = 0; i < curQuest.items.Count; i++)
        {
            // 内部小循环 物品数量
            BagMgr.Instance.DeleteItem(curQuest.items[i], curQuest.itemNums[i]);
        }
    }
}
