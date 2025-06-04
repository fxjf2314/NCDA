using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[HideInInspector]
public class EnemyBehaviours : MonoBehaviour
{
    private EnemyAI enemyAI;
    private NavMeshAgent agent;
    private EnemyContext context;
    private EnemyAIConfig AIConfig;
    private SerializableDictionaryBase<string, float> EnemyArmyDetail;

    public Coroutine healing, attack;

    public EnemyBehaviours(EnemyAI enemyAI,NavMeshAgent agent, EnemyContext context, EnemyAIConfig config, ref SerializableDictionaryBase<string, float> EnemyArmyDetail)
    {
        this.enemyAI = enemyAI;
        this.agent = agent;
        this.context = context;  
        this.AIConfig = config;
        this.EnemyArmyDetail = EnemyArmyDetail;
    }

    public void InitBehaviours(EnemyAI enemyAI,NavMeshAgent agent, EnemyContext context, EnemyAIConfig config, SerializableDictionaryBase<string, float> EnemyArmyDetail)
    {
        this.enemyAI = enemyAI;
        this.agent = agent;
        this.context = context;
        this.AIConfig = config;
        this.EnemyArmyDetail = EnemyArmyDetail;
    }

    //navmeshagent自动寻路
    public void MoveDirectly(Vector3 position)
    {
        NavMeshHit hit = new NavMeshHit();
        NavMesh.SamplePosition(position, out hit, 10f, NavMesh.AllAreas);
        enemyAI.SetMoveGoal(position);
    }

    //朝着反方向撤退(慌不择路)
    public void Retreat()
    {
        //Debug.Log("retreat");
        //Army enemy = context.GetNearestEnemyArmy();
        //if (enemy != null )
        //{
        //    enemyAI.SetMoveGoal(enemy.transform);
        //}
        //else
        //{
            List<Vector3> path = GeometryUtils.CalculateReTreatPath(transform, context.playerArmy.ToArray());
            Queue<Vector3> navPath = new Queue<Vector3>();
            for (int i = 0; i < path.Count; i++)
            {
                NavMeshHit hit = new NavMeshHit();
                NavMesh.SamplePosition(path[i], out hit, 10f, NavMesh.AllAreas);
                navPath.Enqueue(hit.position);
            }
            StartCoroutine(move(agent));
            IEnumerator move(NavMeshAgent agent)
            {
                while (navPath.Count > 0)
                {
                    Vector3 vector3 = (Vector3)navPath.Dequeue();
                    agent.SetDestination(vector3);
                    yield return new WaitUntil(() => Vector3.Distance(agent.transform.position, vector3) <= AIConfig.minDis);
                }
            }
        //}
    }

    public void MoveToCity()
    {
        Debug.Log("move to city");
        Vector3 city = context.GetNearestCity();
        MoveDirectly(city);
    }
    
    //前进,攻占玩家方城市
    public City MoveForward()
    {
        Debug.Log("moveforward");
        City playerCity = EnemyAIManager.Instance.FindNearestCity(enemyAI);
        enemyAI.SetMoveGoal(playerCity.transform);
        return playerCity;
    }

    public void Station()
    {
        Debug.Log("station");
        enemyAI.StopMove();
    }

    public void Attack()
    {
        Debug.Log("attack");
        //先过去
        Army player = context.GetNearestPlayerArmy();
        enemyAI.SetMoveGoal(player.transform);
        //攻击
        attack = StartCoroutine(AttackPlayer());
        IEnumerator AttackPlayer()
        {
            while (true)
            {
                if(player != null)
                if (Vector3.Distance(transform.position, player.transform.position) < AIConfig.minDis)
                {
                    agent.isStopped = true;
                    player.ArmyDetail["people"] -= EnemyArmyDetail["people"] * EnemyArmyDetail["strength"];
                    yield return new WaitForSeconds(2);
                }
                else
                {
                    agent.isStopped = true;
                    break;
                }
                yield return null;
            }
        }
    }

    public void StopAttack()
    {
        StopCoroutine(attack);
        enemyAI.StopMove();
    }

    public void Healing()
    {
        Debug.Log("治疗");
        enemyAI.StopMove();
        this.healing = StartCoroutine(healing());
        IEnumerator healing()
        {
            while (EnemyArmyDetail["people"] < EnemyArmyDetail["peopleLimit"])
            {
                EnemyArmyDetail["people"] += AIConfig.healingSpeed;
                yield return new WaitForSeconds(1);
            }
            if(EnemyArmyDetail["people"] > EnemyArmyDetail["peopleLimit"])
            {
                EnemyArmyDetail["people"] = EnemyArmyDetail["peopleLimit"];
            }
            this.healing = null;
        }
    }

    public void StopHealing()
    {
        StopCoroutine(healing);
    }
}
