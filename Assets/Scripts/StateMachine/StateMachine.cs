using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BossState
{
    Wait,
    Run,
    Dash,
    Circle
}


public class StateMachine : MonoBehaviour
{
    public Dictionary<E_BossState, BaseState> stateDic = new();
    public BossParamters paramters = new();
    public BaseState curState;

    public E_BossState preState;


    private void Awake()
    {
        paramters.animator = GetComponent<Animator>();
        paramters.sr = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        stateDic.Add(E_BossState.Wait, new WaitState(this));
        stateDic.Add(E_BossState.Run, new RunState(this));
        stateDic.Add(E_BossState.Dash, new DashState(this));
        stateDic.Add(E_BossState.Circle, new CircleState(this));

        if (paramters.target != null)
        {
            ChangeState(E_BossState.Dash);
        }
    }


    private void Update()
    {
        curState?.OnUpdate();
    }


    public void ChangeState(E_BossState state)
    {
        if (curState != null)
            curState.OnExit();

        curState = stateDic[state];
        curState.OnEnter();
    }


    public bool IsNear()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position + Vector3.up * 0.5f, paramters.checkRadius, 1 << LayerMask.NameToLayer("Player"));
        return player == null ? false : true;
    }


    public void RandomChangeState()
    {
        float probability = Random.Range(0.0f, 1.0f);

        if (preState == E_BossState.Circle)
        {
            if (probability >= 0.5f)
                ChangeState(E_BossState.Run);
            else
                ChangeState(E_BossState.Dash);
        }

        if (preState == E_BossState.Dash)
        {
            if (probability <= 0.2f)
                ChangeState(E_BossState.Dash);
            else if (probability > 0.2f && probability <= 0.6f)
                ChangeState(E_BossState.Circle);
            else
                ChangeState(E_BossState.Run);
        }

        if (preState == E_BossState.Run)
        {
            if (probability >= 0.5f)
                ChangeState(E_BossState.Circle);
            else
                ChangeState(E_BossState.Dash);
        }
    }


    public void GetHurt(int atk)
    {
        paramters.animator.SetTrigger("Hurt");
        paramters.hp -= atk;

        if (paramters.hp <= 0)
        {
            ChangeState(E_BossState.Wait);  // 触发死亡前状态的OnExit  使Animator中参数回归默认  让sprite成为默认站立
            paramters.target = null;    // 可以设置死亡动画
            DialogueMgr.Instance.ShowDialogue(E_DialogueNPC.Boss, new string[] { "做得……", "好……" }, () =>
            {
                MusicMgr.Instance.ChangeSceneMusic();
                Destroy(GetComponent<Animator>());   // 防止DoFade改透明度时一直切换Spite导致透明度刷新
                GetComponent<SpriteRenderer>().DOFade(0, 4f).OnComplete(() =>
                {
                    Destroy(gameObject);
                    // TODO:到时候改
                    Destroy(GameObject.Find("EnterBoss").transform.GetChild(0).gameObject);
                });
            });

            GameDataMgr.Instance.SceneData.isKilledBoss2 = true;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, paramters.checkRadius);
    }
}
