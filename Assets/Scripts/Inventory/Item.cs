using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public string itemPath;
    public string spritePath;
    [TextArea(4, 8)]
    public string itemContent;
    public bool canUse;
    public bool isQuestItem;
}


public class Item : MonoBehaviour
{
    public ItemInfo info;


    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>(info.spritePath);
    }


    public void UseItem()
    {
        if (!info.canUse)
            return;

        if (info.itemName == "小熊面包")
        {
            Player.Instance.UpdateHP(6, true);
        }
        
        if (info.itemName == "热辣煎蛋")
        {
            Player.Instance.UpdateHP(10, true);
        }
    }
}
