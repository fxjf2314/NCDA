using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToOthers : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool canMove;
    private Transform target;
    private void Start()
    {
        agent=GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (canMove)
        {
            agent.SetDestination(target.position);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("111");
        if (target&& collider.gameObject == target.gameObject)
        {
            canMove = false;
            agent.SetDestination(agent.transform.position);
        }
    }
    public void Move(Transform target)
    {
        this.target=target;
        canMove=true;
    }
}
