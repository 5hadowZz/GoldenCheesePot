using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public string itemPath;
    public string spritePath;
}


public class Item : MonoBehaviour
{
    public ItemInfo info;


    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>(info.spritePath);
    }
}
