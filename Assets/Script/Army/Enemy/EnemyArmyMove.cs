using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyArmyMove: MonoBehaviour
{
    public static EnemyArmyMove Instance => instance;
    private static EnemyArmyMove instance;

    [SerializeField]
    float minDis;

    [SerializeField]
    float dispersionThreshold = 5.0f;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(this);
            }
        }
    }

    
    /// <summary>
    /// 包围目标军队
    /// </summary>
    /// <param name="target">
    /// 被包围的军队
    /// </param>
    /// <param name="enemyArmies">
    /// 包围目标的军队
    /// </param>
    public void SurroundArmy(Transform target, params EnemyAI[] enemyArmies)
    {
        if(enemyArmies == null || enemyArmies.Length <= 0)
        {
            Debug.LogError("传入的敌军数量小于等于0");
        }
        foreach(var enemy in enemyArmies)
        {
            enemy.SetMoveGoal(target);
        }
        EnemyAI[] oneSide, otherSide;
        DivideArmies(target, enemyArmies, out oneSide, out otherSide);
        SurroundTragetFromOneSide(target, Tool.TurnIntoComponent<NavMeshAgent>(oneSide));
        if(otherSide != null)
        {
            SurroundTragetFromOneSide(target, Tool.TurnIntoComponent<NavMeshAgent>(otherSide));
        }
    }

    //将Ai军队分成两类，形成两面包夹芝士
    private void DivideArmies(Transform target, EnemyAI[] enemyArmies, out EnemyAI[] oneSide, out EnemyAI[] otherSide)
    {
        List<EnemyAI> group1 = new List<EnemyAI>();
        List<EnemyAI> group2 = new List<EnemyAI>();
        Transform[] transforms = Tool.TurnIntoTransform(enemyArmies);
        if (GeometryUtils.CalculateDispersion(transforms) >= dispersionThreshold)
        {
            //稀疏程度较大则分成两类
            Vector3 direction = GeometryUtils.CalculateDynamicBoundary(target, transforms);
            foreach (EnemyAI obj in enemyArmies)
            {
                Vector3 toObj = obj.transform.position - target.position;
                float dot = Vector3.Dot(toObj.normalized, direction);
                if (dot >= 0)
                    group1.Add(obj);
                else
                    group2.Add(obj);
            }
        }
        else
        {
            //稀疏程度较小则只分成一类
            foreach (EnemyAI obj in enemyArmies)
            {
                group1.Add(obj);
            }
        }
        oneSide = group1.ToArray();
        otherSide = group2.ToArray();
    }

    private void SurroundTragetFromOneSide(Transform target, params NavMeshAgent[] enemyAgents)
    {
        int minCostIndex = 0;
        float minCost = Tool.CaculatePathCost(target, enemyAgents[0]);
        for (int i = 1; i < enemyAgents.Length; i++)
        {
            float cost = Tool.CaculatePathCost(target, enemyAgents[i]);
            if (cost < minCost)
            {
                minCostIndex = i;
                minCost = cost;
            }
        }

        for (int i = 0; i < enemyAgents.Length; i++)
        {
            if(i != minCostIndex)
            {
                SurroundTraget(target, enemyAgents[minCostIndex], enemyAgents[i]);
            }
        }
    }

    private void SurroundTraget(Transform target, NavMeshAgent agent1, NavMeshAgent agent2)
    {
        //军队联线中点——中点1
        Vector3 armyMiddlePoint = new Vector3((agent1.transform.position.x + agent2.transform.position.x) / 2, 0,
            (agent1.transform.position.z + agent2.transform.position.z) / 2);
        //中点1和target的中点——中点2
        Vector3 armyMiddleAndTargetMiddle = new Vector3((armyMiddlePoint.x + target.position.x) / 2, 0,
            (armyMiddlePoint.z + target.position.z) / 2);
        //中点1指向中点2的向量——向量1
        Vector3 vector1 = new Vector3();
        vector1 = armyMiddleAndTargetMiddle - armyMiddlePoint;

        Queue anotherPath = new Queue();
        StartCoroutine(newPath());

        IEnumerator newPath()
        {
            NavMeshPath path = new NavMeshPath();
            List<NavMeshAgent> agents = new List<NavMeshAgent>();

            //获取离目标最近的agent的路线
            //if (Vector3.Distance(agent1.transform.position, target.position) <
            //    Vector3.Distance(agent2.transform.position, target.position))
            //{

            //}
            //else
            //{
            //    agent2.CalculatePath(target.position, path);
            //    agents.Add(agent2);
            //    agents.Add(agent1);
            //}

            //默认agent1使用navigation自动寻路
            agent1.CalculatePath(target.position, path);
            agents.Add(agent1);
            agents.Add(agent2);
            yield return new WaitUntil(() => path.status == NavMeshPathStatus.PathComplete
            || path.status == NavMeshPathStatus.PathInvalid);

            Debug.Log(path.corners.Length);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                for (int i = 0; i < path.corners.Length; i++)
                {
                    Vector3 vector3 = GeometryUtils.ReflectPointAcrossLine(armyMiddlePoint, target.position, path.corners[i]);
                    NavMeshHit hit = new NavMeshHit();
                    if (NavMesh.SamplePosition(vector3, out hit, 10f, NavMesh.AllAreas))
                    {
                        anotherPath.Enqueue(hit.position); // 正确使用采样后的点
                    }
                    //NavMesh.SamplePosition(vector3, out hit, 10f, NavMesh.AllAreas);
                    //anotherPath.Enqueue(vector3 + hit.position);
                }
            }
            StartCoroutine(move(agents));
        }

        IEnumerator move(List<NavMeshAgent> agents)
        {
            agents[0].SetDestination(target.position);
            while (anotherPath.Count > 0)
            {
                Vector3 vector3 = (Vector3)anotherPath.Dequeue();
                agents[1].SetDestination(vector3);
                yield return new WaitUntil(() => Vector3.Distance(agents[1].transform.position, vector3) <= minDis);
            }
        }
    }

}
