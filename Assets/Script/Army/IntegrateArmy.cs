using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IntegrateArmy : MonoBehaviour
{
    public Button DdButton;
    public Button FbButton;
    public Button BfButton;
    public Button HbButton;
    private bool selectOver;
    private bool isSelecting;
    private GameObject thisArmy;
    private GameObject selectedArmy;
    public GameObject IntegrateImage;
    private static IntegrateArmy instance;
    #region µ¥Àý
    public static IntegrateArmy Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<IntegrateArmy>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("IntegrateArmy");
                    instance = singletonObject.AddComponent<IntegrateArmy>();
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
        HbButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        HbButton.onClick.AddListener(()=>SetButton(GetStateFromA(HbButton.gameObject.GetComponent<Image>().color)));
    }
    private void Update()
    {
        if (!SelectArmy.Instance.canSelect&&!isSelecting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(SelectAArmy());
            }
        }
    }
    IEnumerator SelectAArmy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                if(hit.transform.gameObject == thisArmy)
                {
                    SelectArmy.Instance.SelectAArmy();
                }
                else
                {
                    selectOver = false;
                    IntegrateImage.SetActive(true);
                    isSelecting = true;
                    yield return new WaitWhile(() => isSelecting);
                    IntegrateImage.SetActive(false);
                    if (selectOver)
                    {
                        selectedArmy = hit.transform.gameObject;
                        thisArmy.GetComponent<MoveToOthers>().Move(selectedArmy.transform);
                        selectedArmy.GetComponent<MoveToOthers>().Move(thisArmy.transform);
                        yield return new WaitUntil(() => !thisArmy.GetComponent<MoveToOthers>().CanMove || !selectedArmy.GetComponent<MoveToOthers>().CanMove);
                        thisArmy.GetComponent<MoveToOthers>().CanMove = false;
                        selectedArmy.GetComponent<MoveToOthers>().CanMove = false;
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
        SelectArmy.Instance.canSelect=false;
        thisArmy=SelectArmy.Instance.SelectedArmy;
    }
    public void OverSelect()
    {
        selectOver= true;
        isSelecting = false;
    }
    public void IsSelecting()
    {
        isSelecting= false;
    }

    public bool GetStateFromA(Color color)
    {
        if (color.a==0.5f)
            return false;
        else if(color.a==1f)
            return true;
        else return false;
    }
    public void SetButton(bool state)
    {
        if (!state)
        {
            HbButton.gameObject.GetComponent<Image>().color = new Color(1,1,1,1);
        }
        else
        {
            HbButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            SelectArmy.Instance.canSelect = true;
        }
        FbButton.interactable = state;
        BfButton.interactable = state;
        DdButton.interactable = state;
    }
}
