using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class EnemyContext : MonoBehaviour
{
    //附近城市
    public List<Vector3> nearCitys = new List<Vector3>();

    //附近敌方军队
    public List<Army> enemys = new List<Army>();
    public float enemyAttack
    {
        get 
        {
            float attack = 0;
            for (int i = 0; i < enemys.Count; i++)
            {
                attack += enemys[i].GetStrength() * enemys[i].GetPeople();
            }
            return attack;
        }
        set
        {
            enemyAttack = value;
        }
    }
    //附近玩家军队
    public List<Army> playerArmy = new List<Army>();
    public float playerAttack
    {
        get
        {
            float attack = 0;
            for (int i = 0; i < enemys.Count; i++)
            {
                attack += playerArmy[i].GetStrength() * playerArmy[i].GetPeople();
            }
            return attack;
        }
        set
        {
            enemyAttack = value;
        }
    }



    #region city、enemy和player增减
    public void AddCity(Vector3 city)
    {
        nearCitys.Add(city);
    }

    public void RemoveCity(Vector3 city)
    {
        nearCitys.Remove(city);
    }

    public void AddEnemy(Army army)
    {
        enemys.Add(army);
        //enemyAttack += army.GetPeople() * army.GetStrength();
    }

    public void RemoveEnemy(Army army)
    {
        enemys.Remove(army);
        //enemyAttack -= army.GetPeople() * army.GetStrength();
        //if (enemyAttack < 0)
        //{
        //    enemyAttack = 0;
        //}
    }

    public void AddPlayerArmy(Army army)
    {
        playerArmy.Add(army);
        //playerAttack += army.GetPeople() * army.GetStrength();
    }

    public void RemovePlayerArmy(Army army)
    {
        playerArmy.Remove(army);
        //playerAttack -= army.GetPeople() * army.GetStrength();
        //if (playerAttack < 0)
        //{
        //    playerAttack = 0;
        //}
    }
    #endregion

    //用trigger来增删附近城市和红军
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Army enemy = other.GetComponent<Army>();
            AddEnemy(enemy);
        }
        else if (other.CompareTag("City"))
        {
            AddCity(other.transform.position);
        }
        else if (other.CompareTag("Player"))
        {
            Army player = other.GetComponent<Army>();
            AddPlayerArmy(player);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Army enemy = other.GetComponent<Army>();
            RemoveEnemy(enemy);
        }
        else if (other.CompareTag("City"))
        {
            RemoveCity(other.transform.position);
        }
        else if (other.CompareTag("Player"))
        {
            Army player = other.GetComponent<Army>();
            RemovePlayerArmy(player);
        }
    }

    public void Ordination()//按直线距离对军队和城市进行冒泡排序
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

    public Vector3 GetNearestCity()
    {
        Ordination();
        return nearCitys[0];
    }

    public Army GetNearestPlayerArmy()
    {
        Ordination();
        return playerArmy[0];
    }

    public Army GetNearestEnemyArmy()
    {
        Ordination();
        return enemys[0];
    }
}
