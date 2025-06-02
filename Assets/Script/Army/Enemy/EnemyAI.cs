using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using Vector3 = UnityEngine.Vector3;

public enum EnemyArmyBehavior
{ 
    moveToCity,
    retreat,
    moveForward,
    station,
    stayInCity,
    attack,
    healing
}

[RequireComponent(typeof(EnemyAIStateMachine))]
[RequireComponent(typeof(EnemyBehaviours))]
[RequireComponent(typeof(EnemyContext))]
public class EnemyAI : Army
{
    [HideInInspector]
    public NavMeshAgent agent;

    //血量上限
    [SerializeField] int peopleLimit;
    //环境感知脚本
    EnemyContext context;
    //决策行为优先级计算类
    StrategyPriority strategyPriority;
    //ai行动脚本
    EnemyBehaviours behaviours;
    //ai状态机
    EnemyAIStateMachine aism;
    //移动目标
    Vector3 currentGoal;
    Transform goalTransform;
    bool isGoalEmpty;

    [SerializeField] EnemyAIConfig AIConfig;
    private float time;//计时器
    private Dictionary<EnemyArmyBehavior, Action> behaviorDic;//状态切换字典

    protected void Start()
    {
        //初始化寻路代理、环境感知、决策优先级计算
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        context = GetComponent<EnemyContext>();
        behaviours = GetComponent<EnemyBehaviours>();
        behaviours.InitBehaviours(agent, context, AIConfig, ArmyDetail);
        aism = GetComponent<EnemyAIStateMachine>();
        aism.InitEnemyAIStateMachine(AIConfig, behaviours);
        strategyPriority = new StrategyPriority(AIConfig, ArmyDetail, context);
        InitBehaviorDic();
    }

    //初始化状态切换字典
    private void InitBehaviorDic()
    {
        behaviorDic = new Dictionary<EnemyArmyBehavior, Action>
        {
            {
                EnemyArmyBehavior.moveToCity,
                ()=>{ aism.ChangeState(new MoveToCityState(agent, context, ref aism)); }
            },
            {
                EnemyArmyBehavior.retreat,
                ()=>{ aism.ChangeState(new RetreatState(agent, context, ref aism)); }
            },
            {
                EnemyArmyBehavior.moveForward,
                ()=>{ aism.ChangeState(new MoveForwardState(agent, context, ref aism)); }
            },
            {
                EnemyArmyBehavior.station,
                ()=>{ aism.ChangeState(new StationState(agent, context, ref aism)); }
            },
            {
                EnemyArmyBehavior.stayInCity,
                ()=>{ aism.ChangeState(new StayInCityState(agent, context, ref aism)); }
            },
            {
                EnemyArmyBehavior.attack,
                ()=>{ aism.ChangeState(new AttackState(agent, context, ref aism)); }
            },
            {
                EnemyArmyBehavior.healing,
                ()=>{ aism.ChangeState(new HealingState(agent, context, ref aism)); }
            }
        };
    }

    //private void FindNearestCityAndPlayerAndEnemy()//检测附近城市和玩家，并按直线距离进行排序
    //{
    //    nearCitys.Clear();
    //    playerArmy.Clear();
    //    enemys.Clear();
    //    for (int i = 0; i < rayCount; i++)
    //    {
    //        float angle = i * raySpacing; // 当前射线的角度
    //        Vector3 rayDirection = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)); // 射线方向
    //        RaycastHit hit;
    //        if (Physics.Raycast(transform.position, rayDirection, out hit, radarRange))
    //        {
    //            // 射线与物体相交
    //            if (hit.collider.CompareTag("City"))
    //            {
    //                nearCitys.Add(hit.transform.position);
    //            }
    //            else if (hit.collider.CompareTag("Player"))
    //            {
    //                playerArmy.Add(hit.transform.GetComponent<Army>());
    //            }
    //            else if (hit.collider.CompareTag("Enemy"))
    //            {
    //                enemys.Add(hit.transform.GetComponent<Army>());
    //            }
    //        }
    //    }
    //    //冒泡排序
    //    for (int j = nearCitys.Count; j >= 1; j--)
    //    {
    //        for (int i = 1; i < j; i++)
    //        {
    //            if (Vector3.Distance(nearCitys[i - 1], transform.position) < Vector3.Distance(nearCitys[i], transform.position))
    //            {
    //                Vector3 temp = nearCitys[i];
    //                nearCitys[i] = nearCitys[i - 1];
    //                nearCitys[i - 1] = temp;
    //            }
    //        }
    //    }

    //    for (int j = playerArmy.Count; j >= 1; j--)
    //    {
    //        for (int i = 1; i < j; i++)
    //        {
    //            if (Vector3.Distance(playerArmy[i - 1].transform.position, transform.position) < Vector3.Distance(playerArmy[i].transform.position, transform.position))
    //            {
    //                Army temp = playerArmy[i];
    //                playerArmy[i] = playerArmy[i - 1];
    //                playerArmy[i - 1] = temp;
    //            }
    //        }
    //    }

    //    for (int j = enemys.Count; j >= 1; j--)
    //    {
    //        for (int i = 1; i < j; i++)
    //        {
    //            if (Vector3.Distance(enemys[i - 1].transform.position, transform.position) < Vector3.Distance(enemys[i].transform.position, transform.position))
    //            {
    //                Army temp = enemys[i];
    //                enemys[i] = enemys[i - 1];
    //                enemys[i - 1] = temp;
    //            }
    //        }
    //    }
    //}

    
    protected void IsArriveDestination()
    {
        Vector3 finalGoal;
        if(goalTransform != null)
        {
            finalGoal = goalTransform.position;
        }
        else
        {
            finalGoal = currentGoal;
        }
        if (Vector3.Distance(transform.position, goalTransform.position) <= AIConfig.minDis)
        {
            agent.isStopped = true;
            goalTransform = null;
            currentGoal = Vector3.zero;
        }
    }

    public void SetMoveGoal(Transform transform)
    {
        goalTransform = transform;
    }

    public void SetMoveGoal(Vector3 goal)
    {
        currentGoal = goal;
    }

    public float GetPeopleLimit()
    {
        return peopleLimit;
    }

    protected void ChooseBehaviour()
    {
        behaviorDic[strategyPriority.CaculatePriority()]();
    }

    protected void UpdateState()
    {
        time += Time.deltaTime;
        if(time >= 2)
        {
            time = 0;
            ChooseBehaviour();
        }
    }

    protected void Update()
    {
        UpdateState(); 
        if (!isGoalEmpty)
        {
            IsArriveDestination();
        }
    }

}
