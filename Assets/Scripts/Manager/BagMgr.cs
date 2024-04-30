using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("物品提示框")]
    public GameObject tip;

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


    public void AddToBag(Item _item, bool isBigPlant = false)
    {
        if (isBigPlant)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    // 添加到背包数据
                    items[i] = _item;
                    // 更新插槽显示
                    slots[i].GetComponent<Slot>().UpdateSlot(_item);
                }
            }
            return;
        }

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


    /// <summary>
    /// 删除背包中相应数量的物品
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="_num">要删除的物品的数量</param>
    public void DeleteItem(Item _item, int _num)
    {
        int num = _num;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
                continue;

            if (num != 0 && _item.info.itemName == items[i].info.itemName)
            {
                items[i] = null;
                slots[i].GetComponent<Slot>().UpdateSlot(null);
                --num;
            }
        }
    }

    /// <summary>
    /// 检查物品所需在背包中是否达到
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public bool CheckContainItems(Item[] items, int[] itemNums)
    {
        Dictionary<string, int> itemDic = new Dictionary<string, int>();

        // 遍历背包 往tempDic中添加东西   最后检测dic中物品是否>=所需物品
        foreach (Item item in this.items)
        {
            if (item == null)
            {
                continue;
            }

            if (itemDic.ContainsKey(item.info.itemName))
            {
                ++itemDic[item.info.itemName];
            }
            else
            {
                itemDic.Add(item.info.itemName, 1);
            }
        }

        // 遍历检测每个所需物品及其数量
        for (int i = 0; i < items.Length; i++)
        {
            // 如果背包中没有所需物品  或者  背包中物品数量 < 所需物品数量  返回false
            if (!itemDic.ContainsKey(items[i].info.itemName))
            {
                return false;
            }
            else
            {
                foreach (string itemName in itemDic.Keys)
                {
                    if (itemName == items[i].info.itemName)
                    {
                        if (itemDic[itemName] < itemNums[i])
                            return false;
                    }
                }
            }
        }

        return true;
    }


    /// <summary>
    /// 检查背包是否满了
    /// </summary>
    /// <returns></returns>
    public bool CheckBagMax()
    {
        foreach (Item item in items)
        {
            if (item == null)
                return false;
        }

        return true;
    }
}
