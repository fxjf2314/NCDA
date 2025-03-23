using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadioOffChoose : ChooseCard
{
    [SerializeField]
    ChooseCard gameObject_1;
    //RectTransform rectTransform;

   
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CanUsedCard(gameObject_1);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isChoose)
            {
                IsCardChosen();
                isChoose = false;
                StartCoroutine(MoveUI(-20));
                PlaneMaskOpenAndClose();
                card.Hide();
            }

        }
    }


    

    

   

    
}
