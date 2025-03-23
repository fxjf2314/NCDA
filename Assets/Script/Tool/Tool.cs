using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public static class Tool
{
    public static Transform[] TurnIntoTransform(MonoBehaviour[] needToChange)
    {
        Transform[] transforms = new Transform[needToChange.Length];
        for (int i = 0; i < needToChange.Length; i++)
        {
            transforms[i] = needToChange[i].transform;
        }
        return transforms;
    }

    public static Vector3[] TurnIntoPosition(MonoBehaviour[] needToChange)
    {
        Vector3[] vecotor3s = new Vector3[needToChange.Length];
        for (int i = 0; i < needToChange.Length; i++)
        {
            vecotor3s[i] = needToChange[i].transform.position;
        }
        return vecotor3s;
    }

    public static T[] TurnIntoComponent<T>(MonoBehaviour[] needToChange)
    {
        T[] Ts = new T[needToChange.Length];
        for (int i = 0; i < needToChange.Length; i++)
        {
            Ts[i] = needToChange[i].transform.GetComponent<T>();
        }
        return Ts;
    }

    public static float CaculatePathCost(Transform target, NavMeshAgent agent)
    {
        float totalCost = -1;
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(target.position, path))
        {
            totalCost = 0;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Vector3 start = path.corners[i];
                Vector3 end = path.corners[i + 1];
                float segmentLength = Vector3.Distance(start, end);

                // 采样起点和终点的区域，取平均值（可根据需要增加采样点）
                NavMeshHit hitStart, hitEnd;
                NavMesh.SamplePosition(start, out hitStart, 0.1f, NavMesh.AllAreas);
                NavMesh.SamplePosition(end, out hitEnd, 0.1f, NavMesh.AllAreas);

                // 计算平均Cost（简单处理，实际可能需要更精确的分段计算）
                float costStart = NavMesh.GetAreaCost(hitStart.mask);
                float costEnd = NavMesh.GetAreaCost(hitEnd.mask);
                float averageCost = (costStart + costEnd) / 2f;

                totalCost += segmentLength * averageCost;
            }
            Debug.Log("Total Path Cost: " + totalCost);
        }
        return totalCost;
    }
}
