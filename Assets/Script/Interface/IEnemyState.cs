using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ×´Ì¬½Ó¿Ú
public interface IEnemyState
{
    void Enter();
    void IUpdate();
    bool isCanExit(int nextStateLevel);
    void Exit();
}
