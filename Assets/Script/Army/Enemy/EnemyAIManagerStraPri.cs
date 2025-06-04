using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManagerStraPri : MonoBehaviour
{
    public static EnemyAIManagerStraPri Instance => instance;
    static EnemyAIManagerStraPri instance;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void MoveForward()
    {
        float priority;

    }
}
