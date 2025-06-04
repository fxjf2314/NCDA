using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FeignAttackChoose : ChooseCard
{
    private static FeignAttackChoose instance;

    public static FeignAttackChoose MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FeignAttackChoose>();
            }
            return instance;
        }

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            NormalCard();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CancelOrFinish();

        }
    }

    protected override void NormalCard()
    {
        if (!isChoose && !CardManager.MyInstance.isCardChosen)
        {
            CardManager.MyInstance.isFeignAttack = true;
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

    public void CancelOrFinish()
    {
        if (isChoose)
        {
            CardManager.MyInstance.isFeignAttack = false;
            IsCardChosen();
            isChoose = false;
            StartCoroutine(MoveUI(-20));
            PlaneMaskOpenAndClose();
            card.Hide();
        }
    }
}
