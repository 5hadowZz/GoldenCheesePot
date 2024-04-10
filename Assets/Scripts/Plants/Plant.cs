using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Plant : MonoBehaviour
{
    public bool canPick;
    public Item item;
    public bool isBigPlant;

    public string plantPrefabPath;
    public Vector2 pos;

    [Header("与Hole生成相关")]
    public bool isSpecialPlant;
    public bool isOrnamentPlant;


    private void Start()
    {
        pos = transform.position;    
    }



    private void Update()
    {
        if (canPick && Input.GetButtonDown("Pick"))
        {
            // 被采摘时  调用植物移除时的函数
            transform.parent.GetComponent<PlantZone>().RemovePlant(this);
            // 如果不是装饰植物 添加进背包
            if (!transform.CompareTag("Ornament"))
            {
                BagMgr.Instance.AddToBag(item, isBigPlant);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPick = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPick = false;
        }
    }
}
