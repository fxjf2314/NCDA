using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCard : MonoBehaviour, IPointerClickHandler
{
   
    [SerializeField]
    protected GameObject plane;

    [SerializeField]
    protected Card card;

    protected Renderer renderer; 

    public float moveDistance;

    public float moveDuration;

    protected Vector3 initPosition;

    protected bool isMoving = false;

    protected bool isChoose = false;

    protected bool canUse = true;

    [SerializeField]
    protected Image image;

    
    protected float transparent = 0.4f;
    //RectTransform rectTransform;

    protected virtual void Start()
    {
        plane = GameObject.Find("PlaneMask");
        renderer = plane.GetComponent<Renderer>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    //ui移动的协程
    protected IEnumerator MoveUI(float moveDistance)
    {
        initPosition = transform.position;
        isMoving = true;

        float elapsedTime = 0.0f;
        Vector3 targetPosition = initPosition + new Vector3(0,moveDistance,0);

        while(elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(initPosition,targetPosition, elapsedTime/moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    //有使用限制的卡牌脚本
    protected void CanUsedCard(ChooseCard gameObj_1)
    {
        if (!isChoose && canUse && !CardManager.MyInstance.isCardChosen)
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
            IsCardChosen();
            isChoose = false;
            canUse = false;
            TurnToGrey();
            StartCoroutine(MoveUI(-20));
            PlaneMaskOpenAndClose();
            card.Choose();
            card.Use();
            gameObj_1.TurnToWhite();
            gameObj_1.canUse = !canUse;
        }
    }

    //无使用限制的卡牌脚本
    protected virtual void NormalCard()
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
            //card.Choose();
        }
        else if (isChoose)
        {
            IsCardChosen();
            isChoose = false;
            StartCoroutine(MoveUI(-20));
            PlaneMaskOpenAndClose();
            card.Choose();
            card.Use();
        }
    }

    //当前槽位变白
    protected void TurnToWhite()
    {
        gameObject.GetComponent<Image>().color = Color.white;
        image.color = Color.white;
    }

    //当前槽位变灰
    protected void TurnToGrey()
    {
        gameObject.GetComponent<Image>().color = Color.gray;
        image.color = Color.gray;
    }

    protected void IsCardChosen()
    {
        CardManager.MyInstance.isCardChosen = !CardManager.MyInstance.isCardChosen;
    }

    protected void PlaneMaskOpenAndClose()
    {
        Color color = renderer.material.color;
        color.a = color.a == 0 ? transparent : 0;
        renderer.material.color = color;
    }
}
