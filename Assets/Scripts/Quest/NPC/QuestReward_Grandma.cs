using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward_Grandma : MonoBehaviour
{
    public void GetReward(int questIndex)
    {
        switch (questIndex)
        {
            // 解锁花洒
            case 0:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, new string[] 
                {
                    "孩子，这花洒你拿去吧，兴许路上会有些用处。\n\n" +
                    "[获得花洒]" 
                }, () =>
                {
                    UIMgr.Instance.potIcon.SetActive(true);
                });
                break;

            // 奖励热辣煎蛋
            case 1:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, new string[]
                {
                    "真棒，奖励你一个煎蛋。\n\n" +
                    "[获得热辣煎蛋]"
                }, () =>
                {
                    Item item = Resources.Load<GameObject>("BagItems/Shop_Bag_热辣煎蛋").GetComponent<Item>();
                    BagMgr.Instance.AddToBag(item);
                });
                break;


            // 强化武器
            case 2:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, new string[]
                {
                    "真棒，奖励你!。\n\n" +
                    "[攻击力提升]"
                }, () =>
                {
                    Player.Instance.atk += 2;
                });
                break;


            // 结局动画
            case 3:

                break;
        }
    }
}
