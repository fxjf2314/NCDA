using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    protected int people;

    protected int strength;

    //���ڿ��ƾ�������
    protected void PeopleControl(int num)
    {
        people += num;
    }

    //���ڿ��ƾ���ʵ��
    protected void StrengthControl(int num)
    {
        strength += num;
    }
}
