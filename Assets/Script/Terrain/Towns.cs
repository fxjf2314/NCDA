using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using TMPro;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Towns : MonoBehaviour
{
    public bool occupied = false;
    public Transform relatedTown;
    public GameObject townPanel;
    private Button yesButton;
    private List<Transform> armies = new List<Transform>();
    public float duration;
    public float intervalTime;
    public Dictionary<MyArmy, Coroutine> tasks = new Dictionary<MyArmy, Coroutine>();

    private void Start()
    {
        yesButton = townPanel.transform.Find("Image").Find("YesButton").GetComponent<Button>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MyArmy army = other.GetComponent<MyArmy>();
            townPanel.SetActive(true);
            Transform Image = townPanel.transform.Find("Image");
            Image.Find("Occupation").GetComponent<TextMeshProUGUI>().text = occupied ? "occupied": "unoccupied";
            yesButton.onClick.RemoveListener(Cross);
            yesButton.onClick.AddListener(Cross);
            if (!armies.Contains(other.transform))
            {
                armies.Add(other.transform);
            }
            if (army.GetStrength() < army.defaultStrength && occupied)
            {
                Coroutine task = army.ControlResource(duration, "strength",army.defaultStrength-army.GetStrength(),intervalTime);
                if (task != null)
                {
                    tasks.Add(army, task);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            MyArmy army = other.GetComponent<MyArmy>();
            townPanel.SetActive(false);
            armies.Remove(other.transform);
            if (tasks.TryGetValue(army, out Coroutine runningTask))
            {
                if (runningTask != null)
                {
                    army.StopCoroutine(runningTask);
                }
                tasks.Remove(army);
            }
        }
    }
    
    public void Cross()
    {
        yesButton.onClick.RemoveListener(Cross);
        Vector3 position = relatedTown.position - transform.position;
        foreach (Transform army in armies) 
        {
            army.position += position;
            NavMeshAgent agent = army.GetComponent<NavMeshAgent>();
            agent.SetDestination(army.position);
            army.GetComponent<ArmyMovement>().path = new NavMeshPath();
        }
=======
using UnityEngine;

public class Towns : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
>>>>>>> Stashed changes
    }
}
