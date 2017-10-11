/********************************************************************************
** author：        Liang
** date：          2016-11-16 16:19:11
** description：   鼠标右键抬起
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.Events.MouseEvents
{
	public class OnMouseRightUp : MouseEventBase
	{
		private void Update()
		{
			if (Input.GetMouseButtonUp(2)) Execute();
		}
	}
}