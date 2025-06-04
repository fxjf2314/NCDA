using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 使用 ScriptableObject 存储配置数据
[CreateAssetMenu(fileName = "EnemyAIConfig", menuName = "AI/Config")]
public class EnemyAIConfig : ScriptableObject
{
    [Header("Movement")]
    public float checkRadius = 50f;      // 检测范围
    public float retreatDistance = 30f;  // 安全撤退距离

    public float minDis = 5; //判断是否到达的距离

    //[Header("攻击")]
    //[SerializeField][Range(0, 1)] float attackThreshold = 0.7f; // 攻击优势阈值
    //[SerializeField] float healCooldown = 10f;     // 治疗冷却时间

    [Header("移动,攻击,驻扎,防御")]
    [SerializeField] float oMove = 4, oAttack = 2, oStation = 3, oDefence = 2.5f;
    [HideInInspector]public float move = 4, attack = 2, station = 3, deffence = 2.5f;

    public int healingSpeed = 10;
}
