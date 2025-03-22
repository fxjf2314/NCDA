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
        UpdateValueText();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        fbYesButton.onClick.AddListener(DivideTheArmy);
    }
    #region 滑条数值显示
    private void OnSliderValueChanged(float value)
    {
        UpdateValueText();
    }

    private void UpdateValueText()
    {
        thisArmy = SelectArmy.Instance.SelectedArmy;
        valueText.text = (Convert.ToInt16(slider.value * thisArmy.GetComponent<Army>().people)).ToString(); 
    }
    public void ResetHandle()
    {
        slider.value = 0;
    }
    #endregion
    private void DivideTheArmy()
    {
        thisArmy = SelectArmy.Instance.SelectedArmy;
        Vector3 randomDirection = new Vector3(Random.Range(-0.1f, 0.1f), 0f, Random.Range(-0.1f, 0.1f)).normalized;
        randomDirection = thisArmy.transform.position + randomDirection;
        GameObject newArmy = GameObject.Instantiate(thisArmy, randomDirection, Quaternion.identity);
        //CopyScripts(thisArmy, newArmy);
        NavMeshAgent agent = newArmy.GetComponent<NavMeshAgent>();
        newArmy.GetComponent<Army>().people =Convert.ToInt16(slider.value* thisArmy.GetComponent<Army>().people);
        thisArmy.GetComponent<Army>().PeopleControl(- newArmy.GetComponent<Army>().people);
    }
    //#region 复制脚本
    //void CopyScripts(GameObject oldObject, GameObject newObject)
    //{
    //    // 获取旧物体上的所有 MonoBehaviour 脚本
    //    MonoBehaviour[] oldScripts = oldObject.GetComponents<MonoBehaviour>();

    //    // 遍历旧物体上的每个脚本
    //    foreach (MonoBehaviour oldScript in oldScripts)
    //    {
    //        // 获取脚本的类型
    //        System.Type scriptType = oldScript.GetType();

    //        // 在新物体上添加相同类型的脚本
    //        MonoBehaviour newScript = newObject.AddComponent(scriptType) as MonoBehaviour;

    //        // 复制旧脚本的属性到新脚本
    //        System.Reflection.PropertyInfo[] properties = scriptType.GetProperties();
    //        foreach (System.Reflection.PropertyInfo property in properties)
    //        {
    //            if (property.CanRead && property.CanWrite)
    //            {
    //                try
    //                {
    //                    object value = property.GetValue(oldScript, null);
    //                    property.SetValue(newScript, value, null);
    //                }
    //                catch
    //                {
    //                    // 忽略无法复制的属性
    //                }
    //            }
    //        }

    //        // 复制旧脚本的字段到新脚本
    //        System.Reflection.FieldInfo[] fields = scriptType.GetFields();
    //        foreach (System.Reflection.FieldInfo field in fields)
    //        {
    //            try
    //            {
    //                object value = field.GetValue(oldScript);
    //                field.SetValue(newScript, value);
    //            }
    //            catch
    //            {
    //                // 忽略无法复制的字段
    //            }
    //        }
    //    }
    //}
    //#endregion
}
