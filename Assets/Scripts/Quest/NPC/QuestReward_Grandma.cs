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
                Item reward = Resources.Load<GameObject>("BagItems/Plant_Special_Bag_幽灵草").GetComponent<Item>();
                BagMgr.Instance.AddToBag(reward);
                break;

            // 解锁冲刺
            case 1:

                break;

            // 强化武器
            case 2:
                Player.Instance.atk += 2;
                break;

            // 结局动画
            case 3:
                break;
        }
    }
}
