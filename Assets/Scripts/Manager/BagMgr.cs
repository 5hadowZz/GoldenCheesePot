using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BagMgr : MonoBehaviour
{
    private static BagMgr instance;
    public static BagMgr Instance => instance;

    public bool isOpen = false;
    [Header("背包存储数据")]
    public List<Item> items = new List<Item>();
    [Header("背包显示")]
    public List<GameObject> slots = new List<GameObject>();
    public GameObject gridPrefab;
    public GameObject slotPrefab;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        gameObject.SetActive(false);

        // 开局生成背包数据数量个 的 插槽  并存储在slots的List中
        for (int i = 0; i < items.Count; i++)
        {
            slots.Add(Instantiate(slotPrefab));
            slots[i].transform.SetParent(gridPrefab.transform);
            slots[i].GetComponent<Slot>().UpdateSlot(null);
        }
    }


    public void AddToBag(Item _item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                // 添加到背包数据
                items[i] = _item;
                // 更新插槽显示
                slots[i].GetComponent<Slot>().UpdateSlot(_item);
                break;
            }
        }
    }

    /// <summary>
    /// 该重载用于 在读取数据直接添加到背包中指定位置时使用
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="index"></param>
    public void AddToBag(Item _item, int index)
    {
        // 添加到背包数据
        items[index] = _item;
        // 更新插槽显示
        slots[index].GetComponent<Slot>().UpdateSlot(_item);
    }
}
