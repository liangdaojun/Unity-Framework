/********************************************************************************
** author：        Liang
** date：          2016-11-21 15:14:04
** description：   响应事件的函数库
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZF.DataDriveCom.Service;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  这里收集客户端定义的响应事件的函数库；这些函数默认执行一遍就销毁；
	/// </summary>
	public class EventFunctionLibrary
	{
		/// <summary>
		///  存储事件的函数库；
		/// </summary>
		private static Dictionary<string, KeyValuePair<bool, Action>> DictsAction =
			new Dictionary<string, KeyValuePair<bool, Action>>();

		/// <summary>
		///  存储返回值的事件的函数库；
		/// </summary>
		private static Dictionary<string, KeyValuePair<bool, Func<bool>>> DictsFunc =
			new Dictionary<string, KeyValuePair<bool, Func<bool>>>();

		/// <summary>
		///  存储带参数的函数库；
		/// </summary>
		private static Dictionary<string, KeyValuePair<bool, Action<string>>> DictsActionT =
			new Dictionary<string, KeyValuePair<bool, Action<string>>>();


		/// <summary>
		///  默认的执行函数；
		/// </summary>
		private static Action DefaultEventAction = () => RequestExecuteService.ExecuteNextStep();

		#region 查看一个方法是否存在；

		/// <summary>
		///  查看是否包含有某个方法；
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns></returns>
		public static bool ExternalDispose(string functionName)
		{
			return !String.IsNullOrEmpty(functionName) && DictsAction.ContainsKey(functionName.ToLower());
		}

		/// <summary>
		///  查看是否包含有某个带参方法；
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns></returns>
		public static bool ExternalDisposeT(string functionName)
		{
			return !String.IsNullOrEmpty(functionName) && DictsActionT.ContainsKey(functionName.ToLower());
		}

		/// <summary>
		///  查看是否包含有某个带参方法；
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns></returns>
		public static bool ExternalDisposeFunc(string functionName)
		{
			return !String.IsNullOrEmpty(functionName) && DictsFunc.ContainsKey(functionName.ToLower());
		}

		#endregion

		#region 添加一个方法；

		/// <summary>
		///  添加一个Action 类型的函数；
		/// </summary>
		/// <param name="functionName"></param>
		/// <param name="action"></param>
		/// <param name="dontDestroy"></param>
		public static void Add(string functionName, Action action, bool dontDestroy = false)
		{
			DictsAction.Add(functionName.ToLower(), new KeyValuePair<bool, Action>(dontDestroy, action));
		}

		/// <summary>
		///  添加一个Action<string>的函数；
		/// </summary>
		/// <param name="functionName"></param>
		/// <param name="action"></param>
		/// <param name="dontDestroy"></param>
		public static void Add(string functionName, Action<string> action, bool dontDestroy = false)
		{
			DictsActionT.Add(functionName.ToLower(), new KeyValuePair<bool, Action<string>>(dontDestroy, action));
		}

		/// <summary>
		///  添加一个 Func<bool> 的函数；
		/// </summary>
		/// <param name="functionName"></param>
		/// <param name="func"></param>
		/// <param name="dontDestroy"></param>
		public static void Add(string functionName, Func<bool> func, bool dontDestroy = false)
		{
			DictsFunc.Add(functionName.ToLower(), new KeyValuePair<bool, Func<bool>>(dontDestroy, func));
		}

		#endregion

		#region 获取指定类型的函数；

		/// <summary>
		///  统一获取函数；TODO
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns></returns>
		public static object GetFunction(string functionName)
		{
			if (ExternalDispose(functionName)) return GetAction(functionName);

			if (ExternalDisposeFunc(functionName)) return GetFunc(functionName);

			if (ExternalDisposeT(functionName)) return GetActionT(functionName);

			throw new Exception("不支持该类型的函数！");
		}


		/// <summary>
		///  根据Action 名称，返回一个Action；规则相同，不区分大小写；
		/// 
		///	 该类型的方法，默认执行一次；如果一个方法要执行多次，将dontDestroy 设为 true；
		/// </summary>
		/// <param name="actionName"></param>
		/// <returns></returns>
		public static Action GetAction(string actionName)
		{
			if (!ExternalDispose(actionName)) return DefaultEventAction;

			return DictsAction[actionName.ToLower()].Value;
		}

		/// <summary>
		///  根据Action 名称，返回一个Action<string>；规则相同，不区分大小写；
		/// 
		///	 该类型的方法，默认执行一次；如果一个方法要执行多次，将dontDestroy 设为 true；
		/// </summary>
		/// <param name="actionName"></param>
		/// <returns></returns>
		public static Action<string> GetActionT(string actionName)
		{
			if (!ExternalDisposeT(actionName))

				throw new Exception(string.Format("没有找到指定{0}方法", actionName));

			return DictsActionT[actionName.ToLower()].Value;
		}

		/// <summary>
		///  根据Func 名称，返回一个Func<bool>；规则相同，不区分大小写；
		/// 
		///	 该类型的方法，默认执行一次；如果一个方法要执行多次，将dontDestroy 设为 true；
		/// </summary>
		/// <param name="funcName"></param>
		/// <returns></returns>
		public static Func<bool> GetFunc(string funcName)
		{
			if (!ExternalDisposeFunc(funcName))

				throw new Exception(string.Format("没有找到指定{0}方法", funcName));

			return DictsFunc[funcName.ToLower()].Value;
		}

		#endregion

		/// <summary>
		///  清空所有 dontDestroy=false 的函数；即销毁所有需要销毁的函数；
		/// </summary>
		public static void ClearNeedDestroyFunction()
		{
			List<string> keyList = new List<string>();

			foreach (var key in DictsAction.Keys)
			{
				if (DictsAction[key].Key == false) keyList.Add(key);
			}

			foreach (var key in keyList)
			{
				DictsAction.Remove(key);
			}

			keyList.Clear();

			foreach (var key in DictsActionT.Keys)
			{
				if (!DictsActionT[key].Key) keyList.Add(key);
			}

			foreach (var key in keyList)
			{
				DictsActionT.Remove(key);
			}
		}


		/// <summary>
		///  为事件设置默认的 Action ,默认为执行配置文件的下一步；你可以通过将Action 设置为 NULL，来屏蔽任何操作；
		/// </summary>
		/// <param name="action"></param>
		public static void SetDefualtEventAction(Action action)
		{
			DefaultEventAction = action;
		}
	}
}