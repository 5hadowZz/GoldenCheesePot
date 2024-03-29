using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public bool canPick;
    public Item item;

    private void Update()
    {
        if (canPick && Input.GetButtonDown("Pick"))
        {
            // 被采摘时  调用植物移除时的函数
            PlantMgr.Instance.RemovePlant(this);
            // 添加进背包
            BagMgr.Instance.AddToBag(item);
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
