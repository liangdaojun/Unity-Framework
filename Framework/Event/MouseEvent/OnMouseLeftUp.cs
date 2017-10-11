/********************************************************************************
** author：        Liang
** date：          2016-11-16 16:18:38
** description：   鼠标左键抬起
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.Events.MouseEvents
{
	public class OnMouseLeftUp : MouseEventBase
	{
		private void Update()
		{
			if (Input.GetMouseButtonUp(0)) Execute();
		}
	}
}