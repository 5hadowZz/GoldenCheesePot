using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataMgr : MonoBehaviour
{
    private static GameDataMgr instance;
    public static GameDataMgr Instance => instance;


    public PlayerData playerData = new PlayerData();
    public SceneData sceneData = new SceneData();


    private void Awake()
    {
        // 读取数据
    }


    public void Save()
    {
        SaveSceneData();
        SavePlayerData();
    }

    public void Load()
    {
        // 读取玩家数据时 可能需要先Disable玩家  加载好场景后在打开  以防出现触发场景某区域事件
        LoadSceneData();
        LoadPlayerData();
    }


    /// <summary>
    /// 存储玩家数据
    /// </summary>
    /// <returns></returns>
    private void SavePlayerData()
    {   
        playerData.atk = Player.Instance.atk;
        playerData.hp = Player.Instance.curHP;
        playerData.posX = Player.Instance.transform.position.x;
        playerData.posY = Player.Instance.transform.position.y;

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
        sceneData.sceneName = SceneManager.GetActiveScene().name;

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(Application.persistentDataPath + "/SceneData.gqp");

        bf.Serialize(fs, sceneData);

        fs.Close();

        print(Application.persistentDataPath);
    }


    /// <summary>
    /// 读取玩家数据
    /// </summary>
    private void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.gqp"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = File.Open(Application.persistentDataPath + "/PlayerData.gqp", FileMode.Open);

            playerData = bf.Deserialize(fs) as PlayerData;

            fs.Close();

            Player.Instance.atk = playerData.atk;
            Player.Instance.curHP = playerData.hp;
            Player.Instance.transform.position = new Vector2(playerData.posX, playerData.posY);
        }  
    }


    /// <summary>
    /// 读取场景物体信息
    /// </summary>
    private void LoadSceneData()
    {
        if (File.Exists(Application.persistentDataPath + "/SceneData.gqp"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = File.Open(Application.persistentDataPath + "/SceneData.gqp", FileMode.Open);

            sceneData = bf.Deserialize(fs) as SceneData;

            fs.Close();

            SceneLoadMgr.Instance.Load(sceneData.sceneName);
        }       
    }
}
