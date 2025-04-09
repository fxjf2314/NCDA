using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectArmy : MonoBehaviour
{
    [SerializeField]
    private GameObject armyPanel;
    [SerializeField]
    private int ambushStrength;
    private GameObject selectedArmy;
    public bool canSelect = true;

    public GameObject attackPanel;
    private Button aYesButton;
    private Button aNoButton;

    bool feignAttack = false;

    private static SelectArmy instance;
    public GameObject SelectedArmy { get => selectedArmy; set => selectedArmy = value; }
    #region ����
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

    private void Start()
    {
        aYesButton = attackPanel.transform.Find("YesButton").GetComponent<Button>();
        aNoButton = attackPanel.transform.Find("NoButton").GetComponent<Button>();
        //aNoButton.onClick.AddListener(transform.GetComponent<ArmyMovement>().CancelMove);
    }
    void LateUpdate()
    {
        Select();
    }

    public void Select()
    {
        if (CardManager.MyInstance.isRadioOff)
        {
            if (canSelect && Input.GetMouseButtonDown(0) && !CardManager.MyInstance.isCardChosen)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, -1, QueryTriggerInteraction.Ignore) && !EventSystem.current.IsPointerOverGameObject())
                {
                    DeselectTheArmy();
                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        if (hit.transform.gameObject.GetComponent<MyArmy>().isCommission)
                        {
                            Debug.Log("ѡ�����壺" + hit.transform.gameObject.name);
                            SelectedArmy = hit.transform.gameObject;
                            SelectAArmy();
                        }

                    }
                }
            }
        }
        else if(CardManager.MyInstance.isRadioExposed)
        {
            if (canSelect && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 500f, -1, QueryTriggerInteraction.Ignore) && !EventSystem.current.IsPointerOverGameObject())
                {
                   
                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        //改变敌人状态
                        CardManager.MyInstance.isRadioExposed = false;
                    }
                }
            }
        }
        else if (CardManager.MyInstance.isAmBush)
        {
            if (canSelect && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, -1, QueryTriggerInteraction.Ignore) && !EventSystem.current.IsPointerOverGameObject())
                {

                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        hit.transform.gameObject.GetComponent<MyArmy>().ControlResource(Time.deltaTime,"strength",ambushStrength,Time.deltaTime);
                        AmBushChoose.MyInstance.Hide();
                        hit.transform.GetComponentInChildren<Detection>().StartDetec();

                        //失去控制
                        //hit.transform.GetComponent<Detection>().
                    }
                }
            }
        }
        else if (CardManager.MyInstance.isFeignAttack)
        {
            if (canSelect && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, -1, QueryTriggerInteraction.Ignore) && !EventSystem.current.IsPointerOverGameObject())
                {

                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        if (hit.transform.gameObject.GetComponent<MyArmy>().ArmyDetail["people"] <= 2000)
                        {
                            Debug.Log("选中了");
                            //GameObject role = hit.transform.gameObject;//role�ǵ���Ľ����𹥵��ҷ�Ŀ��
                            if(feignAttack)
                            {
                                StopCoroutine(WaitForChooseTarget(hit));
                                feignAttack = false;
                            }
                            StartCoroutine(WaitForChooseTarget(hit));
                        }

                    }
                }
            }
        }
        else
        {
            if (canSelect && Input.GetMouseButtonDown(0) && !CardManager.MyInstance.isCardChosen)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, -1, QueryTriggerInteraction.Ignore) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                        DeselectTheArmy();
                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        //Debug.Log("ѡ�����壺" + hit.transform.gameObject.name);
                        SelectedArmy = hit.transform.gameObject;
                        SelectAArmy();
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, -1, QueryTriggerInteraction.Ignore) && !EventSystem.current.IsPointerOverGameObject() && hit.transform.gameObject == selectedArmy)
                {
                    canSelect = true;
                    IntegrateArmy.Instance.SetButton(true);
                    DeselectTheArmy();
                }
            }
        }
    }

    #region ѡ�����
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

    #region ʹ��ս�����
    private IEnumerator WaitForChooseTarget(RaycastHit a)
    {
        feignAttack = true;
        while (true)
        {
            if (Input.GetMouseButtonDown(0)) // ��������
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && !hit.transform.gameObject.CompareTag("Player") && !EventSystem.current.IsPointerOverGameObject()) // �����λ��
                {
                    
                    switch (hit.transform.gameObject.tag)
                    {
                        case "Army":
                            aYesButton.onClick.AddListener(() => a.transform.GetComponent<MoveToOthers>().Move(hit.transform));
                            aYesButton.onClick.AddListener(() => FeignAttackChoose.MyInstance.CancelOrFinish());
                            attackPanel.SetActive(true);
                            
                            break;
                        case "Town":
                            aYesButton.onClick.AddListener(()=> a.transform.GetComponent<NavMeshAgent>().SetDestination(hit.transform.position)); 
                            aYesButton.onClick.AddListener(() => FeignAttackChoose.MyInstance.CancelOrFinish());
                            attackPanel.SetActive(true);
                            
                            break;
                        default:

                            break;

                    }

                }

            }
            yield return null;
        }
    }

 



    #endregion
}


