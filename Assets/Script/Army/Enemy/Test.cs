using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public Transform target; // 目标位置的Transform组件
    private NavMeshPath path;

    void Start()
    {
        path = new NavMeshPath();
        //NavMeshHit hit1;
        //NavMesh.SamplePosition(target.position, out hit1, 10.0f, NavMesh.AllAreas);
        //NavMesh.CalculatePath(transform.position + hit2.position, target.position + hit1.position, NavMesh.AllAreas, path);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.CalculatePath(target.position, path);
        agent.SetDestination(target.position);

    }

    private void Update()
    {
        //Debug.Log(path.corners.Length);
        if (path.status != NavMeshPathStatus.PathInvalid)
        {
            
            for (int i = 0; i < path.corners.Length; i++)
            {
                if (i > 0)
                    Debug.DrawLine(path.corners[i - 1], path.corners[i], Color.red);

            }
        }
    }
}
