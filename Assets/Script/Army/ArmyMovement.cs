using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System;

public class ArmyMovement : MonoBehaviour
{
    public GameObject movePanel;
    private Button mYesButton;
    private Button mNoButton;
    public GameObject attackPanel;
    private Button aYesButton;
    private Button aNoButton;
    private NavMeshAgent agent; // 导航代理
    private NavMeshPath path;
    private bool canMove;
    public MoveToOthers moveToOthers;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mYesButton = movePanel.transform.Find("YesButton").GetComponent<Button>();
        mNoButton = movePanel.transform.Find("NoButton").GetComponent<Button>();
        mYesButton.onClick.AddListener(ArmyMove);
        mNoButton.onClick.AddListener(CancelMove);
        aYesButton = attackPanel.transform.Find("YesButton").GetComponent<Button>();
        aNoButton = attackPanel.transform.Find("NoButton").GetComponent<Button>();
        aNoButton.onClick.AddListener(CancelMove);
        path = new NavMeshPath();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标点击
        {
            Action action;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)&&!hit.transform.gameObject.CompareTag("Player")&&!EventSystem.current.IsPointerOverGameObject()) // 检测点击位置
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Army":
                        aYesButton.onClick.AddListener(()=>moveToOthers.Move(hit.transform));
                        action = () =>
                        {
                            attackPanel.SetActive(true);
                        };
                        break;
                    case "Town":
                        aYesButton.onClick.AddListener(ArmyMove);
                        action = () =>
                        {
                            attackPanel.SetActive(true);
                        };
                        break ;
                    default:
                        action = () =>
                        {
                            movePanel.SetActive(true);
                        };
                        break ;
                }
                NavMeshHit navHit;
                // 将点击位置转换为导航网格上的点
                if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
                {
                    // 计算路径
                    bool pathFound = NavMesh.CalculatePath(agent.transform.position, navHit.position, NavMesh.AllAreas, path);

                    if (pathFound && path.status == NavMeshPathStatus.PathComplete)
                    {
                        // 存储经过的区域
                        HashSet<int> areaIDs = new HashSet<int>();

                        // 遍历路径拐点
                        for (int i = 0; i < path.corners.Length - 1; i++)
                        {
                            Vector3 start = path.corners[i];
                            Vector3 end = path.corners[i + 1];

                            // 分段采样路径
                            float segmentLength = Vector3.Distance(start, end);
                            int samples = Mathf.CeilToInt(segmentLength / 0.1f); // 每 0.1 米采样一次
                            for (int j = 0; j <= samples; j++)
                            {
                                Vector3 samplePoint = Vector3.Lerp(start, end, (float)j / samples);
                                samplePoint= PointToGrd(samplePoint);
                                NavMeshHit areaHit;
                                // 检测采样点所在的导航区域
                                if (NavMesh.SamplePosition(samplePoint, out areaHit, 4f, NavMesh.AllAreas))
                                {
                                    int areaID = GetAreaIDFromMask(areaHit.mask); // 获取区域掩码
                                    areaIDs.Add(areaID); // 添加到集合中

                                    // 获取区域名称
                                    string areaName = GetAreaNameFromID(areaID);
                                    Debug.Log($"采样点 {samplePoint} 所在区域: {areaName}");
                                }
                            }
                        }
                        // 输出所有经过的区域
                        Debug.Log("路径将经过的区域:");
                        foreach (int id in areaIDs)
                        {
                            Debug.Log(GetAreaNameFromID(id));
                        }
                        action.Invoke();
                        canMove = true;
                    }
                    else
                    {
                        Debug.LogWarning("无法找到完整路径。");
                    }
                }
            }
        }
    }
    private void ArmyMove()
    {
        if(canMove )
            agent.SetPath(path);
    }
    private void CancelMove()
    {
        canMove = false;
    }
    #region 将点的坐标换到地面上
    private Vector3 PointToGrd(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        Vector3 newPoint= point;
        // 检测射线是否击中物体 射线的最大检测距离100f
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // 检查是否点击在地面上
            if (hit.transform.CompareTag("Ground"))
            {
                Vector3 intersectionPoint = hit.point;
                newPoint = intersectionPoint;
            }
        }
        return newPoint;
    }
    #endregion

    #region 根据区域 ID 获取区域名称
    private string GetAreaNameFromID(int areaID)
    {
        // 定义区域名称和 ID 的映射
        switch (areaID)
        {
            case 0: return "Walkable"; // 默认区域
            case 1: return "Not Walkable"; // 不可行走区域
            case 2: return "Jump"; // 跳跃区域
            case 3: return "River";
            default: return "Unknown";
        }
    }
    private int GetAreaIDFromMask(int areaMask)
    {
        // 将区域掩码转换为区域 ID
        for (int i = 0; i < 32; i++)
        {
            if ((areaMask & (1 << i)) != 0)
            {
                return i;
            }
        }
        return -1; // 未找到
    }
    #endregion
}