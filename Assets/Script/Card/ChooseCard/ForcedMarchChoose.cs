using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForcedMarchChoose : ChooseCard
{
    private static ForcedMarchChoose instance;

    public static ForcedMarchChoose MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ForcedMarchChoose>();
            }
            return instance;
        }

    }

    [SerializeField]
    GameObject effectiveRange;

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

    public void Hide()
    {
        IsCardChosen();
        isChoose = false;
        StartCoroutine(MoveUI(-20));
        PlaneMaskOpenAndClose();
        card.Hide();
        effectiveRange.SetActive(false);
        foreach (Army army in CardManager.MyInstance.armies)
        {
            army.GetComponent<Outline>().enabled = false;
        }
        CardManager.MyInstance.armies.Clear();
    }

    protected override void NormalCard()
    {
        if (!isChoose && !CardManager.MyInstance.isCardChosen)
        {
            effectiveRange.SetActive(true);
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
}
