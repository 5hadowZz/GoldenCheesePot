using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward_Bear : MonoBehaviour
{
    public void GetReward(int questIndex)
    {
        switch (questIndex)
        {
            // 获得去第四关的钥匙(灵魂三明治)
            case 0:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, new string[]
                {
                    "做得好！奖励你。\n\n" +
                    "[获得灵魂三明治]"
                }, () =>
                {
                    Item item = Resources.Load<GameObject>("BagItems/Shop_Bag_灵魂三明治").GetComponent<Item>();
                    BagMgr.Instance.AddToBag(item);
                });
                break;


            // 获得隐藏boss钥匙一（豌豆饼）
            case 1:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, new string[]
                {
                    "做得好！奖励你。\n\n" +
                    "[获得豌豆饼]"
                }, () =>
                {
                    Item item = Resources.Load<GameObject>("BagItems/Shop_Bag_豌豆饼").GetComponent<Item>();
                    BagMgr.Instance.AddToBag(item);
                });
                break;
        }
    }
}
