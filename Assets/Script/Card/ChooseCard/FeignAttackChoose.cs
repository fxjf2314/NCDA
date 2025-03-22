using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FeignAttackChoose : ChooseCard
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            NormalCard();
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

    protected override void NormalCard()
    {
        if (!isChoose && !CardManager.MyInstance.isCardChosen)
        {
            IsCardChosen();
            if (!isMoving)
            {
                isChoose = true;
                StartCoroutine(MoveUI(20));
            }

            PlaneMaskOpenAndClose();
            card.Choose();

        }
        else if (isChoose)
        {
            //
        }

    }
}
