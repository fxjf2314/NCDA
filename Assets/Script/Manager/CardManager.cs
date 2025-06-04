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

    public List<MyArmy> armies;

    public bool isRadioOff;

    public bool isRadioExposed;

    public bool isAmBush;

    public bool isFeignAttack;

    public float forcedMarchSpeed;

    public float forcedMarchStrength;
}