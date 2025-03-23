using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TerrainInteraction : MonoBehaviour
{
    private string thisTerrain;
    private string lastTerrain;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("进入"+GetTerrain(other));
    }
    private string GetTerrain(Collider other)
    {
        if (other.GetComponent<NavMeshModifierVolume>())
            return GetAreaNameFromID(other.GetComponent<NavMeshModifierVolume>().area);
        else return null;
    }
    private string GetAreaNameFromID(int areaID)
    {
        // 定义区域名称和 ID 的映射
        switch (areaID)
        {
            case 0: return "Walkable"; // 默认区域
            case 1: return "Not Walkable"; // 不可行走区域
            case 2: return "Jump"; // 跳跃区域
            case 3: return "平原";
            case 4: return "河流";
            case 5: return "隘口";
            case 6: return "丘陵";
            case 7: return "城镇";
            default: return "Unknown";
        }
    }
}
