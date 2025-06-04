using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Detection : MonoBehaviour
{
    Collider detectionRange;

    private void Start()
    {
        detectionRange = GetComponent<Collider>();
        detectionRange.enabled = false;
    }

    public void StartDetec()
    {
        detectionRange.enabled = !detectionRange.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Army"))
        {
            GetComponentInParent<AmBushDetec>().HasFindEnermy(other.gameObject);
        }
        
    }
}
