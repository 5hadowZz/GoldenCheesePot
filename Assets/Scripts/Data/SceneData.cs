using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    // 保存时场景
    public string sceneName;
    // Scene1中障碍物
    public bool Scene1_Obstacle_isDestroy;
    // 木中铲子场景
    public bool Scene1_Shovel_isTouch;
    // boss1
    public bool isFirstMeetBoss1 = true;
    // boss2
    public bool isFirstMeetBoss2 = true;
    // kill boss2
    public bool isKilledBoss2;
    // Scene4
    public bool isFirstEnterScene4_Branch = true;
}
