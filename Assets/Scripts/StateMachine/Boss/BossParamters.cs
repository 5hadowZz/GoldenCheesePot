using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossParamters
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public float waitTime;
    public Transform target;
    public int hp = 100;
    public int atk = 1;

    public float circleWaitTime;
    public float dashWaitTime;
    public float runWaitTime;

    public float nearSpeed = 1.2f;
    public float nearDistance = 1f;
    public float dashSpeed = 8f;
    public float dashOffset = 4f;
    public float runSpeed = 5f;

    public float checkRadius; 
}
