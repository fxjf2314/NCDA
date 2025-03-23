using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    [SerializeField]
    SerializableDictionaryBase<string, float> dic = new SerializableDictionaryBase<string, float>();
    public Dictionary<string, float> ArmyDetail = new Dictionary<string, float>
    {
        {"people",0 },
        { "strenth",0},
        { "velocity",0}
    };
    
    //用于控制军队人数
    public void PeopleControl(float num)
    {
        ArmyDetail["people"] += num;
    }

    //用于控制军队实力
    public void StrengthControl(float num)
    {
        ArmyDetail["strenth"] += num;
    }

    public float GetPeople()
    {
        return ArmyDetail["people"];
    }

    public float GetStrength()
    {
        return ArmyDetail["strenth"];
    }

    public float GetVelocity()
    {
        return ArmyDetail["velocity"];
    }

    public void VelocityControl(float num)
    {
        ArmyDetail["velocity"] += num;
    }

    /*public IEnumerator SlowControl(float duration,)
    {
        yield return new WaitForSeconds(strength);
    }*/

}
