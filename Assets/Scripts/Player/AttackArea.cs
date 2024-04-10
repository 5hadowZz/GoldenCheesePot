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
            collision.GetComponent<Enemy>()?.GetHit(transform);
        }

        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<StateMachine>()?.GetHurt(transform.parent.GetComponent<Player>().atk);
        }

        if (collision.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
        }
    }
}