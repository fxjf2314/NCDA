using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : ScriptableObject,IDescribe
{
    [SerializeField]
    private Description description;

    [SerializeField]
    protected Sprite icon;

    [SerializeField]
    protected List<GameObject> applicableObjs;

    public Description GetDescription()
    {
        return description;
    }

    public virtual void Use()
    {

    }

    public virtual void Hide()
    {

    }

    public virtual void Choose()
    {

    }
}
