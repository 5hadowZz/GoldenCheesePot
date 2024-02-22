using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel_in_Wood : MonoBehaviour
{
    private bool canInteract = false;
    public Sprite wood;



    private void Start()
    {
        if (GameDataMgr.Instance.SceneData.Scene1_Shovel_isTouch)
        {
            GetComponent<SpriteRenderer>().sprite = wood;
        }
    }


    private void Update()
    {
        // 进入交互区域 且 按下交互键 且 未被交互过
        if (canInteract && Input.GetKeyDown(KeyCode.E) && !GameDataMgr.Instance.SceneData.Scene1_Shovel_isTouch)
        {
            GameDataMgr.Instance.SceneData.Scene1_Shovel_isTouch = true;
            GetComponent<SpriteRenderer>().sprite = wood;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }
}
