using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance => instance;
    private static EnemyAI instance;

    [SerializeField]
    Transform target;
    [SerializeField]
    List<EnemyArmy> enemies;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(this);
            }
        }
        EnemyArmyBehavior.Instance.SurroundArmy(target, enemies.ToArray());
    }
}
