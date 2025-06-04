using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyArmy : Army
{
    public bool isForcedMarch;

    public bool isCommission;

    public float defaultStrength;

    private float forceMarchDuration = 2.0f;

    private void Update()
    {
        //Debug.Log(GetComponent<NavMeshAgent>().speed);
        if (GetComponent<NavMeshAgent>())
        {
            GetComponent<NavMeshAgent>().speed = ArmyDetail["velocity"] / 10;
        }
    }

    public void StartForceMarch()
    {
        if (isForcedMarch)
        {
            Invoke("ForcedMarchStatus", 3.0f);
        }

        //Debug.Log(RangeManager.MyInstance.forcedMarchStrength);
    }



    private void ForcedMarchStatus()
    {
        StartCoroutine(WaitForStatusFinish());
        ControlResource(0.1f, "velocity", -1, 0.1f);
        ControlResource(2.0f, "strength", CardManager.MyInstance.forcedMarchStrength, 0.5f);
        ControlResource(0.01f, "strength", -CardManager.MyInstance.forcedMarchStrength, 0.01f);

    }

    IEnumerator WaitForStatusFinish()
    {

        yield return new WaitForSeconds(forceMarchDuration);
        isForcedMarch = false;
    }

}
