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
    private EnemyAIConfig AIConfig;
    private EnemyContext context;

    SerializableDictionaryBase<string, float> ArmyDetail = new SerializableDictionaryBase<string, float>();

    public StrategyPriority(EnemyAIConfig aIConfig, SerializableDictionaryBase<string, float> ArmyDetail, EnemyContext context)
    {
        AIConfig = aIConfig;
        this.ArmyDetail = ArmyDetail;
        this.context = context;
    }

    //敌军单个部队有以下几种动作，移动到城镇，撤退，前进，原地待机，守城，追击红军，回血

    public float MoveToCity()
    {
        if(context.nearCitys.Count == 0)
        {
            return -100f;
        }
        float priority;
        priority = (float)(AIConfig.deffence * 
            (-Vector3.Distance(context.transform.position, context.GetNearestCity()) / AIConfig.checkRadius + AIConfig.checkRadius));
        return priority;
    }

    public float ReTreat()
    {
        if(context.playerArmy.Count == 0)return -100f;
        float priority;
        priority = AIConfig.move * ((context.playerAttack - context.enemyAttack) / 1000) % 100;
        return priority;
    }

    //暂定（可能改动为根据敌我城镇来移动）
    public float MoveForward()
    {
        //if(context.playerArmy.Count == 0)return 0f;
        float priority;
        priority = AIConfig.move * ((context.enemyAttack - context.playerAttack) / 1000) % 100;
        return priority;
    }

    public float Station()
    {
        if (context.playerArmy.Count == 0) return -100f;
        float priority;
        priority = (float)(AIConfig.move * (context.playerAttack - context.enemyAttack * 0.75) % 10 *
            (-Vector3.Distance(context.transform.position, context.GetNearestPlayerArmy().transform.position) / AIConfig.checkRadius + AIConfig.checkRadius));
        return priority;
    }

    public float StayInCity()
    {
        if (context.nearCitys.Count == 0) return -100;
        AIConfig.deffence = (-Vector3.Distance(context.transform.position, context.GetNearestCity()) / AIConfig.checkRadius + AIConfig.checkRadius);
        float priority;
        priority = AIConfig.deffence * ((context.enemyAttack - context.playerAttack) / 1000) % 100;
        return priority;
    }

    public float Attack()
    {
        if (context.playerArmy.Count == 0) return -100f;
        float priority;
        priority = AIConfig.attack * (context.enemyAttack - context.playerAttack) * 
            (ArmyDetail["velocity"] - context.GetNearestPlayerArmy().GetVelocity() / 2);
        return priority;
    }

    public float Healing()
    {
        if (context.nearCitys.Count == 0) return (1 - ArmyDetail["people"] / ArmyDetail["peopleLimit"]) *100;
        float priority;
        priority = ((1 - ArmyDetail["people"] / ArmyDetail["peopleLimit"])) * 100 + 
            Vector3.Distance(context.GetNearestPlayerArmy().transform.position, context.transform.position) % 100;
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
        prioritys[(int)EnemyArmyBehavior.stayInCity] = StayInCity();
        prioritys[(int)EnemyArmyBehavior.attack] = Attack();
        prioritys[(int)EnemyArmyBehavior.healing] = Healing();
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
