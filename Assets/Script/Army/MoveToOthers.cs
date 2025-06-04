using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToOthers : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private bool canMove;
    private Transform target;

    public bool CanMove { get => canMove; set => canMove = value; }

    private void Start()
    {
        agent=GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (CanMove)
        {
            agent.SetDestination(target.position);
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (target&& collider.gameObject == target.gameObject)
        {
            CanMove = false;
            agent.SetDestination(agent.transform.position);
        }
    }
    public void Move(Transform target)
    {
        if (gameObject==SelectArmy.Instance.SelectedArmy)
        {
            this.target = target;
            CanMove = true;
        }
    }
}
