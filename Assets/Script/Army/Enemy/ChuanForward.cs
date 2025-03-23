using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ChuanForward : MonoBehaviour
{
    public Transform target;
    NavMeshPath path;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
        path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
    }

    //void OnDrawGizmos()
    //{
    //    if (path.status != NavMeshPathStatus.PathInvalid)
    //    {
    //        Gizmos.color = Color.red;
    //        for (int i = 0; i < path.corners.Length; i++)
    //        {
    //            if (i > 0)
    //                Gizmos.DrawLine(path.corners[i - 1], path.corners[i]);
    //            Gizmos.DrawSphere(path.corners[i], 0.1f);
    //        }
    //    }
    //}

    private void Update()
    {
        
    }
}
