/********************************************************************************
** author：        Liang
** date：          2016-11-11 09:02:06
** description：   事件类的抽象基类
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;


namespace ZF.DataDriveCom.Events
{
	/// <summary>
	///  事件类的抽象基类；
	/// </summary>
	public class EventBase : MonoBehaviour
	{
		/// <summary>
		///  被操作物体；
		/// </summary>
		[HideInInspector] public GameObject go;

		/// <summary>
		///  条件成立时，执行的方法；
		/// </summary>
		protected Action action;

		/// <summary>
		///  事件的参数；
		/// </summary>
		protected EventParames EventParames;

		/// <summary>
		///  为被操作物体设置参数；
		/// </summary>
		/// <param name="go"></param>
		/// <param name="action"></param>
		/// <param name="destroy"></param>
		public virtual void SetAction(GameObject go, Action action, EventParames EventParames = 0)
		{
			this.go = go;

			this.action = action;

			this.EventParames = EventParames;
		}


		/// <summary>
		///  执行 Action；
		/// </summary>
		protected virtual void Execute()
		{
			if (action != null)
			{
				action();

				if ((EventParames & EventParames.DontDestory) == 0) Destroy(this);
			}
		}
	}
}