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
                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
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
                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
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
                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
                {

                    if (hit.transform.gameObject.CompareTag("Player"))
                    {
                        hit.transform.gameObject.GetComponent<MyArmy>().StrengthControl(ambushStrength);
                        
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
                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
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
                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
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
        }
        
        
        
    }

    #region ѡ�����
    public void DeselectTheArmy()
    {
        SetArmy(false);
    }
    private void SelectAArmy()
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
    private void SelectTarget(RaycastHit a)
    {
        if (Input.GetMouseButtonDown(0)) // ��������
        {
            Action action;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !hit.transform.gameObject.CompareTag("Player") && !EventSystem.current.IsPointerOverGameObject()) // �����λ��
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Army":
                        aYesButton.onClick.AddListener(() => a.transform.GetComponent<Collider>().GetComponent<MoveToOthers>().Move(hit.transform));
                        action = () =>
                        {
                            attackPanel.SetActive(true);
                        };
                        break;
                    case "Town":
                        aYesButton.onClick.AddListener(transform.GetComponent<ArmyMovement>().ArmyMove);
                        action = () =>
                        {
                            attackPanel.SetActive(true);
                        };
                        break;
                    default:
                        
                        break;

                }
                
            }
        }
    }
    
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
                            aYesButton.onClick.AddListener(() => a.transform.GetComponent<Collider>().GetComponent<MoveToOthers>().Move(hit.transform));
                            aYesButton.onClick.AddListener(() => FeignAttackChoose.MyInstance.CancelOrFinish());
                            attackPanel.SetActive(true);
                            
                            break;
                        case "Town":
                            aYesButton.onClick.AddListener(()=> a.transform.GetComponent<Collider>().GetComponent<NavMeshAgent>().SetDestination(hit.transform.position)); 
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


