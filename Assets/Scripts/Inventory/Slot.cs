using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Item slotItem;
    public Image slotImage;

    private Vector3 originalSlotPos;


    private void OnEnable()
    {
        slotImage.raycastTarget = true;
    }


    public void UpdateSlot(Item _item)
    {
        if (_item == null)
        {
            slotItem = null;
            slotImage.sprite = null;
            slotImage.color = new Color(1, 1, 1, 0);
            return;
        }

        slotItem = _item;
        slotImage.sprite = _item.GetSprite();
        slotImage.color = new Color(1, 1, 1, 1);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalSlotPos = transform.position;               // 保存当前Slot的Pos  用于弹回Slot
        transform.position = eventData.position;    // 让Slot的Postion跟着鼠标走
        slotImage.raycastTarget = false;    // 关闭射线检测 让拖动时可以检测到该Slot下方的Slot
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;    // 让Slot的Postion跟着鼠标走
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        // 如果拖动的是空Slot  就弹回
        if (slotItem == null)
        {
            transform.position = originalSlotPos;
            slotImage.raycastTarget = true;
            return;
        }

        // 如果下方物体为Slot 交换
        if (eventData.pointerCurrentRaycast.gameObject.name.StartsWith("Slot"))     
        {
            GameObject target = eventData.pointerCurrentRaycast.gameObject;

            ExchangeIndex(BagMgr.Instance.slots.IndexOf(gameObject), BagMgr.Instance.slots.IndexOf(target));
        }
        // 如果下方物体为垃圾桶 删除物体 并弹回Slot
        else if (eventData.pointerCurrentRaycast.gameObject.name.StartsWith("Ash-bin"))
        {
            DeleteItem();
            transform.position = originalSlotPos;
        }
        // 否则 弹回
        else
        {            
            transform.position = originalSlotPos;
        }

        slotImage.raycastTarget = true;
    }


    /// <summary>
    /// 交换背包中两个物体
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    private void ExchangeIndex(int index1, int index2)
    {
        // 交换两个Slot在父物体下的位置
        Transform child1 = transform.parent.GetChild(index1);
        Transform child2 = transform.parent.GetChild(index2);
        child1.SetSiblingIndex(index2);
        child2.SetSiblingIndex(index1);

        // 交换两个Item在背包中的存储索引
        Item tempItem = BagMgr.Instance.items[index1];
        BagMgr.Instance.items[index1] = BagMgr.Instance.items[index2];
        BagMgr.Instance.items[index2] = tempItem;

        // 交换两个Slot在背包中的存储索引
        GameObject tempSlot = BagMgr.Instance.slots[index1];
        BagMgr.Instance.slots[index1] = BagMgr.Instance.slots[index2];
        BagMgr.Instance.slots[index2] = tempSlot;
    }


    /// <summary>
    /// 删除背包中物体
    /// </summary>
    private void DeleteItem()
    {
        BagMgr.Instance.AddToBag(null, BagMgr.Instance.slots.IndexOf(gameObject));
    }
}
