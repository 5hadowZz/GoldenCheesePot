using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant"))
        {
            Plant plant = collision.GetComponent<Plant>();
            if (plant != null && PlantMgr.Instance.plantList.Contains(plant))
            {
                PlantMgr.Instance.RemovePlant(plant);
            }
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>()?.GetHit(transform);
        }

        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<StateMachine>()?.GetHurt(GetComponent<Player>().atk);
        }
    }
}