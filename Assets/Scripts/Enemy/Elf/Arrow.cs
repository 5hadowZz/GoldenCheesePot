using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public int atk;

    private float timer;
    private Vector3 dir;
    public float speed;


    private void Start()
    {
        dir = ((target.position + new Vector3(0, 0.37f, 0)) - transform.position).normalized;

        float angle = Vector2.SignedAngle(Vector2.left, dir);
        transform.Rotate(Vector3.forward, angle);
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3)
            Destroy(gameObject);

        transform.position += dir * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().GetHurt(atk);
            Destroy(gameObject);
        }
    }
}
