using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel_in_Wood : MonoBehaviour
{
    private bool canInteract = false;
    public Sprite wood;
    public GameObject tipE;
    [TextArea]
    public string[] tips;



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
            tipE.SetActive(false);
            GameDataMgr.Instance.SceneData.Scene1_Shovel_isTouch = true;
            GetComponent<SpriteRenderer>().sprite = wood;
            // 让Player可以攻击
            Player.Instance.canAttack = true;
            // 显示提示
            DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Tip, tips);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameDataMgr.Instance.SceneData.Scene1_Shovel_isTouch)
        {
            tipE.SetActive(true);
            canInteract = true;
        }     
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        tipE.SetActive(false);
        canInteract = false;
    }
}
