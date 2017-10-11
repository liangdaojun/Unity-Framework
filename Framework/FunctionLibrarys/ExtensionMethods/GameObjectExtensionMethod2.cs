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
using ZF.DataDriveCom.Events.MouseEvents;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  GameObject 的鼠标事件；
	/// </summary>
	public static partial class GameObjectExtensionMethod
	{
		#region  鼠标单击事件

		public static void OnMouseLeftDown(this GameObject gameObject, Action action, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.LeftMouseDown, new[] {gameObject}, action, EventParames);
		}

		public static void OnMouseLeftDown(this GameObject gameObject, Action<string> action, string parameters,
			EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.LeftMouseDown, new[] {gameObject}, action, parameters, EventParames);
		}


		public static void OnMouseLeftUp(this GameObject gameObject, Action action, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.LeftMouseUp, new[] {gameObject}, action, EventParames);
		}

		public static void OnMouseLeftUp(this GameObject gameObject, Action<string> action, string parameters,
			EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.LeftMouseUp, new[] {gameObject}, action, parameters, EventParames);
		}

		public static void OnMouseRightDown(this GameObject gameObject, Action action, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.RightMouseDown, new[] {gameObject}, action, EventParames);
		}

		public static void OnMouseRightDown(this GameObject gameObject, Action<string> action, string parameters,
			EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.RightMouseDown, new[] {gameObject}, action, parameters, EventParames);
		}

		public static void OnMouseRightUp(this GameObject gameObject, Action action, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.RightMouseUp, new[] {gameObject}, action, EventParames);
		}

		public static void OnMouseRightUp(this GameObject gameObject, Action<string> action, string parameters,
			EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.RightMouseUp, new[] {gameObject}, action, parameters, EventParames);
		}


		/// <summary>
		///  为鼠标添加双击事件；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="func"></param>
		public static void OnMouseDoubleClick(this GameObject gameObject, Action action, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.LeftMouseDoubleClick, new[] {gameObject}, action, EventParames);
		}

		public static void OnMouseDoubleClick(this GameObject gameObject, Action<string> action, string parameters,
			EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.LeftMouseDoubleClick, new[] {gameObject}, action, parameters, EventParames);
		}

		#region	 移除鼠标单击事件

		/// <summary>
		///  鼠标事件，并不在鼠标身上；
		/// </summary>
		/// <param name="gameObject"></param>
		public static void OffMouseLeftDown(this GameObject gameObject)
		{
			EventManager.OffEvent<OnMouseLeftDown>(gameObject);
		}


		public static void OffMouseLeftUp(this GameObject gameObject)
		{
			EventManager.OffEvent<OnMouseLeftUp>(gameObject);
		}


		public static void OffMouseRightDown(this GameObject gameObject)
		{
			EventManager.OffEvent<OnMouseRightDown>(gameObject);
		}


		public static void OffMouseRightUp(this GameObject gameObject)
		{
			EventManager.OffEvent<OnMouseRightUp>(gameObject);
		}

		/// <summary>
		///  为鼠标添加双击事件；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="func"></param>
		public static void OffMouseDoubleClick(this GameObject gameObject)
		{
			EventManager.OffEvent<OnMouseDoubleClick>(gameObject);
		}

		#endregion

		#endregion

		#region  鼠标的其他事件

		// 鼠标的拖拽以及它的一些重载方法；

		/// <summary>
		///  添加鼠标拖拽事件；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="EventParames"></param>
		public static void OnMouseDrag( this GameObject gameObject , EventParames EventParames = 0 )
		{
			EventManager.SetFunc( EventsType.MouseDrag , new[] { gameObject } , new Action(() => { }) , EventParames );
		}

		/// <summary>
		///  添加鼠标拖拽事件；添加事件完成时的 Action 函数；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="action"></param>
		public static void OnMouseDrag(this GameObject gameObject, Action function, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.MouseDrag, new[] {gameObject}, function, EventParames);
		}

		public static void OnMouseDrag(this GameObject gameObject, Action<string> function, string parameters,
			EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.MouseDrag, new[] {gameObject}, function, parameters, EventParames);
		}


		public static void OnMouseDrag(this GameObject gameObject,GameObject gameObject2,EventParames EventParames=0)
		{
			EventManager.SetFunc( EventsType.MouseDrag , new[] { gameObject , gameObject2 } , new Action(()=>{}) , EventParames );
		}

		public static void OnMouseDrag(this GameObject gameObject, GameObject gameObject2, Action function,
			EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.MouseDrag, new[] {gameObject, gameObject2}, function, EventParames);
		}

		public static void OnMouseDrag(this GameObject gameObject, GameObject gameObject2, Action<string> function,
			string parameters, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.MouseDrag, new[] {gameObject, gameObject2}, function, parameters, EventParames);
		}

		#region	 移除鼠标事件

		public static void OffMouseDrag(this GameObject gameObject)
		{
			EventManager.OffEvent<OnMouseDrag>(gameObject);
		}

		#endregion

		#endregion

		/// <summary>
		///  删除某个物体上的某个事件；
		/// </summary>
		/// <param name="go"></param>
		/// <param name="eventName"></param>
		public static void OffEvent(this GameObject go, string eventName)
		{
			EventManager.OffEvent(go, eventName);
		}
	}
}