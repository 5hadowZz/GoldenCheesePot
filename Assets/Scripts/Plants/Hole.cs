using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Hole : MonoBehaviour
{
    public float holeLifeTime;
    public float onExitTime;

    public string holePrefabPath;
    public Vector2 pos;


    private void Start()
    {
        pos = transform.position;
    }


    private void Update()
    {       
        if (holeLifeTime <= 0) 
        {
            // 销毁洞
            Destroy(gameObject);
            // HoleTimeMgr中移除hole
            HoleTimeMgr.Instance.RemoveHole(this, transform.parent.GetSiblingIndex());
            // 区域内再生成植物
            transform.parent.GetComponent<PlantZone>().SpawnPlant();
        }

        holeLifeTime -= Time.deltaTime;
    }


    private void OnDestroy()
    {
        onExitTime = Time.time;
    }
}
