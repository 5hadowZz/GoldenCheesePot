using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMgr : MonoBehaviour
{
    private static PlantMgr instance;
    public static PlantMgr Instance => instance;

    //[HideInInspector]
    public List<Plant> plantList = new List<Plant>();


    private void Awake()
    {
        instance = this;
    }
}
