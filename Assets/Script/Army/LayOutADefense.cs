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
    public float Strength { get => strength; set => strength = value; }
    public float Duration { get => duration; set => duration = value; }
    #region µ¥Àý
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
        SelectArmy.Instance.SelectedArmy.GetComponent<ArmyAction>().LayOut();
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
