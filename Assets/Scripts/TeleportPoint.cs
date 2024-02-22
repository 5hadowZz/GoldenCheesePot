using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPoint : MonoBehaviour
{
    private void Start()
    {
        if (SceneLoadMgr.Instance.sourceScene == gameObject.name)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = transform.position;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)     // 因为player身上有2个collider  这样写可以防止它触发2次该函数
        {
            SceneLoadMgr.Instance.sourceScene = SceneManager.GetActiveScene().name;
            SceneLoadMgr.Instance.Load(gameObject.name);
        }
    }
}
