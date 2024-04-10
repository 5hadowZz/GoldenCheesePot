using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_QuestStatus
{
    Waiting,
    Accepted,
    Completed
}


[Serializable]
public class Quest
{
    public string questName;            // 任务名称
    public E_QuestStatus questStatus;   // 任务状态
    public string NpcQuestBgResPath;    // NPC显示在任务栏中的QuestLine背景在Resources中的位置
    // Info 任务所需物品和数量
    public List<Item> items;
    public List<int> itemNums;
}
