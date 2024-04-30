using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smile_AttackArea : MonoBehaviour
{
    private Smile smile;


    private void Awake()
    {
        smile = transform.parent.GetComponent<Smile>();    
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().GetHurt(smile.atk);
        }
    }
}
