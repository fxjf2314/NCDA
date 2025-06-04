using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Canbedrag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public float XleftClamp,XrightClamp;
    public float YleftClamp,YrightClamp;
    RectTransform rectTransform;

    private void Awake()
    {
        if (GetComponent<RectTransform>() != null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        else
        {
            Debug.Log("No RecTransform");
        }
    }

    public void OnBeginDrag(PointerEventData eventdate)
    {
        
    }

    public void OnDrag(PointerEventData eventdate)
    {
        //rectTransform.anchoredPosition=eventdate.position;
        rectTransform.anchoredPosition += eventdate.delta / rectTransform.lossyScale.x;
        Vector2 now = rectTransform.anchoredPosition;
        now.x = Mathf.Clamp(now.x, XleftClamp, XrightClamp);
        now.y=Mathf.Clamp(now.y, YleftClamp, YrightClamp);
        rectTransform.anchoredPosition = now;
    }

    public void OnEndDrag(PointerEventData eventdate) 
    {

    }


}
