/********************************************************************************
** author：        Liang
** date：          2016-12-07 17:18:05
** description：   支持客户端自定义事件，相当于一个 LateUpdate 方法
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.Events.GameObjectEvent
{
	/// <summary>
	/// 支持客户端自定义事件，相当于一个 LateUpdate 方法；
	/// </summary>
	public class OnLateUpdate : EventBase
	{
		private void LateUpdate()
		{
			Execute();
		}
	}
}