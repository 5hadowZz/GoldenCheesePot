using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("买东西后的话")]
    public string[] randomLines;
    [Header("小熊面包材料")]
    public Item[] items_1;
    public int[] nums_1;
    [Header("热辣煎蛋材料")]
    public Item[] items_2;
    public int[] nums_2;
    [Header("灵魂三明治材料")]
    public Item[] items_3;
    public int[] nums_3;
    [Header("豌豆饼材料")]
    public Item[] items_4;
    public int[] nums_4;


    private Item shopItem1;
    private Item shopItem2;
    private Item shopItem3;
    private Item shopItem4;


    private void Awake()
    {
        shopItem1 = Resources.Load<GameObject>("BagItems/Shop_Bag_小熊面包").GetComponent<Item>();
        shopItem2 = Resources.Load<GameObject>("BagItems/Shop_Bag_热辣煎蛋").GetComponent<Item>();
        shopItem3 = Resources.Load<GameObject>("BagItems/Shop_Bag_灵魂三明治").GetComponent<Item>();
        shopItem4 = Resources.Load<GameObject>("BagItems/Shop_Bag_豌豆饼").GetComponent<Item>();
    }


    public void BuyItem(int id)
    {
        switch (id)
        {
            case 1:
                // 材料不够就显示对话
                if(!BagMgr.Instance.CheckContainItems(items_1, nums_1))
                {
                    DialogueMgr.Instance.dialoguePanel.SetActive(false);
                    DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Bear, new string[] { "噢...食材不够我可是没法儿做的。" });
                }
                // 够就删除材料 添加物品 显示对话
                else
                {
                    for (int i = 0; i < items_1.Length; i++)
                    {
                        BagMgr.Instance.DeleteItem(items_1[i], nums_1[i]);
                    }
                    BagMgr.Instance.AddToBag(shopItem1);

                    RandomChat();
                }
                break;


            case 2:
                // 材料不够就显示对话
                if (!BagMgr.Instance.CheckContainItems(items_2, nums_2))
                {
                    DialogueMgr.Instance.dialoguePanel.SetActive(false);
                    DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Bear, new string[] { "噢...食材不够我可是没法儿做的。" });
                }
                // 够就删除材料 添加物品 显示对话
                else
                {
                    for (int i = 0; i < items_2.Length; i++)
                    {
                        BagMgr.Instance.DeleteItem(items_2[i], nums_2[i]);
                    }
                    BagMgr.Instance.AddToBag(shopItem2);

                    RandomChat();
                }
                break;


            case 3:
                // 材料不够就显示对话
                if (!BagMgr.Instance.CheckContainItems(items_3, nums_3))
                {
                    DialogueMgr.Instance.dialoguePanel.SetActive(false);
                    DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Bear, new string[] { "噢...食材不够我可是没法儿做的。" });
                }
                // 够就删除材料 添加物品 显示对话
                else
                {
                    for (int i = 0; i < items_3.Length; i++)
                    {
                        BagMgr.Instance.DeleteItem(items_3[i], nums_3[i]);
                    }
                    BagMgr.Instance.AddToBag(shopItem3);

                    RandomChat();
                }
                break;


            case 4:
                // 材料不够就显示对话
                if (!BagMgr.Instance.CheckContainItems(items_4, nums_4))
                {
                    DialogueMgr.Instance.dialoguePanel.SetActive(false);
                    DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Bear, new string[] { "噢...食材不够我可是没法儿做的。" });
                }
                // 够就删除材料 添加物品 显示对话
                else
                {
                    for (int i = 0; i < items_4.Length; i++)
                    {
                        BagMgr.Instance.DeleteItem(items_4[i], nums_4[i]);
                    }
                    BagMgr.Instance.AddToBag(shopItem4);
                    
                    RandomChat();
                }
                break;
        }
    }


    private void RandomChat()
    {
        DialogueMgr.Instance.dialoguePanel.SetActive(false);
        DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Bear, new string[] { randomLines[Random.Range(0, randomLines.Length)] });
    }
}
