using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Scene1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            Destroy(gameObject);
        }
    }
}
