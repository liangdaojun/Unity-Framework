/********************************************************************************
** author：       Liang
** date：         2016-11-20 19:58:03
** description：  当射线检测到相应物体
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.Events.GameObjectEvent
{
	/// <summary>
	///  当指定物体被射线检测到，就执行相应的Action；
	/// </summary>
	public class OnRaycastHit : EventBase
	{
		private void Update()
		{
			if (go == go.RayHited()) Execute();
		}
	}
}