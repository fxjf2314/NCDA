using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayOutADefense : MonoBehaviour
{
    public Button DdButton;
    public Button FbButton;
    public Button BfButton;
    public Button HbButton;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float strength;
    private static LayOutADefense instance;
    #region 单例
    public static LayOutADefense Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LayOutADefense>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("LayOutADefense");
                    instance = singletonObject.AddComponent<LayOutADefense>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private void Start()
    {
        BfButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        BfButton.onClick.AddListener(() => SetButton(GetStateFromA(BfButton.gameObject.GetComponent<Image>().color)));
    }
    public void LayOut()
    {
        StartCoroutine(I_LayOut());
    }
    IEnumerator I_LayOut()
    {
        float elapsedTime = 0.0f;
        GameObject selectedArmy = SelectArmy.Instance.SelectedArmy;
        ArmyAction ArmyAction = selectedArmy.GetComponent<ArmyAction>();
        Army army = selectedArmy.GetComponent<Army>();
        Image buttonImage = BfButton.gameObject.GetComponent<Image>();

        while (elapsedTime < duration)
        {
            ArmyAction.isArming = true;
            yield return null;
            if (!GetStateFromA(buttonImage.color))
            {
                Debug.Log("布防终止：军队移动或按钮状态变化");
                Debug.Log(ArmyAction.isStill);
                Debug.Log(buttonImage.color);
                ArmyAction.isArming = false;
                SetButton(true);
                yield break;
            }
            elapsedTime += Time.deltaTime;
        }
        army.ControlResource(0.1f, "strength", strength, 0.1f);
        Debug.Log("布防完成，力量增加");
    }
    public bool GetStateFromA(Color color)
    {
        if (color.a == 0.5f)
            return false;
        else if (color.a == 1f)
            return true;
        else return false;
    }
    public void SetButton(bool state)
    {
        if (!state)
        {
            BfButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            BfButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            SelectArmy.Instance.canSelect = true;
        }
        FbButton.interactable = state;
        HbButton.interactable = state;
        DdButton.interactable = state;
    }
}
