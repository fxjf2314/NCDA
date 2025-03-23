using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    protected int people;

    protected int strength;

    protected float speed;

    //用于控制军队人数
    protected void PeopleControl(int num)
    {
        people += num;
    }

    //用于控制军队实力
    protected void StrengthControl(int num)
    {
        strength += num;
    }

    protected void SpeedControl(float num)
    {
        speed += num;
    }

    public int GetPeople()
    {
        return people;
    }

    public int GetStrength()
    {
        return strength;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
