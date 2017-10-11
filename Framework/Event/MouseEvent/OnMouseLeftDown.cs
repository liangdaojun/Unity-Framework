/********************************************************************************
** author：        Liang
** date：          2016-11-16 16:18:24
** description：   鼠标左键按下；
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.Events.MouseEvents
{
	public class OnMouseLeftDown : MouseEventBase
	{
		private void Update()
		{
			if (Input.GetMouseButtonDown(0)) Execute();
		}
	}
}