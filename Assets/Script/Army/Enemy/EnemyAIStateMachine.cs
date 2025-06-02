using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


// 状态机控制器
public class EnemyAIStateMachine : MonoBehaviour
{

    [HideInInspector] public BaseState currentState;
    [HideInInspector] public EnemyAIConfig config;
    [HideInInspector] public EnemyBehaviours behaviors;

    public EnemyAIStateMachine(EnemyAIConfig config, EnemyBehaviours behaviors)
    {
        this.config = config;
        this.behaviors = behaviors;
    }

    public void InitEnemyAIStateMachine(EnemyAIConfig config, EnemyBehaviours behaviors)
    {
        this.config = config;
        this.behaviors = behaviors;
    }

    //mode = true时采用强制切换状态
    public void ChangeState(BaseState newState, bool Mode = false)
    {
        if(!Mode)
        {
            if (currentState == null || (currentState.isCanExit() == true))
            {
                currentState?.Exit();
                currentState = newState;
                currentState?.Enter();
            }
        }
        else
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.IUpdate();
        }
    }
}

public class BaseState : IEnemyState
{
    public NavMeshAgent agent;
    protected EnemyContext context;
    protected EnemyAIStateMachine aiStateMachine;

    public BaseState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine)
    {
        this.agent = agent;
        this.context = context;
        this.aiStateMachine = aiStateMachine;
        //Debug.Log(gameObject.name);
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        aiStateMachine.currentState = null;
    }

    public virtual bool isCanExit()
    {
        return true;
    }

    public virtual void IUpdate()
    {
        
    }
}

//移动到城市
public class MoveToCityState : BaseState
{
    private Vector3 city;

    public MoveToCityState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine): base(agent, context, ref aiStateMachine)
    {
        
    }
    public override void Enter()
    {
        city = context.GetNearestCity();
        aiStateMachine.behaviors.MoveToCity();
    }

    public override void Exit()
    {
        agent.isStopped=true;
        base.Exit();
    }
    //距离过近即可退出状态
    public override bool isCanExit()
    {
        return Vector3.Distance(aiStateMachine.transform.position, city) < aiStateMachine.config.minDis;
    }

    public override void IUpdate()
    {
        if(Vector3.Distance(aiStateMachine.transform.position, city) < aiStateMachine.config.minDis)
        {
            agent.isStopped = true;
        }
    }
   
}


//撤退状态
public class RetreatState : BaseState
{
    public Vector3 targetPos;

    public RetreatState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine) : base(agent, context, ref aiStateMachine)
    {

    }
    public override void Enter()
    {
        aiStateMachine.behaviors.Retreat();
        List<Vector3> path = GeometryUtils.CalculateReTreatPath(aiStateMachine.transform, context.playerArmy.ToArray());
        targetPos = path[path.Count - 1];
    }

    public override void Exit()
    {
        agent.isStopped = true;
        base.Exit();
    }

    public override bool isCanExit()
    {
        return Vector3.Distance(aiStateMachine.transform.position, targetPos) < aiStateMachine.config.minDis;
    }

    public override void IUpdate()
    {
        if (isCanExit())
        {
            Exit();
        }
    }
}

//前进状态(未完成)
public class MoveForwardState : BaseState
{
    public Vector3 playerCity;

    public MoveForwardState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine) : base(agent, context, ref aiStateMachine)
    {

    }
    public override void Enter()
    {
        //playerCity = Tool.FindNearestCityFromPlayerCitys(transform, EnemyAIManager.Instance.playerCity);
        aiStateMachine.behaviors.MoveForward();
    }

    public override void Exit()
    {
        agent.isStopped = true;
        base.Exit();
    }

    public override bool isCanExit()
    {
        return true;
    }

    public override void IUpdate()
    {
        //if (isCanExit())
        //{
        //    Exit();
        //}
    }
}

//原地待机状态
public class StationState : BaseState
{
    public StationState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine) : base(agent, context, ref aiStateMachine)
    {

    }
    public override void Enter()
    {
        aiStateMachine.behaviors.Station();
    }

    public override void Exit()
    {
        agent.isStopped = true;
        base.Exit();
    }

    public override bool isCanExit()
    {
        return true;
    }

    public override void IUpdate()
    {
        
    }
}

//据守城池状态
public class StayInCityState : BaseState
{
    public StayInCityState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine) : base(agent, context, ref aiStateMachine)
    {

    }
    public override void Enter()
    {
        aiStateMachine.behaviors.StayInCity();
    }

    public override void Exit()
    {
        agent.isStopped = true;
        base.Exit();
    }

    public override bool isCanExit()
    {
        return true;
    }

    public override void IUpdate()
    {

    }
}

//攻击状态
public class AttackState : BaseState
{
    Army target;
    public AttackState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine) : base(agent, context, ref aiStateMachine)
    {

    }
    public override void Enter()
    {
        target = context.GetNearestPlayerArmy();
        aiStateMachine.behaviors.Attack();
    }

    public override void Exit()
    {
        agent.isStopped = true;
        aiStateMachine.behaviors.StopAttack();
        base.Exit();
    }

    public override bool isCanExit()
    {
        return true;
    }

    public override void IUpdate()
    {
        
    }
}

//治疗状态
public class HealingState : BaseState
{
    public HealingState(NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine) : base(agent, context, ref aiStateMachine)
    {

    }
    public override void Enter()
    {
        aiStateMachine.behaviors.Healing();
    }
    public override void Exit()
    {
        aiStateMachine.behaviors.StopHealing();
        base.Exit();
    }

    public override bool isCanExit()
    {
        return true;
    }

    public override void IUpdate()
    {

    }
}
