using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyArmy : Army
{
    protected NavMeshAgent agent;
    public NavMeshAgent Agent;
    public Transform agentGoal;
    [SerializeField]
    protected float minDis = 5;

    //附近城市
    List<Vector3> nearCitys = new List<Vector3>();

    //附近敌方军队
    List<Army> enemys = new List<Army>();
    float enemyAttack;
    //附近玩家军队
    List<Army> playerArmy = new List<Army>();
    float playerAttack;

    [SerializeField]
    float checkRadio = 100;
    //#region 雷达模拟
    //[Header("模拟雷达")]
    //[SerializeField]
    //int rayCount = 360;
    //float raySpacing;
    //[SerializeField]
    //float radarRange = 10f;
    //#endregion

    #region 一些状态变量
    [SerializeField]
    bool isInCity = false, isMoving = false;

    #endregion
    protected float oMove, oAttack, oStation, oDeffence; 
    protected float move = 4, attack = 2, station = 3, deffence = 2.5f;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        oMove = move;oAttack = attack;oStation = station;oDeffence = deffence;
        //raySpacing = 360f / rayCount;
    }
    
    //敌军单个部队有以下几种动作，移动到城镇，撤退，前进，原地布防，守城，追击红军，回血
   
    private float MoveToCity()
    {
        if (isInCity) return 0;
        float priority;
        priority = (float)(deffence * (-Vector3.Distance(transform.position, nearCitys[0])/checkRadio + checkRadio));
        return priority;
    }

    private float ReTreat()
    {
        float priority;
        priority = move * ((playerAttack - enemyAttack) / 1000)%100 ;
        return priority;
    }

    private float MoveForward()
    {
        float priority;
        priority = move * ((enemyAttack - playerAttack) / 1000)%100;
        return priority;
    }

    private float StationPriority()
    {
        if (!isInCity) return 0;
        float priority;
        priority = (float)(move * (playerAttack - enemyAttack*0.75)%10 * (-Vector3.Distance(transform.position, playerArmy[0].transform.position)/checkRadio + checkRadio));
        return priority;
    }

    private float StayInCity()
    {
        if (isInCity) return 0;
        deffence = (-Vector3.Distance(transform.position, nearCitys[0]) / checkRadio + checkRadio);
        float priority;
        priority = deffence * ((enemyAttack - playerAttack) / 1000)%100;
        return priority;
    }

    private float AttackPriority()
    {
        float priority;
        priority = attack * (enemyAttack - playerAttack) * (speed - (playerArmy[0].GetSpeed() + playerArmy[1].GetSpeed())/2);
        return priority;
    }

    private float DeffencePriority()
    {
        float priority;
        priority = move * (500 - playerAttack);
        return priority;
    }

    protected void Healing()
    {

    }

    protected void MoveDirectly(Transform goal)
    {
        agentGoal = goal;
        agent.SetDestination(agentGoal.position);
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

    private void Ordination()//按直线距离对军队和城市进行冒泡排序
    {
        //冒泡排序
        for (int j = nearCitys.Count; j >= 1; j--)
        {
            for (int i = 1; i < j; i++)
            {
                if (Vector3.Distance(nearCitys[i - 1], transform.position) < Vector3.Distance(nearCitys[i], transform.position))
                {
                    Vector3 temp = nearCitys[i];
                    nearCitys[i] = nearCitys[i - 1];
                    nearCitys[i - 1] = temp;
                }
            }
        }

        for (int j = playerArmy.Count; j >= 1; j--)
        {
            for (int i = 1; i < j; i++)
            {
                if (Vector3.Distance(playerArmy[i - 1].transform.position, transform.position) < Vector3.Distance(playerArmy[i].transform.position, transform.position))
                {
                    Army temp = playerArmy[i];
                    playerArmy[i] = playerArmy[i - 1];
                    playerArmy[i - 1] = temp;
                }
            }
        }

        for (int j = enemys.Count; j >= 1; j--)
        {
            for (int i = 1; i < j; i++)
            {
                if (Vector3.Distance(enemys[i - 1].transform.position, transform.position) < Vector3.Distance(enemys[i].transform.position, transform.position))
                {
                    Army temp = enemys[i];
                    enemys[i] = enemys[i - 1];
                    enemys[i - 1] = temp;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Army enemy = other.GetComponent<Army>();
            enemys.Add(enemy);
            enemyAttack += enemy.GetPeople() * enemy.GetStrength();
        }
        else if(other.CompareTag("City"))
        {
            nearCitys.Add(other.transform.position);
        }
        else if(other.CompareTag("Player"))
        {
            Army player = other.GetComponent<Army>();
            playerArmy.Add(player);
            playerAttack += player.GetPeople() * player.GetStrength();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Army enemy = other.GetComponent<Army>();
            enemys.Remove(enemy);
            enemyAttack -= enemy.GetPeople() * enemy.GetStrength();
        }
        else if (other.CompareTag("City"))
        {
            nearCitys.Remove(other.transform.position);
        }
        else if (other.CompareTag("Player"))
        {
            Army player = other.GetComponent<Army>();
            if(playerArmy.Contains(player))
            {
                playerArmy.Remove(player);
                playerAttack -= player.GetPeople() * player.GetStrength();
            }
        }
    }

    protected bool IsArriveDestination(ref Transform  agentGoal, NavMeshAgent agent)
    {
        if (Vector3.Distance(transform.position, agentGoal.position) <= minDis)
        {
            agent.ResetPath();
            agentGoal = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Update()
    {
        if (agentGoal != null)
        {
            IsArriveDestination(ref agentGoal, agent);
        }
    }

}
