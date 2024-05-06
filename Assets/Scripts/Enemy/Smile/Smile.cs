using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smile : Enemy
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public AnimationCurve curve;

    [Header("基本属性")]
    public int maxHP;
    [SerializeField]private int curHP;
    public int atk;
    public float hitForce;

    [Header("巡逻相关")]
    public float patrolRadius;      // 巡逻区域的半径
    private Vector2 patrolCenter;    // 巡逻区域中心

    public float jumpDistance;      // 每次跳跃的距离
    public float jumpSpeed;         // 每次跳跃速度

    public float patrolWaitTime;    // 每次跳跃等待时间
    private float patrolWaitTimer;  // 每次跳跃等待时间的计时器
    private Vector2 patrolPoint;    // 巡逻每次找到的点位

    [Header("追击相关")]
    public float findRadius;        // 以自身为中心  进入攻击状态的半径
    public float chaseJumpDistance;     // 追击时的跳跃距离
    public float chaseJumpSpeed;         // 追击时的跳跃速度
    public float chaseWaitTime;         // 追击时每次跳跃等待时间
    private float chaseWaitTimer;       // 计时器
    public float maxDistance;       // 追击时离巡逻中心点的最大距离

    private bool isChase;
    private bool isBack;
    private Transform target;
    private bool canMove = true;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    private void OnDrawGizmosSelected()
    {
        // 巡逻范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
        // 是否进入追击检测范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findRadius);
        // 最大距离
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }


    private void Start()
    {
        if (!GameDataMgr.Instance.SceneData.isBeKilledByBoss1)
        {
            Destroy(gameObject);
        }


        curHP = maxHP;
        patrolWaitTimer = patrolWaitTime;
        chaseWaitTimer = chaseWaitTime;
        patrolCenter = transform.position;
        FindRandomPatrolPoint();
    }


    private void Update()
    {
        if (!canMove) return;

        if (!isChase && !isBack)
        {
            Patrol();
        }
        else if (isChase && !isBack)
        {
            Chase();
        }
        else if (isBack && !isChase)
        {
            Back();
        }
    }


    /// <summary>
    /// 找随机巡逻点位
    /// </summary>
    /// <returns></returns>
    public Vector2 FindRandomPatrolPoint()
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(1f, patrolRadius);
        float randomX = patrolCenter.x + randomRadius * Mathf.Cos(randomAngle);
        float randomY = patrolCenter.y + randomRadius * Mathf.Sin(randomAngle);

        patrolPoint = new Vector2(randomX, randomY);

        return patrolPoint;
    }


    /// <summary>
    /// 每次巡逻跳跃完检测是否进入追击状态
    /// </summary>
    /// <returns></returns>
    public bool isEnterChaseState()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, findRadius, 1 << LayerMask.NameToLayer("Player"));
        this.target = target?.transform;

        return target == null ? false : true;
    }


    /// <summary>
    /// 巡逻逻辑
    /// </summary>
    public void Patrol()
    {
        // 巡逻时发现玩家  就进入追击状态
        if (isEnterChaseState())
        {
            isChase = true;
            return;
        }

        if (((Vector2)transform.position - patrolPoint).magnitude <= 0.1f)
        {
            FindRandomPatrolPoint();
        }
        else
        {
            if (patrolWaitTimer <= 0)
            {
                animator.SetTrigger("Jump");

                Vector2 dir = (patrolPoint - (Vector2)transform.position).normalized;
                spriteRenderer.flipX = dir.x > 0f ? true : false;

                patrolWaitTimer = patrolWaitTime;
                transform.DOMove(transform.position + (Vector3)dir * jumpDistance, jumpDistance / jumpSpeed).SetEase(curve);
            }
            else
            {
                patrolWaitTimer -= Time.deltaTime;
            }
        }
    }


    /// <summary>
    /// 追击逻辑
    /// </summary>
    public void Chase()
    {
        if (chaseWaitTimer <= 0)
        {
            animator.SetTrigger("Jump");

            Vector2 dir = (target.position - transform.position).normalized;
            spriteRenderer.flipX = dir.x > 0f ? true : false;

            chaseWaitTimer = chaseWaitTime;
            transform.DOMove(transform.position + (Vector3)dir * chaseJumpDistance, chaseJumpDistance / chaseJumpSpeed).SetEase(curve).OnComplete(() =>
            {
                // 追击时离巡逻点太远  就进入Back状态
                if (((Vector2)transform.position - patrolCenter).magnitude >= maxDistance)
                {
                    isChase = false;
                    isBack = true;
                }
            });
        }
        else
        {
            chaseWaitTimer -= Time.deltaTime;
        }
    }


    /// <summary>
    /// 脱战回城
    /// </summary>
    public void Back()
    {
        if (chaseWaitTimer <= 0)
        {
            animator.SetTrigger("Jump");

            Vector2 dir = (patrolCenter - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = dir.x > 0f ? true : false;

            chaseWaitTimer = chaseWaitTime;
            transform.DOMove(transform.position + (Vector3)dir * chaseJumpDistance, chaseJumpDistance / chaseJumpSpeed).SetEase(curve).OnComplete(() =>
            {
                // 到中心点了  就巡逻
                if (((Vector2)transform.position - patrolCenter).magnitude <= 1f)
                {
                    isChase = false;
                    isBack = false;
                }
            });
        }
        else
        {
            chaseWaitTimer -= Time.deltaTime;
        }
    }


    /// <summary>
    /// 受伤
    /// </summary>
    public override void GetHurt(Transform attacker)
    {
        // 反冲力
        transform.DOKill();
        Vector2 dir = (transform.position - attacker.position).normalized;
        transform.DOMove((Vector2)transform.position + dir * hitForce, 0.1f);

        // 受伤动画和减血
        animator.SetTrigger("Hurt");
        curHP -= attacker.GetComponent<Player>().atk;

        if (curHP <= 0) 
        {
            canMove = false;
            animator.SetTrigger("Dead");
        }
    }


    /// <summary>
    /// 在死亡动画的最后一帧添加Event  消除自身
    /// </summary>
    public void Dead()
    {
        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        transform.DOKill();
    }
}
