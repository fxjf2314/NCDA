using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TerrainInteraction : MonoBehaviour
{
    private Army army;
    private string thisTerrain;
    private string lastTerrain;
    [Header("平原")]
    [SerializeField]
    private float extraNum;//额外受攻击的数值
    public float exNum;
    [Header("河流")]
    [SerializeField]
    private float duration;
    [SerializeField]
    private float r_VReduction;
    [SerializeField]
    private float intervalTime;
    [Header("丘陵")]
    private float d_VReduction;


    private void Start()
    {
        army=GetComponent<Army>();
    }
    private void OnTriggerEnter(Collider other)
    {
        lastTerrain = thisTerrain;
        thisTerrain = GetTerrain(other);
        Debug.Log("进入"+ thisTerrain);
        Reset();
        switch (thisTerrain)
        {
            case "平原":
                exNum = extraNum;
                break;
            case "河流":
                army.ControlResource(duration, "velocity", -r_VReduction, intervalTime);
                break;
            case "隘口":
                
                break;
            case "丘陵":
                army.ControlResource(0.1f, "velocity", d_VReduction, 0.1f);
                break;
            case "城镇":
                break;
        }
    }
    private void Reset()
    {
        exNum = 0;
        army.ControlResource(0.1f, "velocity", -d_VReduction, 0.1f);
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
