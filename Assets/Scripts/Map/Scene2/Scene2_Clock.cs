using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2_Clock : MonoBehaviour
{
    private bool canGet;
    public GameObject buttonE;


    private void Update()
    {
        if (canGet && Input.GetKeyDown(KeyCode.E))
        {
            canGet = false;
            GameDataMgr.Instance.SceneData.Scene2_Clock_isGet = true;
            buttonE.SetActive(false);

            Item item = Resources.Load<GameObject>("BagItems/Quest_Bag_闹钟").GetComponent<Item>();
            BagMgr.Instance.AddToBag(item);

            Destroy(gameObject);
        }
    }


    private void Start()
    {
        if (GameDataMgr.Instance.SceneData.Scene2_Statue_isDestroy && GameDataMgr.Instance.SceneData.Scene2_Clock_isGet)
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameDataMgr.Instance.SceneData.Scene2_Statue_isDestroy)
        {
            canGet = true;
            buttonE.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canGet = false;
            buttonE.SetActive(false);
        }
    }
}
