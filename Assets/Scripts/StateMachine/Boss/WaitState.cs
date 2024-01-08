using DG.Tweening;
using UnityEngine;


public class WaitState : BaseState
{
    private StateMachine stateMachine;
    private BossParamters paramters;
    private Vector2 dir;
    private float timer;

    public WaitState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.paramters = stateMachine.paramters;
    }


    public override void OnEnter()
    {

    }


    public override void OnUpdate()
    {
        dir = stateMachine.transform.position - paramters.target.position;
        paramters.sr.flipX = dir.x < 0 ? false : true;

        timer += Time.deltaTime;
        if (timer > paramters.waitTime)
        {
            stateMachine.RandomChangeState();
        }
    }


    public override void OnExit()
    {
        timer = 0f;
    }
}


//public class EvadeState : BaseState
//{
//    private StateMachine stateMachine;
//    private BossParamters paramters;
//    private Vector3 curPos;
//    private Vector2 dir;

//    public EvadeState(StateMachine stateMachine)
//    {
//        this.stateMachine = stateMachine;
//        this.paramters = stateMachine.paramters;
//    }


//    public override void OnEnter()
//    {
//        paramters.animator.SetBool("Run", true);
//        dir = (stateMachine.transform.position - paramters.target.position).normalized;
//        curPos = stateMachine.transform.position;
//        paramters.sr.flipX = dir.x < 0 ? true : false;

//        stateMachine.transform.DOMove(curPos + (Vector3)dir * paramters.evadeDistance, paramters.evadeDistance / paramters.evadeSpeed).OnComplete(() =>
//        {
//            stateMachine.ChangeState(E_BossState.Wait);
//        });
//    }


//    public override void OnUpdate()
//    {
//        //stateMachine.transform.position = Vector2.MoveTowards(stateMachine.transform.position,
//        //                                                      curPos + (Vector3)dir * paramters.moveDistance,
//        //                                                      paramters.moveSpeed * Time.deltaTime);
//    }


//    public override void OnExit()
//    {
//        paramters.animator.SetBool("Run", false);
//        paramters.waitTime = paramters.evadeWaitTime;
//    }
//}


//public class NearState : BaseState
//{
//    private StateMachine stateMachine;
//    private BossParamters paramters;
//    private Vector3 curPos;
//    private Vector2 dir;

//    public NearState(StateMachine stateMachine)
//    {
//        this.stateMachine = stateMachine;
//        this.paramters = stateMachine.paramters;
//    }


//    public override void OnEnter()
//    {
//        paramters.animator.SetBool("Run", true);
//        dir = (stateMachine.transform.position - paramters.target.position).normalized;
//        curPos = stateMachine.transform.position;
//        paramters.sr.flipX = dir.x < 0 ? false : true;

//        stateMachine.transform.DOMove(paramters.target.position + (Vector3)dir * paramters.nearDistance, paramters.nearDistance / paramters.nearSpeed).OnComplete(() =>
//        {
//            stateMachine.ChangeState(E_BossState.Circle);
//        });
//    }


//    public override void OnUpdate()
//    {

//    }


//    public override void OnExit()
//    {
//        paramters.animator.SetBool("Run", false);
//    }
//}


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

        if (info.IsName("DashPre") && info.normalizedTime >= 0.97f)
        {
            stateMachine.transform.DOMove(paramters.target.position + (Vector3)dir * paramters.dashOffset, paramters.dashOffset / paramters.dashSpeed).OnComplete(() =>
            {
                stateMachine.ChangeState(E_BossState.Wait);
            });
        }
    }


    public override void OnExit()
    {
        paramters.animator.SetBool("Dash", false);
        paramters.waitTime = paramters.dashWaitTime;
        stateMachine.preState = E_BossState.Dash;
    }
}


public class CircleState : BaseState
{
    private StateMachine stateMachine;
    private BossParamters paramters;
    private AnimatorStateInfo info;
    private Vector2 dir;
    private bool canToTarget;

    public CircleState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.paramters = stateMachine.paramters;
    }


    public override void OnEnter()
    {
        if (stateMachine.IsNear())
        {
            paramters.sr.flipX = false;
            paramters.animator.SetTrigger("Circle");
        }
        else
        {                     
            paramters.animator.SetBool("Run", true);
            canToTarget = true;
        }
    }


    public override void OnUpdate()
    {
        if (canToTarget)
        {
            canToTarget = false;
            dir = (stateMachine.transform.position - paramters.target.position).normalized;
            paramters.sr.flipX = dir.x < 0 ? false : true;

            stateMachine.transform.DOMove(paramters.target.position + (Vector3)dir * paramters.nearDistance, paramters.nearDistance / paramters.nearSpeed).SetEase(Ease.Linear).OnComplete(() =>
            {
                paramters.animator.SetBool("Run", false);
                paramters.sr.flipX = false;
                paramters.animator.SetTrigger("Circle");                
            });
        }

        info = paramters.animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Circle") && info.normalizedTime >= 0.98f)
        {
            stateMachine.ChangeState(E_BossState.Wait);
        }
    }


    public override void OnExit()
    {
        paramters.waitTime = paramters.circleWaitTime;
        stateMachine.preState = E_BossState.Circle;
    }
}


public class RunState : BaseState
{
    private StateMachine stateMachine;
    private BossParamters paramters;
    private Vector2 dir;

    public RunState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.paramters = stateMachine.paramters;
    }


    public override void OnEnter()
    {       
        float randomAngle = Random.Range(0f, 360f);
        float randomX = paramters.target.position.x + 2f * Mathf.Cos(randomAngle);
        float randomY = paramters.target.position.y + 2f * Mathf.Sin(randomAngle);
        float distance = ((Vector2)stateMachine.transform.position - new Vector2(randomX, randomY)).magnitude;

        dir = (Vector2)stateMachine.transform.position - new Vector2(randomX, randomY);
        paramters.sr.flipX = dir.x < 0 ? false : true;

        paramters.animator.SetBool("Run", true);
        stateMachine.transform.DOMove(new Vector3(randomX, randomY, 0), distance / paramters.runSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            stateMachine.ChangeState(E_BossState.Wait);
        });
    }


    public override void OnUpdate()
    {

    }


    public override void OnExit()
    {
        paramters.animator.SetBool("Run", false);
        paramters.waitTime = paramters.runWaitTime;
        stateMachine.preState = E_BossState.Run;
    }
}
