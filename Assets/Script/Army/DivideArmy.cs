using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DivideArmy : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private TextMeshProUGUI valueText;
    [SerializeField]
    private Button fbYesButton;
    private GameObject thisArmy;
    private void Start()
    {
        valueText= slider.gameObject.transform.Find("Handle Slide Area/Handle/Text").GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        fbYesButton.onClick.AddListener(DivideTheArmy);
    }
    private void OnEnable()
    {
        StartCoroutine(SetHandle());
    }
    #region 滑条数值显示
    private void OnSliderValueChanged(float value)
    {
        UpdateValueText();
    }

    private void UpdateValueText()
    {
        thisArmy = SelectArmy.Instance.SelectedArmy;
        valueText.text = (Convert.ToInt16(slider.value * thisArmy.GetComponent<MyArmy>().ArmyDetail["people"])).ToString(); 
    }
    public void ResetHandle()
    {
        StartCoroutine(SetHandle());
    }
    private IEnumerator SetHandle()
    {
        yield return null;
        slider.value = 0;
    }
    #endregion
    private void DivideTheArmy()
    {

        if (slider.value != 0 && slider.value != 1) 
        {
            SelectArmy.Instance.DeselectTheArmy();
            thisArmy = SelectArmy.Instance.SelectedArmy;
            Vector3 randomDirection = new Vector3(Random.Range(-0.1f, 0.1f), 0f, Random.Range(-0.1f, 0.1f)).normalized;
            randomDirection = thisArmy.transform.position + randomDirection;
            GameObject newArmy = GameObject.Instantiate(thisArmy, randomDirection, Quaternion.identity);
            //CopyScripts(thisArmy, newArmy);
            NavMeshAgent agent = newArmy.GetComponent<NavMeshAgent>();
            newArmy.GetComponent<MyArmy>().ControlResource(0f, "people", Convert.ToInt16(slider.value * thisArmy.GetComponent<MyArmy>().ArmyDetail["people"]) - newArmy.GetComponent<MyArmy>().ArmyDetail["people"], 0f);
            //newArmy.GetComponent<MyArmy>().ArmyDetail["people"] = Convert.ToInt16(slider.value * thisArmy.GetComponent<MyArmy>().ArmyDetail["people"]);
            Debug.Log(Convert.ToInt16(slider.value * thisArmy.GetComponent<MyArmy>().ArmyDetail["people"]));
            thisArmy.GetComponent<MyArmy>().ControlResource(0f, "people", -newArmy.GetComponent<MyArmy>().ArmyDetail["people"], 0f);
            ArmyManager.MyInstance.allArmies.Add(newArmy);
            ArmyManager.MyInstance.otherArmies.Add(newArmy);
        }

    }
    
}
