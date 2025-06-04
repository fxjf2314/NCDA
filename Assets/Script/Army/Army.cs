using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Unity.VisualScripting;
using UnityEngine.AI;

public class Army : MonoBehaviour
{

    public SerializableDictionaryBase<string, float> ArmyDetail = new SerializableDictionaryBase<string, float>
    {
        {"people",0 },
        { "strength",0},
        { "velocity",0}
    };

    float initNum;


    public float GetPeople()
    {
        return ArmyDetail["people"];
    }

    public float GetStrength()
    {
        return ArmyDetail["strength"];
    }

    public float GetVelocity()
    {
        return ArmyDetail["velocity"];
    }

    public Coroutine ControlResource(float duration, string name, float deltaNum, float intervalTime)
    {
        Coroutine task = StartCoroutine(AllControl(duration, name, deltaNum, intervalTime));
        return task;
    }

    private IEnumerator AllControl(float duration, string name, float deltaNum, float intervalTime)
    {
        initNum = ArmyDetail[name];

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            ArmyDetail[name] += deltaNum * (intervalTime / duration);
            yield return new WaitForSeconds(intervalTime);
            elapsedTime += intervalTime;

        }


        if (ArmyDetail["people"] == 0)
            Destroy(gameObject);
    }

    public void BeAttacked(float Num)
    {
        float extraNum = gameObject.GetComponent<TerrainInteraction>().exNum_Attack;
        StartCoroutine(AllControl(0.1f, "people", -(Num + extraNum), 0.1f));
    }

    public void CoverToDefaultStrength(float defaultNum)
    {
        StartCoroutine(AllControl(3f, "strength", (defaultNum - ArmyDetail["strength"]), 0.4f));
    }
}
