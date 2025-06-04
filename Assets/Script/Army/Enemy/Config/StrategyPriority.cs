using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEditor.Progress;

//决策优先级计算函数类
public class StrategyPriority
{
    private EnemyAI enemyAI;
    private EnemyAIConfig AIConfig;
    private EnemyContext context;

    SerializableDictionaryBase<string, float> ArmyDetail = new SerializableDictionaryBase<string, float>();

    public StrategyPriority(EnemyAIConfig AIConfig, SerializableDictionaryBase<string, float> ArmyDetail, EnemyContext context, EnemyAI enemyAI)
    {
        this.AIConfig = AIConfig;
        this.ArmyDetail = ArmyDetail;
        this.context = context;
        this.enemyAI = enemyAI;
    }

    //敌军单个部队有以下几种动作，移动到城镇，撤退，前进，原地待机，守城，追击红军，回血

    public float MoveToCity()
    {
        if(context.nearCitys.Count == 0)
        {
            return -100f;
        }
        float priority;
        priority = (float)(2*AIConfig.deffence * 
            (Vector3.Distance(context.transform.position, context.GetNearestCity()) / AIConfig.checkRadius));
        return priority;
    }

    public float ReTreat()
    {
        if(context.playerArmy.Count == 0)return -100f;
        float priority;
        priority = AIConfig.attack * Math.Clamp((context.playerAttack * 0.9f - context.enemyAttack) ,-1000,1000)/1000+ 
            AIConfig.deffence * (1 - enemyAI.GetPeople()/enemyAI.GetPeopleLimit());
        return priority;
    }

    //占领玩家城市
    public float MoveForward()
    {
        //if(context.playerArmy.Count == 0)return 0f;
        float priority;
        priority = AIConfig.attack * Math.Clamp((context.enemyAttack - context.playerAttack),-1000,1000)/1000+
            + AIConfig.attack * (Math.Clamp(Vector3.Distance(enemyAI.transform.position, EnemyAIManager.Instance.FindNearestCity(enemyAI).transform.position),0,100)/100);
        return priority;
    }

    public float Station()
    {
        if (context.playerArmy.Count == 0) return -100f;
        float priority;
        priority = (float)(AIConfig.deffence * Math.Clamp((context.playerAttack - context.enemyAttack * 0.75),-1000,1000)/1000 + AIConfig.deffence *
            (Vector3.Distance(context.transform.position, context.GetNearestPlayerArmy().transform.position) / AIConfig.checkRadius));
        return priority;
    }

    public float Attack()
    {
        if (context.playerArmy.Count == 0) return -100f;
        float priority;
        priority = AIConfig.attack * Math.Clamp((context.enemyAttack - context.playerAttack),-1000,1000)/1000 + 
            AIConfig.attack * Math.Clamp(Vector3.Distance(context.transform.position, context.GetNearestPlayerArmy().transform.position),0,100)/100;
        //如果有军委总队增加概率
        return priority;
    }

    public float Healing()
    {
        if (context.nearCitys.Count == 0) return (1 - ArmyDetail["people"] / ArmyDetail["peopleLimit"]) *100;
        float priority;
        priority = ((1 - enemyAI.GetPeople() / enemyAI.GetPeopleLimit())) * AIConfig.deffence + 
            AIConfig.deffence * Math.Clamp(Vector3.Distance(context.GetNearestPlayerArmy().transform.position, context.transform.position),0,100)/100 ;
        return priority;
    }

    public float NeedSupport()
    {
        float priority = 0;
        List<EnemyAI> nearEnemy = EnemyAIManager.Instance.FindNearestEnemyArmy(enemyAI);
        priority = AIConfig.attack * Math.Clamp(Vector3.Distance(context.transform.position, nearEnemy[0].transform.position), 0, 500) / 500 +
            AIConfig.attack * Math.Clamp((context.playerAttack - context.enemyAttack), 0, 1000) / 1000;
        return priority;
    }

    //计算当前最优行动
    public EnemyArmyBehavior CaculatePriority()
    {
        //计算行动优先级
        float[] prioritys = new float[7];
        Debug.Log(prioritys.Length);
        prioritys[(int)EnemyArmyBehavior.moveToCity] = MoveToCity();
        prioritys[(int)EnemyArmyBehavior.retreat] = ReTreat();
        prioritys[(int)EnemyArmyBehavior.moveForward] = MoveForward();
        prioritys[(int)EnemyArmyBehavior.station] = Station();
        prioritys[(int)EnemyArmyBehavior.attack] = Attack();
        prioritys[(int)EnemyArmyBehavior.healing] = Healing();
        prioritys[(int)EnemyArmyBehavior.needsupport] = NeedSupport();
        float max = prioritys[0];
        EnemyArmyBehavior index = 0;
        //找到优先级最大的然后执行
        for (int i = 0; i < prioritys.Length; i++)
        {
            if (prioritys[i] > max)
            {
                max = prioritys[i];
                index = (EnemyArmyBehavior)i;
            }
            Debug.Log(prioritys[i]);
        }
        return index;
    }
    
}
