/********************************************************************************
** author：        Liang
** date：          2016-11-16 16:18:55
** description：   鼠标右键按下
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.Events.MouseEvents
{
	public class OnMouseRightDown : MouseEventBase
	{
		/// <summary>
		/// 检测事件
		/// </summary>
		private void Update()
		{
			if (Input.GetMouseButtonDown(1)) Execute();
		}
	}
}