using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMgr : MonoBehaviour
{
    private static PlantMgr instance;
    public static PlantMgr Instance => instance;

    private GameObject parent;
    [HideInInspector]
    public List<Plant> plantList = new List<Plant>();

    [Header("植物生成相关")]
    public GameObject[] plants;
    public int maxNum = 10;
    public float spawnOffset;   // 被采摘后 要隔多久生成下一个
    [Header("植物消失相关")]
    public GameObject hole;
    public float holeLifeTime;
    [Header("植物生成区域")]
    public Vector2 center;
    public Vector2 size;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        parent = new GameObject("Plants");

        for (int i = 0; i < maxNum; i++)
        {
            SpawnPlant();
        }
    }


    /// <summary>
    /// 画植物生成区域
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }


    /// <summary>
    /// 生成植物
    /// </summary>
    public void SpawnPlant()
    {
        // 生成时 如已达到地图上最大植物数量  就不生成
        if (plantList.Count == maxNum)
            return;

        // 随机生成的植物的种类
        int index = Random.Range(0, plants.Length);

        // 随机生成不重叠的位置
        Vector2 spawnPos = SpawnPos();

        // 实例化植物  并更改父对象  使界面植物统一干净
        GameObject plantObj = Instantiate(plants[index].gameObject, spawnPos, Quaternion.identity);
        plantObj.transform.SetParent(parent.transform, false);
        // 存入List
        plantList.Add(plantObj.GetComponent<Plant>());
    }


    /// <summary>
    /// 在表中移除植物  植物消失时调用
    /// </summary>
    /// <param name="plant"></param>
    public void RemovePlant(Plant plant)
    {
        plantList.Remove(plant);    // 植物管理器列表中清除植物

        Destroy(plant.gameObject);
        GameObject hole = Instantiate(this.hole, plant.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);   // 生成洞
        hole.transform.SetParent(parent.transform, false);  // 更改洞的父对象

        Destroy(hole, holeLifeTime);                // 一段时间后消除洞
        Invoke("SpawnPlant", spawnOffset);                      // 一段时间后再次随机生成植物
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


    /// <summary>
    /// 装饰植物消失时调用
    /// </summary>
    public void RemoveOrnament(Ornament ornament)
    {
        StartCoroutine(RealRemoveOrnament(ornament));
    }
    private IEnumerator RealRemoveOrnament(Ornament ornament)
    {
        ornament.gameObject.SetActive(false);    // 隐藏装饰物
        GameObject hole = Instantiate(ornament.hole, ornament.transform.position, Quaternion.identity);   // 生成洞

        yield return new WaitForSeconds(ornament.holeLifeTime);      // 等待一段时间

        Destroy(hole);                           // 消除洞
        ornament.gameObject.SetActive(true);     // 显示装饰物
    }
}
