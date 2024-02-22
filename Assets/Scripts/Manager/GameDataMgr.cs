using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameDataMgr : MonoBehaviour
{
    private static GameDataMgr instance;
    public static GameDataMgr Instance => instance;


    private PlayerData playerData;
    private SceneData sceneData;

    public PlayerData PlayerData { get { return playerData; } set { playerData = value; } }
    public SceneData SceneData { get { return sceneData; } set { sceneData = value; } }



    private void Awake()
    {
        instance = this;
        // 读取数据
        Load();
    }


    public void Save()
    {
        SaveSceneData();
        SavePlayerData();
    }

    /// <summary>
    /// Load数据
    /// </summary>
    public void Load()
    {
        // 读取玩家数据时 可能需要先Disable玩家  加载好场景后在打开  以防出现触发场景某区域事件
        LoadSceneData();
        LoadPlayerData();
    }

    // Load数据同时加载场景
    public void ClickLoad()
    {
        LoadSceneData();
        LoadPlayerData();
        SceneLoadMgr.Instance.Load(sceneData.sceneName);
    }


    /// <summary>
    /// 存储玩家数据
    /// </summary>
    /// <returns></returns>
    private void SavePlayerData()
    {
        // 这里初始化到时候用Json读取  or  直接赋值
        PlayerData.atk = Player.Instance.atk;
        PlayerData.hp = Player.Instance.curHP;
        PlayerData.posX = Player.Instance.transform.position.x;
        PlayerData.posY = Player.Instance.transform.position.y;
        
        for (int i = 0; i < BagMgr.Instance.items.Count; i++)
        {
            if (BagMgr.Instance.items[i] == null)
            {
                PlayerData.bagItemInfos[i] = null;
                continue;
            }

            PlayerData.bagItemInfos[i] = BagMgr.Instance.items[i].info;
        }

        //-----------------------------------------------------
        
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(Application.persistentDataPath + "/PlayerData.gqp");

        bf.Serialize(fs, playerData);

        fs.Close();
    }


    /// <summary>
    /// 存储场景物体信息
    /// </summary>
    /// <returns></returns>
    private void SaveSceneData()
    {
        SceneData.sceneName = SceneManager.GetActiveScene().name;

        // -------------------------------------------------------------
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(Application.persistentDataPath + "/SceneData.gqp");

        bf.Serialize(fs, sceneData);

        fs.Close();
    }


    /// <summary>
    /// 读取玩家数据
    /// </summary>
    private void LoadPlayerData()
    {
        print(Application.persistentDataPath);
        // 不是第一次读取 直接从硬盘读
        if (File.Exists(Application.persistentDataPath + "/PlayerData.gqp"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = File.Open(Application.persistentDataPath + "/PlayerData.gqp", FileMode.Open);

            playerData = bf.Deserialize(fs) as PlayerData;

            fs.Close();
        }
        // 第一次读取 赋初始值
        else
        {
            PlayerData = new PlayerData();
            PlayerData.atk = Player.Instance.atk;
            PlayerData.hp = Player.Instance.maxHP;

            PlayerData.bagItemInfos = new();
            for (int i = 0; i < 9; i++)
            {
                playerData.bagItemInfos.Add(null);
            }

            return;
        }

        //-----------------------------------------------------------------------------------------------
        Player.Instance.atk = PlayerData.atk;
        Player.Instance.curHP = PlayerData.hp;
        // 这里可以等SceneLoadMgr中Load完毕，彻底黑屏时再设置position  同时Load中让人物Disable
        Player.Instance.transform.position = new Vector2(PlayerData.posX, PlayerData.posY);
    }


    /// <summary>
    /// 读取场景物体信息
    /// </summary>
    private void LoadSceneData()
    {
        // 不是第一次读取 直接从硬盘读
        if (File.Exists(Application.persistentDataPath + "/SceneData.gqp"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = File.Open(Application.persistentDataPath + "/SceneData.gqp", FileMode.Open);

            sceneData = bf.Deserialize(fs) as SceneData;

            fs.Close();
        }
        // 第一次读取 赋初始值
        else
        { 
            sceneData = new SceneData();
            sceneData.sceneName = "Scene1";

            return;
        }

        //--------------------------------------------------------------------------------------------------

    }
}
