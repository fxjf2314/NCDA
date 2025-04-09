using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmBushDetec : MonoBehaviour
{
   

    public void HasFindEnermy(GameObject enermy)
    {
        transform.GetComponent<MoveToOthers>().Move(enermy.transform);
        GetComponentInChildren<Detection>().StartDetec();
    }
}
