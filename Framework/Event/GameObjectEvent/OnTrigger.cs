/********************************************************************************
** author：        Liang
** date：          2016-11-04 10:23:27
** description：   模拟一个物体的触发器，当一个带有 Collider 的物体接触该物体，将触发事件
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.Events.GameObjectEvent
{
	/// <summary>
	///  模拟一个物体的触发器，当一个带有 Collider 的物体接触该物体，将触发事件；
	/// 
	///  注意，真正的触发器的触发条件是：要触发事件的物体必须带有 Rigidbody , 而这种方法则不需要；
	/// </summary>
	public class OnTrigger : EventBase
	{
		/// <summary>
		///  被碰撞物体；
		/// </summary>
		private GameObject collider_Go;

		/// <summary>
		///  当两个物体碰撞时，执行Action函数；
		/// </summary>
		/// <param name="go"></param>
		/// <param name="collider_Go"></param>
		/// <param name="action"></param>
		/// <param name="destroy"></param>
		public void SetAction(GameObject go, GameObject collider_Go, Action action, EventParames EventParames = 0)
		{
			base.SetAction(go, action, EventParames);

			this.collider_Go = collider_Go;
		}


		private void Update()
		{
			if (go.OnIntersect(collider_Go)) Execute();
		}
	}
}