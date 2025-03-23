using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectArmy : MonoBehaviour
{
    [SerializeField]
    private GameObject armyPanel;
    private GameObject selectedArmy;
    public bool canSelect = true;
    private static SelectArmy instance;
    public GameObject SelectedArmy { get => selectedArmy; set => selectedArmy = value; }
    #region 单例
    public static SelectArmy Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SelectArmy>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("SelectArmy");
                    instance = singletonObject.AddComponent<SelectArmy>();
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
    void LateUpdate()
    {
        if (canSelect&&Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)&& !EventSystem.current.IsPointerOverGameObject())
            {
                DeselectTheArmy();
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    Debug.Log("选中物体：" + hit.transform.gameObject.name);
                    SelectedArmy = hit.transform.gameObject;
                    SelectAArmy();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject()&&hit.transform.gameObject==selectedArmy)
            {
                canSelect = true;
                IntegrateArmy.Instance.SetButton(true);
                DeselectTheArmy();
            }
        }
    }
    #region 选择相关
    public void DeselectTheArmy()
    {
        SetArmy(false);
    }
    public void SelectAArmy()
    {
        SetArmy(true);
    }
    private void SetArmy(bool state)
    {
        if (SelectedArmy != null)
        {
            SelectedArmy.GetComponent<ArmyMovement>().enabled = state;
            SelectedArmy.GetComponent<Outline>().enabled = state;
            armyPanel.SetActive(state);
        }
    }
    #endregion
}
