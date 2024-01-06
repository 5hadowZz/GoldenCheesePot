using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_BossState
{
    Wait,
    Move,
    Dash,
    Circle
}


public class StateMachine : MonoBehaviour
{
    public Dictionary<E_BossState, BaseState> stateDic = new();
    public BossParamters paramters = new();
    public BaseState curState;


    private void Awake()
    {
        paramters.animator = GetComponent<Animator>();
        paramters.sr = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        stateDic.Add(E_BossState.Wait, new WaitState(this));
        stateDic.Add(E_BossState.Move, new MoveState(this));
        stateDic.Add(E_BossState.Dash, new DashState(this));
        stateDic.Add(E_BossState.Circle, new CircleState(this));

        ChangeState(E_BossState.Wait);
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, paramters.checkRadius);
    }
}
