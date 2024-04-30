using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward_Fox : MonoBehaviour
{
    public void GetReward(int questIndex)
    {
        switch (questIndex)
        {
            // 移速提升
            case 0:
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Fox, new string[]
                {
                    "噢！是它！太感谢你了！",
                    "....",
                    "过来，我教你一招...\n\n\n" +
                    "[移速提升]"
                }, () =>

                {
                    Player.Instance.maxSpeed += 8;
                    Player.Instance.curSpeed = Player.Instance.maxSpeed;
                    // 由于下一个任务是对话类任务 所以直接分发了 下次找狐狸直接就触发对话
                    Questable questable = GetComponent<Questable>();
                    questable.DelegateQuest(questable.quests[1]);
                });
                break;


            // 对话  获得隐藏boss钥匙2 ：信件
            case 1:
                if (BagMgr.Instance.CheckBagMax())
                {
                    DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Fox, new string[]
                    {
                        "还有件事儿，不过...",
                        "你的口袋似乎有些满啊，清理过后再来找我吧。"
                    });
                }
                else
                {
                    DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Fox, new string[]
                    {
                        "噢，对了。",
                        "这有封信，有空的话，麻烦把它放进不远处的信箱里吧。\n\n" +
                        "[获得信件]"
                    }, () =>

                    {
                        Item item = Resources.Load<GameObject>("BagItems/Quest_Bag_信件").GetComponent<Item>();
                        BagMgr.Instance.AddToBag(item);
                        GetComponent<Questable>().RemoveNoItemQuest();
                    });
                }

                break;
        }
    }
}
