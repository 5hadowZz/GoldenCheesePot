using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossParamters
{
    public Transform target;
    public SpriteRenderer sr;
    public int hp = 100;
    public float waitTime = 3f;
    public float moveSpeed = 5f;
    public float moveDistance = 3f;
    public float DashSpeed = 8f;
    public float DashDistance = 3f;

    public float checkRadius;

    public Animator animator;
}
