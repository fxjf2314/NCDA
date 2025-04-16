using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ArmyAction : MonoBehaviour
{
    [SerializeField]
    private bool isStill;
    [SerializeField]
    private bool isTBD;
    public bool isArming;
    private bool armOver;
    private Vector3 thisPosition;
    private Vector3 lastPosition;
    private Army army;
    private void Start()
    {
        army = GetComponent<Army>();
        isStill = true;
        isArming= false;
        armOver = false;
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
        {
            isStill = false;
            isArming = false;
            if(armOver)
            {
                Debug.Log("取消布防");
                army.ControlResource(0f, "strength", -LayOutADefense.Instance.Strength, 0f);
                armOver = false;
            }
        } 
    }
    public void LayOut()
    {
        if (I_LayOut() != null)
        {
            StopCoroutine(I_LayOut());
        }
        StartCoroutine(I_LayOut());
    }
    public IEnumerator I_LayOut()
    {
        Debug.Log("开始布防");
        isArming = true;
        float elapsedTime = 0.0f;
        Image buttonImage = LayOutADefense.Instance.BfButton.gameObject.GetComponent<Image>();
        while (elapsedTime < LayOutADefense.Instance.Duration)
        {
            yield return null;
            if (SelectArmy.Instance.SelectedArmy==gameObject&&!LayOutADefense.Instance.GetStateFromA(buttonImage.color))
            {
                Debug.Log("中断布防");
                isArming = false;
                LayOutADefense.Instance.SetButton(true);
                if (armOver)
                {
                    Debug.Log("取消布防");
                    army.ControlResource(0f, "strength", -LayOutADefense.Instance.Strength, 0f);
                    armOver = false;
                }
                yield break;
            }
            elapsedTime += Time.deltaTime;
            Debug.Log("正在布防");
        }
        army.ControlResource(0f, "strength", LayOutADefense.Instance.Strength, 0f);
        Debug.Log("布防成功" + name + army.ArmyDetail["strength"]);
        armOver = true;
        yield return null;
    }
}
