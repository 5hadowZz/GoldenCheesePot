using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Scene
{
    Scene1,
    Scene1_HideScene,
    Scene1_Shovel,
    Scene2_Main,
    Scene2_Branch,
    Scene3_Main,
    Scene3_Branch,
    Scene4_Main,
    Scene4_Branch,
    Test
}


public class AA_StartPanel : MonoBehaviour
{
    public E_Scene scene;
    public bool isTestMode;


    public void EnterGame()
    {        
        // GameDataMgr.Instance.Load();    // 读取数据  //GameDataMgr的Awake中已经Load了一次数据
        if (isTestMode)
        {
            SceneLoadMgr.Instance.Load(scene.ToString());
        }
        else
        {
            SceneLoadMgr.Instance.Load(GameDataMgr.Instance.SceneData.sceneName);
        }

        gameObject.SetActive(false);

        GameDataMgr.Instance.ItemDataLoadToBag();   // GameDataMgr中的背包物品数据  加载到背包
    }
}
