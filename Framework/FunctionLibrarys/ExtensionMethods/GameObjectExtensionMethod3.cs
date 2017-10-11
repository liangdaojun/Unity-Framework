/********************************************************************************
** author：        Liang
** date：          2016-11-14 15:09:46
** description：   GameObject 的扩展方法
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.Events.GameObjectEvent;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  GameObject 的物体其他（键盘或者碰撞）事件；
	/// </summary>
	public static partial class GameObjectExtensionMethod
	{



		/// <summary>
		///  为第一个物体设置触发器， 当两个物体相接触，就会执行 Action；
		/// 
		///  要求两个物体都必须要有 Collider；否则，将抛出异常；
		/// </summary>
		/// <param name="go1"></param>
		/// <param name="go2"></param>
		/// <param name="action"></param>
		public static void OnTrigger(this GameObject go1, GameObject go2, Action action, EventParames EventParames=0)
		{
			if (!go1.GetComponent<Collider>() || !go2.GetComponent<Collider>()) throw new Exception("缺少Collider!");

			if (action == null) throw new Exception("Action 不能为空");

			go1.AddComponent<OnTrigger>().SetAction(go1, go2, action, EventParames);
		}

		#region	 移除事件

		public static void OffTrigger(this GameObject gameObject)
		{
			EventManager.OffEvent<OnTrigger>(gameObject);
		}

		#endregion
	}
}