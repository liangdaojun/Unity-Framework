/********************************************************************************
** author：        Liang
** date：          2016-11-14 14:45:52
** description：   通过射线选择物体
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.Events.MouseEvents
{
	/// <summary>
	///  鼠标单击事件；
	/// </summary>
	public class MouseEventBase : EventBase
	{
		/// <summary>
		///  执行用户的函数，并且销毁自身；
		/// </summary>
		protected void Execute()
		{
			if (go.RayHited()) base.Execute();
		}
	}
}