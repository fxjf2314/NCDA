using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ExposedRadio", menuName = "Card/ExposedRadio", order = 3)]
public class ExposedRadio :Card
{
    
    public override void Choose()
    {
        foreach (GameObject applicableObject in ArmyManager.MyInstance.allArmies)
        {
            if(applicableObject.GetComponent<Outline>() != null)
            {
                
                applicableObject.GetComponent<Outline>().enabled = true;
                
            }
        }
    }

    public override void Hide()
    {
        foreach (GameObject applicableObject in ArmyManager.MyInstance.allArmies)
        {
            if (applicableObject.GetComponent<Outline>() != null)
            {

                applicableObject.GetComponent<Outline>().enabled = false;
                
            }
        }
    }

    public override void Use()
    {
        //
        
    }
}
