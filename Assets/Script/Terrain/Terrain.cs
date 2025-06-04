using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Terrain", menuName = "Terrain")]
public class Terrain : ScriptableObject
{
    public string type;
    public ExtraValue extraValue;
    public ChangeVaule[] changeVaule;

    public void Use(GameObject army)
    {
<<<<<<< Updated upstream
        Debug.Log($"进入地形: {type}");
        TerrainInteraction terrainInteraction = army.GetComponent<TerrainInteraction>();
        MyArmy armyDetail = army.GetComponent<MyArmy>();
=======
        TerrainInteraction terrainInteraction = army.GetComponent<TerrainInteraction>();
        Army armyDetail = army.GetComponent<Army>();
>>>>>>> Stashed changes
        
        extraValue.Use(terrainInteraction);
        foreach (ChangeVaule changeVaule in changeVaule)
        {
            changeVaule.Use(armyDetail);
        }
    }
    public void Exit(GameObject army)
    {
<<<<<<< Updated upstream
        Debug.Log($"离开地形: {type}");
        TerrainInteraction terrainInteraction = army.GetComponent<TerrainInteraction>();
        MyArmy armyDetail = army.GetComponent<MyArmy>();
=======
        TerrainInteraction terrainInteraction = army.GetComponent<TerrainInteraction>();
        Army armyDetail = army.GetComponent<Army>();
>>>>>>> Stashed changes
        extraValue.Exit(terrainInteraction);
        foreach (ChangeVaule changeVaule in changeVaule)
        {
            changeVaule.Exit(armyDetail);
        }
    }
}
[System.Serializable]
public class ExtraValue
{
    [SerializeField]
    private float extraNum_Attack;
    [SerializeField]
    private float extraNum_Def;
    private float oExtraNum_A;
    private float oExtraNum_D;
    public void Use(TerrainInteraction terrainInteraction)
    {
        oExtraNum_A = terrainInteraction.exNum_Attack;
        oExtraNum_D = terrainInteraction.exNum_Def;
        terrainInteraction.exNum_Attack += extraNum_Attack;
        terrainInteraction.exNum_Def += extraNum_Def;
    }
    public void Exit(TerrainInteraction terrainInteraction)
    {
        terrainInteraction.exNum_Attack = oExtraNum_A;
        terrainInteraction.exNum_Def = oExtraNum_D;
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
    [Header("对于攻击力来说deltaNum=0则恢复至默认值")]
    private float deltaNum;
    [SerializeField]
    private float intervalTime;
<<<<<<< Updated upstream
    public void Use(MyArmy army)
    {
        if (val == "strength" && deltaNum == 0)
        {
            if (army.GetStrength() < army.defaultStrength)
                army.ControlResource(duration, val, army.defaultStrength - army.GetStrength(), intervalTime);
        }
        else
        {

            army.ControlResource(0.01f, val, deltaNum, 0.01f);
        }
    }
    public void Exit(MyArmy army)
    {
        army.ControlResource(duration, val, -deltaNum, intervalTime);
    }
}
=======
    public void Use(Army army)
    {
        if (val == "strength" && deltaNum == 0)
        {
            //恢复至默认值
        }
        else 
        {

            army.ControlResource(0.01f, val, deltaNum, 0.01f);

        }
    }
    public void Exit(Army army)
    {
        army.ControlResource(duration, val, -deltaNum, intervalTime);
    }
 }
>>>>>>> Stashed changes
