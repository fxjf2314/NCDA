using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TerrainInteraction : MonoBehaviour
{
    private Army army;
    // 当前激活的地形效果
    private string activeTerrain = "";

    // 记录角色当前所在的所有地形区域
    private List<string> overlappingTerrains = new List<string>();

    public float exNum_Attack;
    public float exNum_Def;

    [Header("平原")]
    public Terrain plain;
    [Header("河流")]
    public Terrain river;
    [Header("隘口")]
    public Terrain defile;
    [Header("丘陵")]
    public Terrain hilly;
    [Header("城镇")]
    public Terrain town;


    private void Start()
    {
        army=GetComponent<Army>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Army"))
        {
            string newTerrain = GetTerrain(other);

            // 添加新地形到重叠列表
            if (!overlappingTerrains.Contains(newTerrain))
            {
                overlappingTerrains.Add(newTerrain);
            }

            // 更新激活地形
            UpdateActiveTerrain();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Army"))
        {
            string exitedTerrain = GetTerrain(other);

            // 从重叠列表中移除地形
            overlappingTerrains.Remove(exitedTerrain);

            // 更新激活地形
            UpdateActiveTerrain();
        }
    }

    // 更新当前激活的地形
    private void UpdateActiveTerrain()
    {
        // 确定新的激活地形（使用最后进入的地形）
        string newActive = overlappingTerrains.Count > 0
            ? overlappingTerrains[overlappingTerrains.Count - 1]
            : "";

        // 如果激活地形发生变化
        if (newActive != activeTerrain)
        {
            // 退出旧地形
            if (!string.IsNullOrEmpty(activeTerrain))
            {
                //Debug.Log($"离开地形: {activeTerrain}");
                ExitTerrain(activeTerrain);
            }

            // 进入新地形
            if (!string.IsNullOrEmpty(newActive))
            {
                //Debug.Log($"进入地形: {newActive}");
                if (activeTerrain == "城镇")
                {
                    //town.changeVaule[0].active = thisTown.GetComponent<Towns>().occupied;
                }
                EnterTerrain(newActive);
            }

            // 更新激活地形
            activeTerrain = newActive;
        }
    }

    private void ExitTerrain(string terrain)
    {
        switch (terrain)
        {
            case "平原": plain.Exit(gameObject); break;
            case "河流": river.Exit(gameObject); break;
            case "隘口": defile.Exit(gameObject); break;
            case "丘陵": hilly.Exit(gameObject); break;
            case "城镇": town.Exit(gameObject); break;
        }
    }

    // 地形进入处理
    private void EnterTerrain(string terrain)
    {
        switch (terrain)
        {
            case "平原": plain.Use(gameObject); break;
            case "河流": river.Use(gameObject); break;
            case "隘口": defile.Use(gameObject); break;
            case "丘陵": hilly.Use(gameObject); break;
            case "城镇": town.Use(gameObject); break;
        }
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
