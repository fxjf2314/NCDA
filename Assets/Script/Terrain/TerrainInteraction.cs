using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TerrainInteraction : MonoBehaviour
{
    private Army army;
    private string thisTerrain;
    private string lastTerrain;

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
        if (!other.CompareTag("Player")|| !other.CompareTag("Enemy"))
        {
            lastTerrain = thisTerrain;
            thisTerrain = GetTerrain(other);
            switch (lastTerrain)
            {
                case "平原":
                    plain.Exit(gameObject);
                    break;
                case "河流":
                    river.Exit(gameObject);
                    break;
                case "隘口":
                    defile.Exit(gameObject);
                    break;
                case "丘陵":
                    hilly.Exit(gameObject);
                    break;
                case "城镇":
                    town.Exit(gameObject);
                    break;
            }
            Debug.Log("进入" + thisTerrain);
            switch (thisTerrain)
            {
                case "平原":
                    plain.Use(gameObject);
                    break;
                case "河流":
                    river.Use(gameObject);
                    break;
                case "隘口":
                    defile.Use(gameObject);
                    break;
                case "丘陵":
                    hilly.Use(gameObject);
                    break;
                case "城镇":
                    town.Use(gameObject);
                    break;
            }
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
