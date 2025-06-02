using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

enum ChooseType
{
    moveforward,
    surround,
    support
}

[RequireComponent(typeof(EnemyArmyMove))]
public class EnemyAIManager : MonoBehaviour
{
    public static EnemyAIManager Instance => instance;
    private static EnemyAIManager instance;

    [SerializeField]
    List<City> allCitys;//在inspector面板处赋初值，攻占城市时改变标识符
    [SerializeField]
    List<City> warline;//分割战场的战线，用战线上的玩家城市表示
    [HideInInspector]
    public List<City> playerCity;
    [HideInInspector]
    public List<City> enemyCity;
    [SerializeField]
    Transform target;
    [SerializeField]
    List<EnemyAI> enemies;
    [HideInInspector]
    public List<Army> allPlayerArmys;//如何维护？
    float time;

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
        InitDistributionOfCity();
    }

    //初始化城市阵营分布
    void InitDistributionOfCity()
    {
        for (int i = 0; i < allCitys.Count; i++)
        {
            if (allCitys[i].isBelongToPlayer)
            {
                playerCity.Add(allCitys[i]);
                continue;
            }
            else
            {
                enemyCity.Add(allCitys[i]);
            }
        }
    }

    //在前线城市占领时调用
    public void ChangeWarline(bool isPlayerArmy, City city)
    {
        if(!isPlayerArmy)//敌军占领玩家城市改变战线
        {
            city.isBelongToPlayer = false;
            warline.Remove(city);
            playerCity.Remove(city);
            enemyCity.Add(city);
            //获取离被占领城市最近的玩家城市，将其加入战线中
            float mindis;
            City temp;
            if (allCitys[0] != city)
            {
                mindis = Vector3.Distance(allCitys[0].transform.position, city.transform.position);
                temp = allCitys[0];
            }
            else
            {
                mindis = Vector3.Distance(allCitys[1].transform.position, city.transform.position);
                temp = allCitys[1];
            }
            for (int i = 0; i < allCitys.Count; i++)
            {
                if (allCitys[i].isBelongToPlayer == true && !warline.Contains(allCitys[i]) && allCitys[i] != city)
                {
                    float dis = Vector3.Distance(allCitys[i].transform.position, city.transform.position);
                    if(dis < mindis)
                    {
                        mindis = dis;
                        temp = allCitys[i];
                    }
                }
            }
            warline.Add(temp);
            Debug.Log(temp.name);
        }
        else//玩家占领敌军城市
        {
            playerCity.Add(city);
            enemyCity.Remove(city);
            warline.Add(city);
            city.isBelongToPlayer = true;
            float mindis1,mindis2,mindis3;
            City temp1, temp2, temp3;
            if (warline.Count > 4)
            {
                mindis1 = Vector3.Distance(allCitys[0].transform.position, city.transform.position);
                temp1 = allCitys[0];
                mindis2 = mindis1; mindis3 = mindis1;
                temp2 = temp1; temp3 = temp1;
                #region 找到离city最近的三个城市
                for (int i = 0;i < warline.Count - 1;i++)//找到离city最近的
                {
                    float dis = Vector3.Distance(allCitys[i].transform.position, city.transform.position);
                    if (dis < mindis1)
                    {
                        mindis1 = dis;
                        temp1 = allCitys[i];
                    }
                }
                for (int i = 0; i < warline.Count - 1; i++)//找到离city第二近的
                {
                    float dis = Vector3.Distance(allCitys[i].transform.position, city.transform.position);
                    if (mindis1 < dis && dis < mindis2)
                    {
                        mindis2 = dis;
                        temp2 = allCitys[i];
                    }
                }
                for (int i = 0; i < warline.Count - 1; i++)//找到离city第三近的
                {
                    float dis = Vector3.Distance(allCitys[i].transform.position, city.transform.position);
                    if (mindis2 < dis && dis < mindis3)
                    {
                        mindis3 = dis;
                        temp3 = allCitys[i];
                    }
                }
                #endregion
                Vector3 meanPosition = Vector3.zero;//计算中心点
                meanPosition = (meanPosition + temp1.transform.position + temp2.transform.position + temp3.transform.position) / 3;
                //计算方向向量
                Vector2 direction = new Vector2(meanPosition.x - city.transform.position.x, meanPosition.z - city.transform.position.z);
                float dis1 = GeometryUtils.PointToLineDistance(new Vector2(temp1.transform.position.x, temp1.transform.position.z), direction,
                                                               new Vector2(city.transform.position.x, city.transform.position.z));
                float dis2 = GeometryUtils.PointToLineDistance(new Vector2(temp2.transform.position.x, temp2.transform.position.z), direction,
                                                               new Vector2(city.transform.position.x, city.transform.position.z));
                float dis3 = GeometryUtils.PointToLineDistance(new Vector2(temp3.transform.position.x, temp3.transform.position.z), direction,
                                                               new Vector2(city.transform.position.x, city.transform.position.z));
                float finaldis = 0;
                City finalcity;
                if(dis1 < dis2)
                {
                    finaldis = dis1;
                    finalcity = temp1;
                }
                else
                {
                    finaldis = dis2;
                    finalcity = temp2;
                }
                if(finaldis > dis3)
                {
                    finaldis = dis3;
                    finalcity = temp3;
                }
                warline.Remove(finalcity);
            }
        }
    }

    //推进战线（占领更多城市）
    void Moveforward()
    {
        List<EnemyAI> movingenemy = ChooseEnemy(ChooseType.moveforward);
        for (int i = 0; i < movingenemy.Count; i++)
        {
            movingenemy[i].agent.SetDestination(FindNearestCity(movingenemy[i]).transform.position);
        }
    }

    //找到离该部队最近的玩家方城市
    City FindNearestCity(EnemyAI enemy)
    {
        City nearestCity = playerCity[0];
        float mindis = Vector3.Distance(playerCity[0].transform.position, enemy.transform.position);
        for (int i = 1;i < playerCity.Count;i++)
        {
            float dis = Vector3.Distance(playerCity[i].transform.position, enemy.transform.position);
            if (dis < mindis)
            {
                mindis = dis;
                nearestCity = playerCity[i];
            }
        }
        return nearestCity;
    }

    List<EnemyAI> ChooseEnemy(ChooseType choosetype)
    {
        List<EnemyAI> chosen = new List<EnemyAI>();
        switch (choosetype)
        {
            case ChooseType.moveforward:
                {
                    chosen = ChooseMovingEnemy();
                }
                break;
            case ChooseType.surround:
                {
                    chosen = ChooseSurroundEnemy();
                }break;
        }
        return chosen;
    }

    //挑选推进战线的部队单位(随机挑选)
    List<EnemyAI> ChooseMovingEnemy()
    {
        List<EnemyAI> chosenEnemy = new List<EnemyAI>();
        int choosecount = Random.Range(1,enemies.Count/3);
        for (int i = 0; i < choosecount; i++)
        {
            int num = Random.Range(0, enemies.Count);
            //确保选中的军队没有重复且血量健康
            while (!chosenEnemy.Contains(enemies[num]) && enemies[num].ArmyDetail["people"] > enemies[num].GetPeopleLimit()/2)
            {
                num = Random.Range(0, enemies.Count);
            }
            chosenEnemy.Add(enemies[num]);
            Debug.Log("挑选的军队是" + enemies[num].name);
        }
        return chosenEnemy;
    }

    //挑选实行包围战略的部队单位
    List<EnemyAI> ChooseSurroundEnemy()
    {
        OrderPlayerArmy();
        List<EnemyAI> chosenEnemy = new List<EnemyAI>();
        for (int i = 0;i < allPlayerArmys[0].nearbyEnemyArmy.Count; i++)
        {
            chosenEnemy.Add(allPlayerArmys[0].nearbyEnemyArmy[i] as EnemyAI);
        }
        return chosenEnemy;
    }

    void SurroundArmy()
    {
        List<EnemyAI> surrundEnemy = ChooseEnemy(ChooseType.surround);
        EnemyArmyMove.Instance.SurroundArmy(allPlayerArmys[0].transform, surrundEnemy.ToArray());
    }

    //根据周围敌人军队的多少来排序
    void OrderPlayerArmy()
    {
        QuickSort(allPlayerArmys, 0, allPlayerArmys.Count - 1);

        void QuickSort(List<Army> array, int left, int right)
        {
            if (left < right)
            {
                // 找到分区点
                int partitionIndex = Partition(array, left, right);

                // 递归排序左右两部分
                QuickSort(array, left, partitionIndex - 1);
                QuickSort(array, partitionIndex + 1, right);
            }
        }

        int Partition(List<Army> array, int left, int right)
        {
            // 选择最右边的元素作为基准值
            int pivot = (array[right] as Army).nearbyEnemyArmy.Count;
            int partitionIndex = left;

            // 遍历数组，将小于基准值的元素移到左边
            for (int i = left; i < right; i++)
            {
                if ((array[i] as Army).nearbyEnemyArmy.Count > pivot)
                {
                    Army temp0 = array[i];
                    array[i] = array[partitionIndex];
                    array[partitionIndex] = temp0;
                    partitionIndex++;
                }
            }

            // 将基准值移到分区点
            Army temp = array[partitionIndex];
            array[partitionIndex] = array[right];
            array[right] = temp;

            return partitionIndex;
        }

    }

    //AI总指挥，可以指挥单个ai推进战线，包围玩家，支援队友
    void CaculatePriority()
    {
        float moveforward;
        float surrondarmy;

    }

    void UpdataStrategy()
    {
        time += Time.deltaTime;
        if (time >= 5)
        {
            CaculatePriority();
        }
    }

    //allPlayerArmys的增减,在分兵、合并、击杀时可用
    void AddArmyToAllPlayerArmys(Army newarmy)
    {
        allPlayerArmys.Add(newarmy);
    }

    void RemoveArmyFromPlayerArmys(Army deletedarmy)
    {
        if(allPlayerArmys.Contains(deletedarmy))
        {
            allPlayerArmys.Remove(deletedarmy);
        }
        else
        {
            Debug.LogError("deletedarmy is not in allPlayerArmys");
        }
    }
}
