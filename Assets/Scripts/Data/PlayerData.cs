using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // 攻击力、位置、血量、能量、速度?
    public int atk;
    public float posX;
    public float posY;
    public int hp;
    //public int power;

    // 背包物品
    public List<ItemInfo> bagItemInfos;
}
