using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    protected int people;

    protected int strength;

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
}
