using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward_Grandma : MonoBehaviour
{
    [TextArea(1, 3)]
    public string[] lines1;
    [TextArea(1, 3)]
    public string[] lines2;
    [TextArea(1, 3)]
    public string[] lines3;
    [TextArea(1, 3)]
    public string[] lines4;


    public void GetReward(int questIndex)
    {
        switch (questIndex)
        {
            // 解锁花洒
            case 0:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, lines1, () =>
                {
                    UIMgr.Instance.potIcon.SetActive(true);
                });
                break;

            // 奖励热辣煎蛋
            case 1:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, lines2, () =>
                {
                    Item item = Resources.Load<GameObject>("BagItems/Shop_Bag_热辣煎蛋").GetComponent<Item>();
                    BagMgr.Instance.AddToBag(item);
                });
                break;


            // 强化武器
            case 2:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, lines3, () =>
                {
                    Player.Instance.atk += 2;
                });
                break;


            // 结局动画
            case 3:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Grandma, lines4, () =>
                {

                });
                break;
        }
    }
}
