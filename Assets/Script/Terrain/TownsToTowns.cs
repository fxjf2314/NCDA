using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Unity.VisualScripting;


public class TownsToTowns : MonoBehaviour
{
    public SerializableDictionaryBase<GameObject, GameObject> townsToTowns = new SerializableDictionaryBase<GameObject, GameObject>();
}
