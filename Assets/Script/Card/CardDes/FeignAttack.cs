using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FeignAttack", menuName = "Card/FeignAttack", order = 5)]
public class FeignAttack : Card
{
    public override void Choose()
    {
        foreach (GameObject applicableObject in ArmyManager.MyInstance.allArmies)
        {
            if (applicableObject.GetComponent<Outline>() != null)
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
}
