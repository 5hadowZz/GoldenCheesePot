using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SmokeObj : MonoBehaviour
{
    public Smoke father;
    public float moveDistance;
    public float moveDuration;


    private void OnEnable()
    {
        transform.DOMoveX(transform.position.x + moveDistance, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            father.EnqueueSmoke(gameObject);
        });
    }


    private void OnDestroy()
    {
        transform.DOPause();
    }
}
