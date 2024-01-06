using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float hitForce;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void GetHit(Transform attacker)
    {
        Vector2 direction = (transform.position - attacker.position).normalized;
        //rb.velocity = direction * hitForce;
        rb.AddForce(direction * hitForce, ForceMode2D.Impulse);
    }
}
