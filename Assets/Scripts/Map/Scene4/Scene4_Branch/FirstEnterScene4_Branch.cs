using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class FirstEnterScene4_Branch : MonoBehaviour
{
    private bool canPick;
    private GameObject plant;


    private void Awake()
    {
        if (GameDataMgr.Instance.SceneData.isFirstEnterScene4_Branch)
        {      
            plant = Instantiate(Resources.Load<GameObject>("PlantsPrefab/Special/Plant_Special_奶酪草"), transform, false);
        }
    }


    private void Update()
    {
        if (plant == null)
            return;

        if (canPick && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(plant);
            BagMgr.Instance.AddToBag(Resources.Load<GameObject>("BagItems/Plant_Special_Bag_奶酪草").GetComponent<Item>(), false);
            GameDataMgr.Instance.SceneData.isFirstEnterScene4_Branch = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPick = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPick = false;
        }
    }
}
