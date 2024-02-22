using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AA_StartPanel : MonoBehaviour
{
    public void EnterGame()
    {
        SceneLoadMgr.Instance.Load(GameDataMgr.Instance.SceneData.sceneName);
        gameObject.SetActive(false);


        //读取数据后AddToBag 使用其重载 添加到背包指定位置
        for (int i = 0; i < GameDataMgr.Instance.PlayerData.bagItemInfos.Count; i++)
        {
            if (GameDataMgr.Instance.PlayerData.bagItemInfos[i] == null)
            {
                BagMgr.Instance.AddToBag(null, i);
                continue;
            }

            BagMgr.Instance.AddToBag(Resources.Load<Item>(GameDataMgr.Instance.PlayerData.bagItemInfos[i].itemPath), i);
        }
    }
}
