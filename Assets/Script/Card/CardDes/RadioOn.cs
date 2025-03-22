using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RadioOn", menuName = "Card/RadioOn", order = 2)]
public class RadioOn :Card
{
    
    public override void Choose()
    {
        foreach (GameObject applicableObject in ArmyManager.MyInstance.otherArmies)
        {
            if(applicableObject.GetComponent<Outline>() != null)
            {
                
                applicableObject.GetComponent<Outline>().enabled = true;
                applicableObject.GetComponent<Renderer>().material.color = Color.grey;
            }
        }
    }

    public override void Hide()
    {
        foreach (GameObject applicableObject in ArmyManager.MyInstance.otherArmies)
        {
            if (applicableObject.GetComponent<Outline>() != null)
            {

                applicableObject.GetComponent<Outline>().enabled = false;
                applicableObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    public override void Use()
    {
        foreach (GameObject applicableObject in ArmyManager.MyInstance.otherArmies)
        {
            if (applicableObject.GetComponent<Outline>() != null)
            {

                applicableObject.GetComponent<Outline>().enabled = false;
                applicableObject.GetComponent<Renderer>().material.color = Color.grey;
            }
        }
        //无法选中其余军队，只能操纵军委纵队
    }
}
