using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    float heightOffset;

    [SerializeField]
    float forcedMarchSpeed;

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.CompareTag("Player"))
        {
            //Debug.Log("player");
            other.transform.GetComponent<Outline>().enabled = true;
            CardManager.MyInstance.armies.Add(other.transform.GetComponent<MyArmy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            CardManager.MyInstance.armies.Remove(other.transform.GetComponent<MyArmy>());
            other.transform.GetComponent<Outline>().enabled = false;

        }
    }

    

    void OnEnable()
    {
        StartCoroutine(WaitForConfirm());
        StartCoroutine(StartFollowMouse());
    }

    void OnDisable()
    {
        StopCoroutine(WaitForConfirm());
        StopCoroutine(StartFollowMouse());
    }

    IEnumerator WaitForConfirm()
    {
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                foreach(Army army in CardManager.MyInstance.armies)
                {
                    army.ControlResource(Time.deltaTime,"velocity", forcedMarchSpeed, Time.deltaTime);
                    
                }
                ForcedMarchChoose.MyInstance.Hide();
               
            }
            yield return null;
        }
        
    }

    IEnumerator StartFollowMouse()
    {
        while(true)
        {
            // 获取鼠标在屏幕上的位置
            Vector3 mousePosition = Input.mousePosition;

            // 将鼠标位置转换为世界坐标
            Ray ray = cam.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            // 假设地面在Y=0的位置
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // 设置物体的位置，保持高度不变
                Vector3 targetPosition = hit.point;
                targetPosition.y = heightOffset;
                transform.position = targetPosition;
            }
            yield return null;
        }
    }
}
