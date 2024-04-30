using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [Header("Pot")]
    private Animator animator;
    private Animator playerAnimator;
    [Header("WaterDir")]
    public GameObject waterRight;
    public GameObject waterLeft;
    public GameObject waterUp;
    public GameObject waterDown;
    [Header("WaterTime")]
    public float waterOffset;
    private float waterTimer;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerAnimator = transform.parent.GetComponent<Animator>();
    }


    private void Update()
    {
        if (waterTimer > 0)
            waterTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && waterTimer <= 0)
        {
            Player.Instance.canMove = false;
            animator.SetTrigger("Shaking");

            // 右浇
            if (playerAnimator.GetFloat("SpeedX") > 0)
            {
                waterRight.SetActive(true);
            }
            // 左
            else if (playerAnimator.GetFloat("SpeedX") < 0)
            {
                waterLeft.SetActive(true);
            }           
            else
            {
                // 前
                if (playerAnimator.GetFloat("SpeedY") > 0)
                {
                    waterUp.SetActive(true);
                }
                // 后
                else
                {
                    waterDown.SetActive(true);
                }
            }

            waterTimer = waterOffset;
        }
    }
}
