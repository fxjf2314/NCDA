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
    private NavMeshAgent agent; // ��������
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
        if (Input.GetMouseButtonDown(0)) // ��������
        {
            Action action;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)&&!hit.transform.gameObject.CompareTag("Player")&&!EventSystem.current.IsPointerOverGameObject()) // �����λ��
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
                // �����λ��ת��Ϊ���������ϵĵ�
                if (NavMesh.SamplePosition(hit.point, out navHit, 1.0f, NavMesh.AllAreas))
                {
                    // ����·��
                    bool pathFound = NavMesh.CalculatePath(agent.transform.position, navHit.position, NavMesh.AllAreas, path);

                    if (pathFound && path.status == NavMeshPathStatus.PathComplete)
                    {
                        // �洢����������
                        HashSet<int> areaIDs = new HashSet<int>();

                        // ����·���յ�
                        for (int i = 0; i < path.corners.Length - 1; i++)
                        {
                            Vector3 start = path.corners[i];
                            Vector3 end = path.corners[i + 1];

                            // �ֶβ���·��
                            float segmentLength = Vector3.Distance(start, end);
                            int samples = Mathf.CeilToInt(segmentLength / 0.1f); // ÿ 0.1 �ײ���һ��
                            for (int j = 0; j <= samples; j++)
                            {
                                Vector3 samplePoint = Vector3.Lerp(start, end, (float)j / samples);
                                samplePoint= PointToGrd(samplePoint);
                                NavMeshHit areaHit;
                                // �����������ڵĵ�������
                                if (NavMesh.SamplePosition(samplePoint, out areaHit, 4f, NavMesh.AllAreas))
                                {
                                    int areaID = GetAreaIDFromMask(areaHit.mask); // ��ȡ��������
                                    areaIDs.Add(areaID); // ��ӵ�������

                                    // ��ȡ��������
                                    string areaName = GetAreaNameFromID(areaID);
                                    Debug.Log($"������ {samplePoint} ��������: {areaName}");
                                }
                            }
                        }
                        // ������о���������
                        Debug.Log("·��������������:");
                        foreach (int id in areaIDs)
                        {
                            Debug.Log(GetAreaNameFromID(id));
                        }
                        action.Invoke();
                        canMove = true;
                    }
                    else
                    {
                        Debug.LogWarning("�޷��ҵ�����·����");
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
    #region ��������껻��������
    private Vector3 PointToGrd(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        Vector3 newPoint= point;
        // ��������Ƿ�������� ���ߵ���������100f
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // ����Ƿ����ڵ�����
            if (hit.transform.CompareTag("Ground"))
            {
                Vector3 intersectionPoint = hit.point;
                newPoint = intersectionPoint;
            }
        }
        return newPoint;
    }
    #endregion

    #region �������� ID ��ȡ��������
    private string GetAreaNameFromID(int areaID)
    {
        // �����������ƺ� ID ��ӳ��
        switch (areaID)
        {
            case 0: return "Walkable"; // Ĭ������
            case 1: return "Not Walkable"; // ������������
            case 2: return "Jump"; // ��Ծ����
            case 3: return "River";
            default: return "Unknown";
        }
    }
    private int GetAreaIDFromMask(int areaMask)
    {
        // ����������ת��Ϊ���� ID
        for (int i = 0; i < 32; i++)
        {
            if ((areaMask & (1 << i)) != 0)
            {
                return i;
            }
        }
        return -1; // δ�ҵ�
    }
    #endregion
}