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
        if (collision.CompareTag("Player"))
        {
            SceneLoadMgr.Instance.sourceScene = SceneManager.GetActiveScene().name;           

            SceneLoadMgr.Instance.Load(gameObject.name);
        }
    }
}
