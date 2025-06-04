using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GeometryUtils
{
    // 计算点P关于线AB的对称点P'
    public static Vector3 ReflectPointAcrossLine(Vector3 a, Vector3 b, Vector3 p)
    {
        //// 计算向量AB和AP
        //Vector3 ab = b - a;
        //Vector3 ap = p - a;

        //// 计算AP在AB上的投影长度
        //float ab2 = Vector3.Dot(ab, ab);
        //float ap_ab = Vector3.Dot(ap, ab);
        //float t = ap_ab / ab2;

        //// 计算投影点Q
        //Vector3 q = a + t * ab;

        //// 计算向量AQ
        //Vector3 aq = q - a;

        //// 计算对称点P'
        //Vector3 pp = a - aq;

        //return pp;

        // 计算向量AB和AP
        Vector3 ab = b - a;
        Vector3 ap = p - a;

        // 计算投影系数t
        float t = Vector3.Dot(ap, ab) / ab.sqrMagnitude;

        // 计算投影点Q
        Vector3 q = a + t * ab;

        // 计算对称点P' = Q + (Q - P)
        Vector3 pp = 2 * q - p;

        return pp;
    }

    //计算空间中一类物体的离散程度
    public static float CalculateDispersion(Transform[] targetObjects)
    {
        Vector3 meanPosition = Vector3.zero;
        foreach (Transform t in targetObjects)
            meanPosition += t.position;
        meanPosition /= targetObjects.Length;

        float totalDistance = 0;
        foreach (Transform t in targetObjects)
            totalDistance += Vector3.Distance(t.position, meanPosition);

        return totalDistance / targetObjects.Length;
    }

    // 计算PCA并分类
    public static Vector3 CalculateDynamicBoundary(Transform objectA, Transform[] surroundingObjects)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (Transform obj in surroundingObjects)
        {
            positions.Add(obj.position - objectA.position);
        }

        // 计算协方差矩阵
        Vector3 mean = Vector3.zero;
        foreach (Vector3 pos in positions) mean += pos;
        mean /= positions.Count;

        Matrix4x4 covariance = Matrix4x4.zero;
        foreach (Vector3 pos in positions)
        {
            Vector3 diff = pos - mean;
            covariance.m00 += diff.x * diff.x;
            covariance.m01 += diff.x * diff.y;
            covariance.m02 += diff.x * diff.z;
            covariance.m11 += diff.y * diff.y;
            covariance.m12 += diff.y * diff.z;
            covariance.m22 += diff.z * diff.z;
        }

        // 计算特征向量（主方向）
        Vector3 pcaDirection = CalculatePrincipalComponent(covariance);
        Vector3 boundaryDirection = Vector3.Cross(pcaDirection, Vector3.up).normalized;

        return boundaryDirection;
    }

    // 简化版PCA主方向计算
    private static Vector3 CalculatePrincipalComponent(Matrix4x4 covariance)
    {
        // 此处可替换为完整的特征值分解算法
        // 简化为假设最大方差在XZ平面
        return new Vector3(covariance.m02, 0, covariance.m22).normalized;
    }

    //计算撤退路径
    public static List<Vector3> CalculateReTreatPath(Transform enemy,params Army[] players)
    {
        Vector3 playerCenter = Vector3.zero;
        foreach(var player in players)
        {
            playerCenter += player.transform.position;
        }
        playerCenter /= players.Length;
        //考虑players为空的情况
        if(playerCenter == Vector3.zero)
        {
            foreach (var player in EnemyAIManager.Instance.allPlayerArmys)
            {
                playerCenter += player.transform.position;
            }
            playerCenter /= players.Length;
        }
        Vector3 dir = enemy.position - playerCenter;
        Vector3 finalPosition = enemy.position + dir;
        List<Vector3> points = new List<Vector3>();
        int pointCount = (int)MathF.Ceiling(Vector3.Distance(enemy.position, playerCenter)/ 10) + 1;//每10米一个点
        for (int i = 1; i <= pointCount; i++)
        {
            Vector3 temp = enemy.position + i * dir / pointCount;
            points.Add(temp);
        }

        return points;
    }

    // 计算点到直线的距离
    public static float PointToLineDistance(Vector2 point, Vector2 direction, Vector2 pointOnLine)
    {
        // 检查方向向量是否为零向量
        if (direction == Vector2.zero)
        {
            Debug.LogError("方向向量不能为零向量");
            return float.MaxValue;
        }

        // 计算从直线上点到目标点的向量
        Vector2 pointDiff = point - pointOnLine;

        // 直接计算二维叉积：a.x * b.y - a.y * b.x
        float crossProduct = Mathf.Abs(pointDiff.x * direction.y - pointDiff.y * direction.x);

        // 计算方向向量的长度
        float directionMagnitude = direction.magnitude;

        // 距离 = |叉积| / 方向向量长度
        return crossProduct / directionMagnitude;
    }

    public static void QuickSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            // 找到分区点
            int partitionIndex = Partition(array, left, right);

            // 递归排序左右两部分
            QuickSort(array, left, partitionIndex - 1);
            QuickSort(array, partitionIndex + 1, right);
        }
    }

    private static int Partition(int[] array, int left, int right)
    {
        // 选择最右边的元素作为基准值
        int pivot = array[right];
        int partitionIndex = left;

        // 遍历数组，将小于基准值的元素移到左边
        for (int i = left; i < right; i++)
        {
            if (array[i] < pivot)
            {
                Swap(ref array[i], ref array[partitionIndex]);
                partitionIndex++;
            }
        }

        // 将基准值移到分区点
        Swap(ref array[partitionIndex], ref array[right]);

        return partitionIndex;
    }

    private static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
}
