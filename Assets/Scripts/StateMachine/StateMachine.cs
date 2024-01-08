using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

        ChangeState(E_BossState.Dash);
    }


    private void Update()
    {
        curState.OnUpdate();
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


    public void GetHurt(Transform attacker)
    {
        paramters.animator.SetTrigger("Hurt");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, paramters.checkRadius);
    }
}
