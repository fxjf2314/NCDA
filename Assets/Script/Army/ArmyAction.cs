using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArmyAction : MonoBehaviour
{
    public bool isStill;
    private bool isTBD;
    private Vector3 thisPosition;
    private Vector3 lastPosition;
    private Army army;
    private void Start()
    {
        army=GetComponent<Army>();
        isStill = true;
    }
    private void Update()
    {
        lastPosition = thisPosition;
        thisPosition = transform.position;
        if (thisPosition == lastPosition) 
        {
            isStill=true;
            if (!isTBD)
            {
                //Army.ControlResource(duration, army.ArmyDetail["strength"], 默认值-armyDetail.ArmyDetail["strength"], intervalTime);
                isTBD = true;
            }
            //else if (army.ArmyDetail["strength"]== 默认值)
            //    isTBD=false;
        }
        else
            isStill = false;
    }
}
