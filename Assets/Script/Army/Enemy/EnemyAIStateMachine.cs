using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


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

    //newState=99时采用强制切换
    public void ChangeState(BaseState newState)
    {
        if ((bool)(currentState?.isCanExit(newState.stateLevel)))
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
        else
        {
            currentState = newState;
            currentState.Enter();
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
    protected EnemyAI enemyAI;
    public NavMeshAgent agent;
    protected EnemyContext context;
    protected EnemyAIStateMachine aiStateMachine;
    public int stateLevel = 0;//表示指令动作的优先级

    //可以在实例化时进行动态优先级调整
    public BaseState(EnemyAI enemyAI,int stateLevel = 0)
    {
        this.enemyAI = enemyAI;
        this.agent = enemyAI.agent;
        this.context = enemyAI.context;
        this.aiStateMachine = enemyAI.aism;
        this.stateLevel = stateLevel;
        //Debug.Log(gameObject.name);
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        aiStateMachine.currentState = null;
    }

    public virtual bool isCanExit(int nextStateLevel = 100)
    {
        return nextStateLevel > stateLevel;
    }

    public virtual void IUpdate()
    {
        
    }
}

//移动到城市2
public class MoveToCityState : BaseState
{
    private Vector3 city;

    public MoveToCityState(EnemyAI enemyAI,NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine,int stateLevel = 2)
                            : base(enemyAI,stateLevel)
    {
        
    }
    public override void Enter()
    {
        aiStateMachine.behaviors.MoveToCity();
    }

    public override void Exit()
    {
        enemyAI.StopMove();
        base.Exit();
    }
    //距离过近即可退出状态
    public override bool isCanExit(int nextStateLevel = 100)
    {
        if (nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel)) return true;
        return Vector3.Distance(aiStateMachine.transform.position, city) < aiStateMachine.config.minDis;
    }

    public override void IUpdate()
    {
        if(isCanExit())
        {
            Exit();
        }
    }
   
}

//撤退状态5
public class RetreatState : BaseState
{
    public Vector3 targetPos;

    public RetreatState(EnemyAI enemyAI, NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine, int stateLevel = 5) 
                        : base(enemyAI, stateLevel)
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
        enemyAI.StopMove();
        base.Exit();
    }

    public override bool isCanExit(int nextStateLevel = 100)
    {
        if (nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel)) return true;
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

//前进状态3
public class MoveForwardState : BaseState
{
    City playerCity;

    public MoveForwardState(EnemyAI enemyAI, NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine, int stateLevel = 3)
                            : base(enemyAI, stateLevel)
    {

    }
    public override void Enter()
    {
        playerCity = aiStateMachine.behaviors.MoveForward();
    }

    public override void Exit()
    {
        enemyAI.StopMove();
        base.Exit();
    }

    public override bool isCanExit(int nextStateLevel = 100)
    {
        if (nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel)) return true;
        return Vector3.Distance(aiStateMachine.transform.position, playerCity.transform.position) < aiStateMachine.config.minDis;
    }

    public override void IUpdate()
    {
        if (!isCanExit())
        {
            Exit();
        }
    }
}

//原地待机状态2
public class StationState : BaseState
{
    public StationState(EnemyAI enemyAI, NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine, int stateLevel = 2)
                        : base(enemyAI, stateLevel)
    {

    }
    public override void Enter()
    {
        aiStateMachine.behaviors.Station();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override bool isCanExit(int nextStateLevel = 100)
    {
        if (nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel)) return true;
        return false;
    }

    public override void IUpdate()
    {
        
    }
}

//攻击状态4
public class AttackState : BaseState
{
    public Army target;
    public AttackState(EnemyAI enemyAI, NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine, int stateLevel = 4)
                        : base(enemyAI, stateLevel)
    {

    }
    public override void Enter()
    {
        target = context.GetNearestPlayerArmy();
        aiStateMachine.behaviors.Attack();
    }

    public override void Exit()
    {
        aiStateMachine.behaviors.StopAttack();
        base.Exit();
    }

    public override bool isCanExit(int nextStateLevel = 100)
    {
        if (nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel)) return true;
        return target == null;
    }

    public override void IUpdate()
    {
        
    }
}

//治疗状态3
public class HealingState : BaseState
{
    public HealingState(EnemyAI enemyAI, NavMeshAgent agent, EnemyContext context, ref EnemyAIStateMachine aiStateMachine, int stateLevel = 3)
                        : base(enemyAI, stateLevel)
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

    public override bool isCanExit(int nextStateLevel = 100)
    {
        if (nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel)) return true;
        return false;
    }

    public override void IUpdate()
    {

    }
}

//围剿状态6
public class SurroundState : BaseState
{
    Army target;

    public SurroundState(EnemyAI enemyAI, Army target, int stateLevel = 6)
                        : base(enemyAI, stateLevel)
    {
        this.target = target;
    }
    public override void Enter()
    {
        
    }
    public override void Exit()
    {
        base.Exit();
        enemyAI.ExecuteBehaviour(EnemyArmyBehavior.attack);
    }

    public override bool isCanExit(int nextStateLevel = 100)
    {
        if(nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel))return true;
        return Vector3.Distance(aiStateMachine.transform.position, target.transform.position) < aiStateMachine.config.minDis;
    }

    public override void IUpdate()
    {
        if(isCanExit())
        {
            Exit();
        }
    }
}

//支援状态6
public class SupportState:BaseState
{
    Army supportTarget;
    public SupportState(EnemyAI enemyAI, Army supportTarget, int stateLevel = 6)
                        : base(enemyAI, stateLevel)
    {
        this.supportTarget = supportTarget;
    }
    public override void Enter()
    {

    }
    public override void Exit()
    {
        base.Exit();
    }

    public override bool isCanExit(int nextStateLevel = 100)
    {
        if (nextStateLevel != 100)
            if (base.isCanExit(nextStateLevel)) return true;
        return Vector3.Distance(aiStateMachine.transform.position, supportTarget.transform.position) < aiStateMachine.config.minDis;
    }

    public override void IUpdate()
    {
        if (isCanExit())
        {
            Exit();
        }
    }
}
