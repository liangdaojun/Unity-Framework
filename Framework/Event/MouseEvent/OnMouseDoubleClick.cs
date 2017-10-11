/********************************************************************************
** author：        Liang
** date：          2016-11-07 15:12:27
** description：   鼠标的双击事件
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.Events.MouseEvents
{
	/// <summary>
	///  鼠标的双击事件；
	/// </summary>
	public class OnMouseDoubleClick : MouseEventBase
	{
		private void OnGUI()
		{
			if (Event.current.isMouse
			    && Event.current.type == EventType.MouseDown
			    && Event.current.clickCount == 2)
			{
				Execute();
			}
		}
	}
}