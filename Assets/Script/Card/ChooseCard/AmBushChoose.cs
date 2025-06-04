using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AmBushChoose : ChooseCard
{
    private static AmBushChoose instance;

    public static AmBushChoose MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AmBushChoose>();
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
            if (isChoose)
            {
                Hide();
            }

        }
    }

    protected override void NormalCard()
    {
        if (!isChoose && !CardManager.MyInstance.isCardChosen)
        {
            CardManager.MyInstance.isAmBush = true;
            IsCardChosen();
            if (!isMoving)
            {
                isChoose = true;
                StartCoroutine(MoveUI(20));
            }

            PlaneMaskOpenAndClose();
            card.Choose();

        }
        

    }

    public void Hide()
    {
        CardManager.MyInstance.isAmBush = false;
        IsCardChosen();
        isChoose = false;
        StartCoroutine(MoveUI(-20));
        PlaneMaskOpenAndClose();
        card.Hide();
    }
}
