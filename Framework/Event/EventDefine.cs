/********************************************************************************
** author：        Liang
** date：          2016-12-12 13:23:38
** description：   事件的类型和定义
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections.Generic;


namespace ZF.DataDriveCom.Events
{

	#region	 事件类型定义

	/// <summary>
	///  UnityEvent 事件；这里是可以对事件进行扩充的（用StructLayout特性对事件进行扩充，此版本的C#不支持）；
	/// </summary>
	public struct UnityEvent
	{
		// 事件类型；
		public EventsType EventType;

		// 事件参数；
		public EventParames EventParames;

		// 与事件相关的物体；
		public GameObject[] go;

		// 事件调用的函数及其参数；

		public Action action;

		public Func<bool> func;

		public KeyValuePair<string, Action<string>> actKeyPair;


		/// <summary>
		///  添加有参方法；
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public void AddActKeyPair(string parameters, Action<string> action)
		{
			actKeyPair = new KeyValuePair<string, Action<string>>(parameters, action);
		}

		public override string ToString()
		{
			return string.Format("EventType: {0}   ,GameObject: {1}   , Function: {2}", EventType,
				go == null ? "NULL" : go[0].name, GetFunctionName());
		}

		/// <summary>
		///  返回UnityEvent事件的方法；
		/// </summary>
		/// <returns></returns>
		private string GetFunctionName()
		{
			if (action != null) return action.Method.Name;

			if (func != null) return func.Method.Name;

			if (actKeyPair.Value != null) return actKeyPair.Value.Method.Name;

			return "NULL";
		}
	}

	/// <summary>
	///  事件类型；
	/// </summary>
	public enum EventsType
	{
		LeftMouseDown,

		LeftMouseUp,

		RightMouseDown,

		RightMouseUp,

		MidMouseDown,

		MidMouseUp,

		LeftMouseDoubleClick,

		MouseDrag,

		Trigger,

		OnUpdate,

		OnFixedUpdate,

		OnLateUpdate,
	}


	/// <summary>
	///  事件参数；
	/// </summary>
	[Flags]
	public enum EventParames
	{
		/// <summary>
		/// 不销毁物体；
		/// </summary>
		DontDestory = 1 << 0,

		/// <summary>
		///  不控制该脚本的执行顺序；
		/// </summary>
		NoOrder = 1 << 1,

		/// <summary>
		/// 如果脚本有相应的 bool 参数设置，则对其取反； 
		/// </summary>
		DontSetTrue = 1 << 2
	}

	#endregion
}