//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Widget container is a generic type class that acts like a non-resizeable widget when selecting things in the scene view.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Widget Container")]
public class UIWidgetContainer : MonoBehaviour
{
    // 先这样控制 UIButton、UIToggle 等；


    // 此属性表示该 控件 被控制与否； 

    public bool Controller ;

    // 此属性表示该控件的唯一标识符；

    public string Identifer ;

    
    

}