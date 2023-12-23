using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 dir;
    private float speedX, speedY;
    private float idleX, idleY;
    private float timer;    // 攻击间隔计时器

    [Header("移动相关")]
    public bool canMove = true;  // 是否可以移动  攻击、对话时等等使用
    public int speed;
    [Header("攻击相关")]
    public float attackOffset;   // 攻击间隔  外部调整
    public float attackMove;     // 攻击时的位移


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        ChangeAnimation();
        Attack();
    }


    private void FixedUpdate()
    {
        Move();
    }


    public void Move()
    {
        if (!canMove)
            return;

        // 速度方向 归一化
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");
        dir = new Vector2(speedX, speedY).normalized;
        // 速度
        rb.velocity = dir * speed * 0.0625f;

        if (dir != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            idleX = speedX;
            idleY = speedY;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }


    public void Attack()
    {
        if (Input.GetButtonDown("Attack") && timer >= attackOffset && canMove)
        {
            rb.velocity = Vector2.zero;
            canMove = false;
            animator.SetTrigger("Attack");
            timer = 0f;
            // 攻击位移
            if (animator.GetBool("isMoving"))
            {
                // 攻击时在移动 按住了X和Y  就斜方向移动
                rb.velocity = new Vector2(idleX * attackMove, idleY * attackMove);
            }
            else 
            {
                // 攻击时没在移动 就朝面朝向移动
                rb.velocity = animator.GetFloat("SpeedX") == 0f ?
                    new Vector2(rb.velocity.x, idleY * attackMove) : new Vector2(idleX * attackMove, rb.velocity.y);
            }
        }

        if (timer != attackOffset)
        {
            timer += Time.deltaTime;
            if (timer >= attackOffset)
            {
                timer = attackOffset;
            }
        }
    }


    /// <summary>
    /// 攻击结束事件  在帧动画中添加
    /// </summary>
    public void AttackOver()
    {
        canMove = true;
    }


    /// <summary>
    /// 更改动画状态机参数
    /// </summary>
    public void ChangeAnimation()
    {
        animator.SetFloat("SpeedX", idleX);
        animator.SetFloat("SpeedY", idleY);
    }
}
