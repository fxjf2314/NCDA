using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSet:MonoBehaviour
{
    private static GameSet instance;
    public static GameSet Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameSet>();
                if (instance == null) 
                {
                    
                }
            }
            return instance;
        }
    }

    public float volumn;
    public bool openvolumn;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(volumn == 0)
        {
            openvolumn = false;
        }
        else
        {
            openvolumn=true;
        }
    }
}
