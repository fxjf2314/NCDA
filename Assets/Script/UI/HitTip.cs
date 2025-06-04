using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HitTip : MonoBehaviour
{
    public GameObject tipback;
    public TextMeshProUGUI tip;
    public List<HitTip> hits;

    void Start()
    {

    }


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (EventSystem.current.IsPointerOverGameObject())//ºÏ≤‚UI
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit " + hit.collider.gameObject.name);
                
            }
        }
        else if(Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())//ºÏ≤‚ŒÔÃÂ
        {

        }
    }
}
