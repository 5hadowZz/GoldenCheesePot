using DG.Tweening;
using UnityEngine;


public class WaitState : BaseState
{
    private StateMachine stateMachine;
    private BossParamters paramters;
    private float timer;
    float probability;

    public WaitState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.paramters = stateMachine.paramters;
    }


    public override void OnEnter()
    {
        probability = Random.Range(0f, 1f);
    }


    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > paramters.waitTime)
        {
            if (stateMachine.IsNear())
            {
                if (probability >= 0.7f)
                    stateMachine.ChangeState(E_BossState.Move);
                else
                    stateMachine.ChangeState(E_BossState.Circle);
            }
            else
            {
                stateMachine.ChangeState(E_BossState.Dash);
            }
        }
    }


    public override void OnExit()
    {
        timer = 0f;
    }
}


public class MoveState : BaseState
{
    private StateMachine stateMachine;
    private BossParamters paramters;
    private Vector3 curPos;
    private Vector2 dir;

    public MoveState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.paramters = stateMachine.paramters;
    }


    public override void OnEnter()
    {
        paramters.animator.SetBool("Run", true);
        dir = (stateMachine.transform.position - paramters.target.position).normalized;
        curPos = stateMachine.transform.position;

        paramters.sr.flipX = dir.x < 0 ? true : false;

        stateMachine.transform.DOMove(curPos + (Vector3)dir * paramters.moveDistance, paramters.moveDistance / paramters.moveSpeed).OnComplete(() =>
        {
            stateMachine.ChangeState(E_BossState.Wait);
        });
    }


    public override void OnUpdate()
    {
        //stateMachine.transform.position = Vector2.MoveTowards(stateMachine.transform.position,
        //                                                      curPos + (Vector3)dir * paramters.moveDistance,
        //                                                      paramters.moveSpeed * Time.deltaTime);
    }


    public override void OnExit()
    {
        paramters.animator.SetBool("Run", false);
    }
}


public class DashState : BaseState
{
    private StateMachine stateMachine;
    private BossParamters paramters;
    private Vector3 curPos;
    private Vector2 dir;
    private AnimatorStateInfo info;

    public DashState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.paramters = stateMachine.paramters;
    }


    public override void OnEnter()
    {
        curPos = stateMachine.transform.position;
        dir = (paramters.target.position - curPos).normalized;
        paramters.sr.flipX = dir.x < 0 ? true : false;

        paramters.animator.SetBool("Dash", true);
    }


    public override void OnUpdate()
    {
        info = paramters.animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("DashPre") && info.normalizedTime >= 0.95f)
        {
            stateMachine.transform.DOMove(paramters.target.position + (Vector3)dir * paramters.DashDistance, paramters.DashDistance / paramters.DashSpeed).OnComplete(() =>
            {
                stateMachine.ChangeState(E_BossState.Wait);
            });
        }
    }


    public override void OnExit()
    {
        paramters.animator.SetBool("Dash", false);
    }
}


public class CircleState : BaseState
{
    private StateMachine stateMachine;
    private BossParamters paramters;
    private AnimatorStateInfo info;

    public CircleState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.paramters = stateMachine.paramters;
    }


    public override void OnEnter()
    {
        paramters.animator.SetTrigger("Circle");
    }


    public override void OnUpdate()
    {
        info = paramters.animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Circle") && info.normalizedTime >= 0.95f)
        {
            stateMachine.ChangeState(E_BossState.Move);
        }
    }


    public override void OnExit()
    {

    }
}
