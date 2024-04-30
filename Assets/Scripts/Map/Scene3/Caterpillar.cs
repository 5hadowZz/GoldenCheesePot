using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caterpillar : MonoBehaviour
{
    public GameObject buttonE;
    [TextArea]
    public string[] lines;
    [TextArea]
    public string[] checkHaveSandwich;
    public Sprite after;

    private bool canInteract;
    private bool isOver;


    private void Awake()
    {
        if (GameDataMgr.Instance.SceneData.isOverCaterpillar)
            DestroyImmediate(gameObject);
    }


    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Caterpillar, lines, CheckSandwich);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isOver)
        {
            buttonE.SetActive(true);
            canInteract = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            buttonE.SetActive(false);
            canInteract = false;
        }
    }


    public void CheckSandwich()
    {
        Item sandwich = Resources.Load<GameObject>("BagItems/Shop_Bag_灵魂三明治").GetComponent<Item>();

        if (!BagMgr.Instance.CheckContainItems(new Item[] { sandwich }, new int[] { 1 }))
            return;

        DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Caterpillar, checkHaveSandwich, () =>
        {
            buttonE.SetActive(false);

            Destroy(GetComponent<Animator>());
            GetComponent<SpriteRenderer>().sprite = after;      // 改毛虫样子
            GetComponentInChildren<Smoke>().isSmoke = false;    // 不吐烟圈 不设阻挡
            BagMgr.Instance.DeleteItem(sandwich, 1);            // 删除背包物品
            isOver = true;
            GameDataMgr.Instance.SceneData.isOverCaterpillar = true;    // 更改全局完成的布尔
            DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Tip, new string[] { "[失去灵魂三明治]" }); // 提示玩家
        });

    }
}
