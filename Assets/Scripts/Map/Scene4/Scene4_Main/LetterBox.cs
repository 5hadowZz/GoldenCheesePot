using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    public GameObject buttonE;
    [TextArea] public string[] onItemsNotEnough;
    [TextArea] public string[] onItemsEnough;

    private bool canInteract;
    private Item letter;
    private Item peaCake;


    private void Start()
    {
        letter = Resources.Load<GameObject>("BagItems/Quest_Bag_信件").GetComponent<Item>();
        peaCake = Resources.Load<GameObject>("BagItems/Shop_Bag_豌豆饼").GetComponent<Item>();
    }


    private void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (!CheckItemsContain())
            {
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Tip, onItemsNotEnough);
            }
            else
            {
                DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Tip, onItemsEnough, () =>
                {
                    buttonE.SetActive(false);
                    BagMgr.Instance.DeleteItem(letter, 1);
                    BagMgr.Instance.DeleteItem(peaCake, 1);             
                    // 招隐藏boss

                });
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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


    private bool CheckItemsContain()
    {
        return BagMgr.Instance.CheckContainItems(new Item[] { letter, peaCake }, 
                                                 new int[]  { 1, 1 });
    }
}
