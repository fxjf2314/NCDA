using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntegrateArmy : MonoBehaviour
{
    private bool canIntegrate;
    private GameObject thisArmy;
    private GameObject selectedArmy;
    private void Update()
    {
        if (canIntegrate)
        { 
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (hit.transform.gameObject != thisArmy && hit.transform.gameObject.CompareTag("Player"))
                    {
                        Debug.Log(111);
                        selectedArmy = hit.transform.gameObject;
                        thisArmy.GetComponent<Army>().people += selectedArmy.GetComponent<Army>().people;
                        Destroy(selectedArmy);
                        SelectArmy.Instance.canSelect = true;
                    }
                }
            }
        }
    }
    public void IntegrateTheArmy()
    {
        Debug.Log(111);
        SelectArmy.Instance.canSelect=false;
        thisArmy=SelectArmy.Instance.SelectedArmy;
        canIntegrate = true;
    }
}
