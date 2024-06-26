using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    [Header("移动相关")]
    public bool canMove = true;  // 是否可以移动  攻击、对话时等等使用
    public int maxSpeed;
    public int curSpeed;
    [Header("攻击相关")]
    public bool canAttack;
    private bool isAttack;  // 是否处于攻击状态
    public int atk;
    public float attackOffset;   // 攻击间隔  外部调整
    public float attackMove;     // 攻击时的位移
    [Header("属性相关")]
    public int maxHP = 12;
    public int curHP;
    public int maxPower = 5;
    public int curPower;

    [Header("花洒")]
    public bool isPotting;
    public GameObject pot;
    private SpriteRenderer potSprite;
    private Animator potAnimator;
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
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        potSprite = pot.GetComponent<SpriteRenderer>();
        potAnimator = pot.GetComponent<Animator>();
    }


    private void Update()
    {
        ChangeAnimation();
        Attack();
        ChangePottingDir();
        CheckWater();
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
            return;     // 由于Move函数一直在Update调用  这里如果让攻击时velocity==0  会导致不能攻击时移动
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
        if (!canAttack || isPotting)
            return;

        if ((Input.GetButtonDown("Attack") || Input.GetMouseButtonDown(0)) && timer >= attackOffset && canMove && !isAttack && !isHurt)      // canMove是由其他如对话系统控制  !canMove时就不能攻击  isAttack由攻击本身控制  必须一次攻击结束 让isAttack为true时 才能攻击
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
        UIMgr.Instance.UpdateHP(curHP + atk, curHP, false);

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


    public void UpdateHP(int changedHP, bool isAddHP)
    {
        if (curHP + changedHP >= maxHP)
            curHP = maxHP;
        else
            curHP += changedHP;

        UIMgr.Instance.UpdateHP(curHP - changedHP, curHP, isAddHP);
    }


    public void Dead()
    {
        // 在场景2  并且没有被boss1击杀过  那么死亡时  就触发被boss1击杀的逻辑  并且改布尔 被boss1击杀 为true
        if (SceneLoadMgr.Instance.curScene.name == "Scene2_Main" && !GameDataMgr.Instance.SceneData.isBeKilledByBoss1)
        {
            SceneLoadMgr.Instance.OnPlayerDeadFromBoss1();
        }
        // 在场景4 并且进入区域触发了遇见boss2  才算作这个场景的死亡来源是boss2   死亡时才会触发被boss2杀死的逻辑
        else if (SceneLoadMgr.Instance.curScene.name == "Scene4_Main" && FindObjectOfType<Test_FirstEnterBoss2>().curSceneMeet)
        {
            SceneLoadMgr.Instance.OnPlayerDeadFromBoss2();
        }
        else
        {
            canMove = false;

            UIMgr.Instance.fadePanel.DOFade(1, 2f).OnComplete(() =>
            {
                AsyncOperation ao = SceneManager.LoadSceneAsync("Scene3_Main");

                ao.completed += (param) =>
                {
                    SceneLoadMgr.Instance.curScene = SceneManager.GetActiveScene();
                    transform.position = new Vector3(-8f, -49f, 0f);
                    UIMgr.Instance.fadePanel.DOFade(0, 1f).OnComplete(() =>
                    {
                        MusicMgr.Instance.ChangeSceneMusic();
                        canMove = true;
                        curHP = maxHP;
                        UIMgr.Instance.UpdateHP(0, curHP, true);
                    });
                };
            });
        }
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


    /// <summary>
    /// 花洒状态下的花洒Obj的方向切换
    /// </summary>
    private void ChangePottingDir()
    {
        if (!isPotting)
        {
            pot.SetActive(false);
        }


        if (isPotting)
        {
            if (idleX > 0)
                potSprite.flipX = false;
            else if (idleX < 0)
                potSprite.flipX = true;

            if (idleY > 0 && idleX == 0)
                pot.SetActive(false);
            else
                pot.SetActive(true);
        }
    }


    /// <summary>
    /// 花洒状态下浇水的处理
    /// </summary>
    public void CheckWater()
    {
        if (waterTimer > 0)
            waterTimer -= Time.deltaTime;

        if (!isPotting)
            return;

        if (Input.GetMouseButtonDown(0) && waterTimer <= 0)
        {
            canMove = false;
            potAnimator.SetTrigger("Shaking");

            // 右浇
            if (animator.GetFloat("SpeedX") > 0)
            {
                waterRight.SetActive(true);
            }
            // 左
            else if (animator.GetFloat("SpeedX") < 0)
            {
                waterLeft.SetActive(true);
            }
            else
            {
                // 前
                if (animator.GetFloat("SpeedY") > 0)
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


    /// <summary>
    /// 反弹自身
    /// </summary>
    public void BounceSelf()
    {
        rb.velocity = animator.GetFloat("SpeedX") == 0f ?
            new Vector2(rb.velocity.x, -idleY * attackMove * 1.5f) * 0.0625f : new Vector2(-idleX * attackMove * 1.5f, rb.velocity.y) * 0.0625f;
    }
}
