using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public int people;
    public int strength;

    //���ڿ��ƾ�������
    public void PeopleControl(int num)
    {
        people += num;
    }

    //���ڿ��ƾ���ʵ��
    public void StrengthControl(int num)
    {
        strength += num;
    }
}
