using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2_Statue : MonoBehaviour
{
    [TextArea]
    public string[] onAtkNotEnough;


    private void Start()
    {
        if (GameDataMgr.Instance.SceneData.Scene2_Statue_isDestroy)
        {
            DestroyImmediate(gameObject);
        }
    }
}
