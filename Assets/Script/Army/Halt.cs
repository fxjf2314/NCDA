using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Halt : MonoBehaviour
{
    public void TBD()
    {
        GameObject army = SelectArmy.Instance.SelectedArmy;
        NavMeshAgent agent = army.GetComponent<NavMeshAgent>();
        ArmyMovement armyMovement = army.GetComponent<ArmyMovement>();
        armyMovement.moveToOthers.CanMove = false;
        agent.SetDestination(agent.transform.position);
    }
}
