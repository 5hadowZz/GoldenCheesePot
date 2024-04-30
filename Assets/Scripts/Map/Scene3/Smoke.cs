using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public bool isSmoke;

    public GameObject smokePrefab;
    public Queue<GameObject> smokeQueue = new();

    public float spawnOffset;

    public float remainTime;


    private void Update()
    {
        if (isSmoke)
        {
            remainTime -= Time.deltaTime;

            if (remainTime < 0)
            {
                if (smokeQueue.Count < 5)
                {
                    GameObject smoke = Instantiate(smokePrefab, transform, false);
                    smoke.GetComponent<SmokeObj>().father = this;
                    smokeQueue.Enqueue(smoke);
                    remainTime = spawnOffset;
                    return;
                }

                DequeueSmoke();
                remainTime = spawnOffset;
            }
        }
        else
        {
            Destroy(GetComponent<BoxCollider2D>());
        }
    }


    private void Start()
    {
        remainTime = spawnOffset;
    }


    public void EnqueueSmoke(GameObject smoke)
    {
        smokeQueue.Enqueue(smoke);
        smoke.transform.position = transform.position;
        smoke.SetActive(false);
    }


    public void DequeueSmoke()
    {
        GameObject smoke = smokeQueue.Dequeue();
        smoke.SetActive(true);
    }
}
