using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Terrain", menuName = "Terrain")]
public class Terrain : ScriptableObject
{
    public ExtraValueProp extraValue;
    public ChangeVaule[] changeVaule;

    public void Use(GameObject army)
    {
        extraValue.Use(army);
        foreach (ChangeVaule changeVaule in changeVaule)
        {
            changeVaule.Use(army);
        }
    }
}
[System.Serializable]
public class ExtraValueProp
{
    [SerializeField]
    private float extraNum_Attack;
    [SerializeField]
    private float extraNum_Def;
    public void Use(GameObject army)
    {
        TerrainInteraction terrainInteraction=army.GetComponent<TerrainInteraction>();
        terrainInteraction.exNum_Attack += extraNum_Attack;
        terrainInteraction.exNum_Def += extraNum_Def;
    }
}
[System.Serializable]
public class ChangeVaule
{
    [SerializeField]
    private string val;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float Reduction;
    [SerializeField]
    private float intervalTime;
    public void Use(GameObject army)
    {
        Army armyDetail=army.GetComponent<Army>();
        armyDetail.ControlResource(0.01f, "val", -Reduction, 0.01f);
        armyDetail.ControlResource(duration, "val", Reduction, intervalTime);
    }
}
