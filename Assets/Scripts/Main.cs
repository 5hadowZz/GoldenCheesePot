using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum E_Scene
{
    Scene1,
    Scene1_HideScene,
    Scene1_Shovel,
    Test
}


public class Main : MonoBehaviour
{
    public E_Scene scene;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // 新的游戏 加载设定好的场景 （测试用）
        //SceneManager.LoadScene(scene.ToString());
        // 继续游戏 加载数据中保存的场景  在GameDataMgr中进行Load

    }
}
