using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance => instance;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 dir;
    private float speedX, speedY;
    private float idleX, idleY;
    private float timer;    // 攻击间隔计时器
    private bool isHurt = false;
    private int curSpeed;

    [Header("移动相关")]
    public bool canMove = true;  // 是否可以移动  攻击、对话时等等使用
    public int maxSpeed;
    [Header("攻击相关")]
    private bool isAttack;  // 是否处于攻击状态
    public int atk;
    public float attackOffset;   // 攻击间隔  外部调整
    public float attackMove;     // 攻击时的位移
    [Header("属性相关")]
    public int maxHP = 12;
    public int curHP;
    public int maxPower = 5;
    public int curPower;


    private void Awake()
    {
        instance = this;
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


    private void Start()
    {
        curHP = maxHP;
        curSpeed = maxSpeed;
    }


    public void Move()
    {
        if (isAttack)
        {
            return;     // 这里如果让攻击时velocity==0  会一直调用  导致不能攻击时移动  也就是这里会一直控制velocity==0
        }

        if (!canMove)
        {
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            return;
        }

        // 速度方向 归一化
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");
        dir = new Vector2(speedX, speedY).normalized;
        // 速度
        rb.velocity = dir * curSpeed * 0.0625f;

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
        if (Input.GetButtonDown("Attack") && timer >= attackOffset && canMove && !isAttack && !isHurt)      // canMove是由其他如对话系统控制  !canMove时就不能攻击  isAttack由攻击本身控制  必须一次攻击结束 让isAttack为true时 才能攻击
        {
            rb.velocity = Vector2.zero;
            //canMove = false;
            isAttack = true;
            animator.SetTrigger("Attack");
            timer = 0f;
            // 攻击位移
            if (animator.GetBool("isMoving"))
            {
                // 攻击时在移动 按住了X和Y  就斜方向移动              
                rb.velocity = dir * attackMove * 0.0625f;
            }
            else
            {
                // 攻击时没在移动 就朝面朝向移动
                rb.velocity = animator.GetFloat("SpeedX") == 0f ?
                    new Vector2(rb.velocity.x, idleY * attackMove) * 0.0625f : new Vector2(idleX * attackMove, rb.velocity.y) * 0.0625f;
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


    public void GetHurt(int atk)
    {
        if (curHP <= 0 || isHurt)
            return;

        curHP -= atk;
        UIMgr.Instance.UpdateHP(curHP, false);

        if (curHP <= 0)
        {
            Dead();
        }
        else
        {
            isHurt = true;
            animator.SetTrigger("Hurt");
            curSpeed = maxSpeed / 2;
        }
    }


    public void Dead()
    {
        canMove = false;
    }


    /// <summary>
    /// 攻击结束事件  在帧动画中添加
    /// </summary>
    public void AttackOver()
    {
        //canMove = true;
        isAttack = false;
    }


    /// <summary>
    /// 受伤结束事件  在帧动画中添加
    /// </summary>
    public void HurtOver()
    {
        isHurt = false;
        curSpeed = maxSpeed;
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
