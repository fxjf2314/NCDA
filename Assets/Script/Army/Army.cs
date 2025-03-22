using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    protected int people;

    protected int strength;

    protected float velocity;

    //用于控制军队人数
    public void PeopleControl(int num)
    {
        people += num;
    }

    //用于控制军队实力
    public void StrengthControl(int num)
    {
        strength += num;
    }

    public void VelocityControl(float num)
    {
        velocity += num;
    }
}
