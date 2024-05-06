using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elf : Enemy
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool canMove = true;

    private bool isChase;
    private Transform target;

    [Header("基本属性")]
    public int maxHP;
    [SerializeField]private int curHP;
    public int atk;
    public float hitForce;
    public GameObject arrowPrefab;

    [Header("巡逻相关")]
    private bool isGoToPoint;
    public float patrolRadius;      // 巡逻区域的半径
    private Vector2 patrolCenter;    // 巡逻区域中心
    public float patrolSpeed;         // 巡逻移动速度

    public float patrolWaitTime;    // 巡逻一次等待时间
    private float patrolWaitTimer;  // 计时器
    private Vector2 patrolPoint;    // 巡逻每次找到的点位

    [Header("追击相关")]
    public float shootRadius;        // 以自身为中心  进入攻击状态的半径
    public float awayDistance;       // 每射一箭的后撤距离
    public float awaySpeed;          // 后撤速度
    public float attackCD;           // 射一箭后撤后  等待的时间
    private float attackCDTimer;           // 射箭等待计时器
    private bool isAttacking;

    [Header("箭矢相关")]
    public Transform arrowLeft;
    public Transform arrowRight;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        if (!GameDataMgr.Instance.SceneData.isBeKilledByBoss1)
        {
            Destroy(gameObject);
        }

        curHP = maxHP;
        patrolWaitTimer = patrolWaitTime;
        patrolCenter = transform.position;
        FindRandomPatrolPoint();
    }


    private void Update()
    {
        if (!canMove) return;

        if (!isChase)
        {
            Patrol();
        }
        else if (isChase)
        {
            Chase();
        }
    }


    private void OnDrawGizmosSelected()
    {
        // 巡逻范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        // 攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRadius);
    }


    /// <summary>
    /// 巡逻逻辑
    /// </summary>
    public void Patrol()
    {
        // 巡逻时发现玩家  就进入追击状态
        if (isEnterChaseState())
        {
            transform.DOKill();
            isGoToPoint = false;
            isChase = true;
            return;
        }

        if (patrolWaitTimer <= 0f && !isGoToPoint)
        {
            isGoToPoint = true;

            Vector2 dir = (patrolPoint - (Vector2)transform.position).normalized;
            spriteRenderer.flipX = dir.x > 0f ? true : false;
            float distance = (patrolPoint - (Vector2)transform.position).magnitude;

            patrolWaitTimer = patrolWaitTime;
            transform.DOMove(patrolPoint, distance / patrolSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                isGoToPoint = false;
                FindRandomPatrolPoint();
            });
        }
        else
        {
            if (!isGoToPoint)
                patrolWaitTimer -= Time.deltaTime;
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
    /// 检测是否有玩家进入攻击范围
    /// </summary>
    /// <returns></returns>
    public bool isEnterChaseState()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, shootRadius, 1 << LayerMask.NameToLayer("Player"));
        this.target = target?.transform;

        return target == null ? false : true;
    }


    /// <summary>
    /// 追击
    /// </summary>
    public void Chase()
    {
        if (isAttacking) return;

        // 追击时玩家离自身太远  且自身不在攻击状态时  进入巡逻状态
        if ((target.position - transform.position).magnitude > patrolRadius && !isAttacking)
        {
            isChase = false;
            attackCDTimer = 0f;
            patrolWaitTimer = 0f;
            return;
        }

        if (attackCDTimer <= 0)
        {
            Vector3 dir = (transform.position - target.position).normalized;
            spriteRenderer.flipX = dir.x > 0f ? false : true;
            animator.SetTrigger("Shoot");
            isAttacking = true;
        }
        else
        {
            attackCDTimer -= Time.deltaTime;
        }
    }


    /// <summary>
    /// 射箭
    /// </summary>
    public void Shoot()
    {
        GameObject arrowObj;
        if (!spriteRenderer.flipX)
        {
            arrowObj = Instantiate(arrowPrefab, arrowLeft.position, Quaternion.identity);
        }
        else
        {
            arrowObj = Instantiate(arrowPrefab, arrowRight.position, Quaternion.identity);
        }

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrow.target = target;
        arrow.atk = atk;
    }


    /// <summary>
    /// 远离target
    /// </summary>
    public void Away()
    {
        Vector3 dir = (transform.position - target.position).normalized;
        spriteRenderer.flipX = dir.x > 0f ? true : false;

        transform.DOMove(transform.position + dir * awayDistance, awayDistance / awaySpeed).OnComplete(() =>
        {
            isAttacking = false;
            attackCDTimer = attackCD;
        });
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
