using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private static CardManager instance;

    public static CardManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CardManager>();
            }
            return instance;
        }

    }

    public bool isCardChosen;

    public List<Army> armies;

    
}