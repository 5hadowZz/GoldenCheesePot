using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantZone : MonoBehaviour
{
    [Header("植物生成区域")]
    public Vector2 center;
    public Vector2 size;

    [Header("植物生成相关")]
    public GameObject[] plants;
    public int maxNum;

    [Header("植物消失相关")]
    public GameObject normalHole;
    public GameObject ornamentHole;

    [Header("特殊植物相关")]
    public GameObject[] specialPlants;
    public GameObject specialHole;
    public float specialSpawnPro = -1f;


    /// <summary>
    /// 画植物生成区域
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }


    private void Start()
    {

        if (!HoleTimeMgr.Instance.zoneIsInit.ContainsKey(SceneManager.GetActiveScene().name + transform.GetSiblingIndex()))
        {
            Init();
            HoleTimeMgr.Instance.zoneIsInit.Add(SceneManager.GetActiveScene().name + transform.GetSiblingIndex(), true);
        }

        else
        {
            int zoneIndex = transform.GetSiblingIndex();

            if (zoneIndex < HoleTimeMgr.Instance.zonesPlants[SceneLoadMgr.Instance.curScene.name].Count)
            {
                for (int j = 0; j < HoleTimeMgr.Instance.zonesPlants[SceneLoadMgr.Instance.curScene.name][zoneIndex].Count; j++)
                {
                    // 加载资源实例
                    GameObject plantPrefab = Resources.Load<GameObject>(HoleTimeMgr.Instance.zonesPlants[SceneLoadMgr.Instance.curScene.name][zoneIndex][j].plantPrefabPath);
                    // 生成植物 设置父对象
                    GameObject plant = Instantiate(plantPrefab, transform);
                    // 改位置
                    plant.transform.position = HoleTimeMgr.Instance.zonesPlants[SceneLoadMgr.Instance.curScene.name][zoneIndex][j].pos;
                    // plantList中添加
                    PlantMgr.Instance.plantList.Add(plant.GetComponent<Plant>());

                    // HoleTimeMgr的zonesPlants中更改存储的Plant脚本
                    HoleTimeMgr.Instance.zonesPlants[SceneLoadMgr.Instance.curScene.name][zoneIndex][j] = plant.GetComponent<Plant>();
                }
            }

            // 相当于在新场景没有生成洞  字典中都没有改场景的洞List生成  那就return不生成洞
            if (!HoleTimeMgr.Instance.zonesHole.ContainsKey(SceneLoadMgr.Instance.curScene.name))
                return;

            if (zoneIndex < HoleTimeMgr.Instance.zonesHole[SceneLoadMgr.Instance.curScene.name].Count)
            {
                for (int j = 0; j < HoleTimeMgr.Instance.zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex].Count; j++)
                {
                    // 加载洞prefab资源
                    GameObject holePrefab = Resources.Load<GameObject>(HoleTimeMgr.Instance.zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex][j].holePrefabPath);
                    // 生成洞 设置父对象
                    GameObject holeObj = Instantiate(holePrefab, transform);
                    // 改位置
                    holeObj.transform.position = HoleTimeMgr.Instance.zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex][j].pos;
                    // 得到hole脚本
                    Hole hole = holeObj.GetComponent<Hole>();
                    // 更改Hole的lifeTime
                    hole.holeLifeTime = HoleTimeMgr.Instance.zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex][j].holeLifeTime - (Time.time - HoleTimeMgr.Instance.zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex][j].onExitTime);
                    // HoleTimeMgr的zonesHoles中更改存储的Hole脚本
                    HoleTimeMgr.Instance.zonesHole[SceneLoadMgr.Instance.curScene.name][zoneIndex][j] = hole;
                }
            }
        }
    }


    public void Init()
    {
        for (int i = 0; i < maxNum; i++)
        {
            SpawnPlant();
        }
    }


    /// <summary>
    /// 生成植物
    /// </summary>
    public void SpawnPlant()
    {
        // 生成时 如已达到Zone内最大植物数量  就不生成
        //if (transform.childCount >= maxNum)
        //return;

        // 随机生成的植物的种类
        int index = Random.Range(0, plants.Length);

        // 随机生成不重叠的位置
        Vector2 spawnPos = SpawnPos();

        // 实例化植物  并更改父对象  使界面植物统一干净
        float pro = Random.Range(0.0f, 1.0f);
        GameObject plantObj;
        if (pro < specialSpawnPro)
        {
            int specialIndex = Random.Range(0, specialPlants.Length);
            plantObj = Instantiate(specialPlants[specialIndex], spawnPos, Quaternion.identity);
        }
        else
        {
            plantObj = Instantiate(plants[index].gameObject, spawnPos, Quaternion.identity);
        }
        plantObj.transform.SetParent(transform, true);
        // 存入List
        PlantMgr.Instance.plantList.Add(plantObj.GetComponent<Plant>());
        // 存入大Mgr记录生成位置
        HoleTimeMgr.Instance.AddPlant(plantObj.GetComponent<Plant>(), transform.GetSiblingIndex());
    }


    /// <summary>
    /// 移除植物生成洞
    /// </summary>
    /// <param name="plant"></param>
    public void RemovePlant(Plant plant)
    {
        // 植物管理器列表中清除植物
        PlantMgr.Instance.plantList.Remove(plant);

        // HoleTimeMgr中删除植物
        HoleTimeMgr.Instance.RemovePlant(plant, transform.GetSiblingIndex());

        // 销毁植物
        Destroy(plant.gameObject);

        // 生成洞
        GameObject holePrefab;       
        if (plant.isOrnamentPlant) 
        {
            holePrefab = ornamentHole;
        }
        else if (plant.isSpecialPlant)
        {
            holePrefab = specialHole;
        }
        else
        {
            holePrefab = normalHole;
        }

        GameObject hole = Instantiate(holePrefab, plant.transform.position, Quaternion.identity);
        // 更改洞的父对象
        hole.transform.SetParent(transform, true);

        // 装进HoleTimeMgr中计算时间
        HoleTimeMgr.Instance.AddHole(hole.GetComponent<Hole>(), transform.GetSiblingIndex());
    }


    /// <summary>
    /// 随机生成植物生成位置
    /// </summary>
    /// <returns></returns>
    private Vector2 SpawnPos()
    {
        float randomX = Mathf.RoundToInt(Random.Range(center.x - size.x / 2, center.x + size.x / 2) / 0.0625f) * 0.0625f;
        float randomY = Mathf.RoundToInt(Random.Range(center.y - size.y / 2, center.y + size.y / 2) / 0.0625f) * 0.0625f;
        Vector2 spawnPos = new Vector2(randomX, randomY);

        spawnPos = CheckPos(spawnPos);

        return spawnPos;
    }


    /// <summary>
    /// 检测该位置是否有植物重叠   重叠了就重新生成位置
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <returns></returns>
    private Vector2 CheckPos(Vector2 spawnPos)
    {
        Collider2D collider = Physics2D.OverlapBox(spawnPos, 2 * Vector2.one, 0f, 1 << LayerMask.NameToLayer("Plant"));

        if (collider != null)
        {
            float randomX = Mathf.RoundToInt(Random.Range(center.x - size.x / 2, center.x + size.x / 2) / 0.0625f) * 0.0625f;
            float randomY = Mathf.RoundToInt(Random.Range(center.y - size.y / 2, center.y + size.y / 2) / 0.0625f) * 0.0625f;
            spawnPos = new Vector2(randomX, randomY);
            return CheckPos(spawnPos);
        }

        return spawnPos;
    }
}
