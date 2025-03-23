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
}
