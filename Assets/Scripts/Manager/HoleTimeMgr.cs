using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoleTimeMgr : MonoBehaviour
{
    private static HoleTimeMgr instance;
    public static HoleTimeMgr Instance => instance;

    // 外层是Zones数量  内层是Zone中的每个植物
    public Dictionary<string, List<List<Plant>>> zonesPlants = new();

    // 外层是Zones数量  内层是Zone中的每个洞
    public Dictionary<string, List<List<Hole>>> zonesHole = new();

    // 每个场景的独立区块是否初始化
    public Dictionary<string, bool> zoneIsInit = new();


    private void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// 添加植物到场景区块的植物字典中
    /// </summary>
    /// <param name="plant"></param>
    /// <param name="zoneIndex"></param>
    public void AddPlant(Plant plant, int zoneIndex)
    {
        if (!zonesPlants.ContainsKey(SceneLoadMgr.Instance.curScene.name))
        {
            zonesPlants.Add(SceneLoadMgr.Instance.curScene.name, new List<List<Plant>>());
        }

        while (zoneIndex >= zonesPlants[SceneLoadMgr.Instance.curScene.name].Count)
        {
            zonesPlants[SceneLoadMgr.Instance.curScene.name].Add(new List<Plant>());
        }

        zonesPlants[SceneLoadMgr.Instance.curScene.name][zoneIndex].Add(plant);
    }


    /// <summary>
    /// 添加洞到场景区块的洞字典中
    /// </summary>
    /// <param name="hole"></param>
    /// <param name="zoneIndex"></param>
    public void AddHole(Hole hole, int zoneIndex)
    {
        if (!zonesHole.ContainsKey(SceneLoadMgr.Instance.curScene.name))
        {
            zonesHole.Add(SceneLoadMgr.Instance.curScene.name, new List<List<Hole>>());
        }

        while (zoneIndex >= zonesHole[SceneLoadMgr.Instance.curScene.name].Count)
        {
            zonesHole[SceneLoadMgr.Instance.curScene.name].Add(new List<Hole>());
        }

        zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex].Add(hole);
    }


    public void RemovePlant(Plant plant, int zoneIndex)
    {
        zonesPlants[SceneLoadMgr.Instance.curScene.name][zoneIndex].Remove(plant);
    }


    public void RemoveHole(Hole hole, int zoneIndex)
    {
        zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex].Remove(hole);
    }
}
