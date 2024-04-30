using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant") || collision.CompareTag("Ornament"))
        {
            Plant plant = collision.GetComponent<Plant>();
            if (plant != null && PlantMgr.Instance.plantList.Contains(plant))
            {
                plant.transform.parent.GetComponent<PlantZone>().RemovePlant(plant);
            }
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>()?.GetHurt(transform.parent);
        }

        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<StateMachine>()?.GetHurt(transform.parent.GetComponent<Player>().atk);
        }

        if (collision.CompareTag("Obstacle"))
        {
            if (collision.gameObject.name == "雕像")
            {
                if (Player.Instance.atk >= 3)
                {
                    Destroy(collision.gameObject);
                    GameDataMgr.Instance.SceneData.Scene2_Statue_isDestroy = true;
                }
                else
                {
                    Player.Instance.BounceSelf();
                    string[] lines = collision.GetComponent<Scene2_Statue>().onAtkNotEnough;
                    DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Tip, lines);
                }              
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}