using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Unity.VisualScripting;

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

    public void ControlResource(float duration, string name, float deltaNum, float intervalTime)
    {
        StartCoroutine(AllControl(duration, name, deltaNum, intervalTime));
    }

    private IEnumerator AllControl(float duration, string name, float deltaNum,float intervalTime)
    {
        initNum = ArmyDetail[name];

        float elapsedTime = 0.0f;
        float targetNum = initNum + deltaNum;

        while (elapsedTime < duration)
        {
            ArmyDetail[name] = Mathf.Lerp(initNum, targetNum, elapsedTime / duration);
            yield return new WaitForSeconds(intervalTime);
            elapsedTime += intervalTime;
            
        }

        ArmyDetail[name] = targetNum;
        if (ArmyDetail["people"] == 0)
            Destroy(gameObject);
    }
    public void BeAttacked(float Num)
    {
        float extraNum = gameObject.GetComponent<TerrainInteraction>().exNum_Attack;
        StartCoroutine(AllControl(0.1f, "people",- ( Num + extraNum ), 0.1f));
    }
}
