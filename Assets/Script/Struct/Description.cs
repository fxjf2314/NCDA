using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Description
{
    [TextArea(3, 5)]
    public string title;
    [TextArea(3, 5)]
    public string applicableArea;
    [TextArea(3, 5)]
    public string effect;
    
}
