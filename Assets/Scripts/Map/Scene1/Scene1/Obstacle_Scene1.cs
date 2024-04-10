using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Scene1 : MonoBehaviour
{
    private void Start()
    {
        if (GameDataMgr.Instance.SceneData.Scene1_Obstacle_isDestroy)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            GameDataMgr.Instance.SceneData.Scene1_Obstacle_isDestroy = true;
            Destroy(gameObject);
        }
    }
}
