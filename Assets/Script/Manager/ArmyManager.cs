using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    private static ArmyManager instance;

    public static ArmyManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ArmyManager>();
            }
            return instance;
        }

    }

    
    public List<GameObject> allArmies = new List<GameObject>();

    public List<GameObject> otherArmies = new List<GameObject>();
}
