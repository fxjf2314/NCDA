using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public int people;
    public int strength;

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
}
