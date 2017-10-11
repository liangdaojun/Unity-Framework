/********************************************************************************
** author：        Liang
** date：          2016-10-28 11:20:25
** description：   客户端自定义函数服务
** version:        V_1.0.0
*********************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using UnityEngine;
using ZF.DataDriveCom.FunctionLibrarys;
using ZF.DataDriveCom.Tools;


namespace ZF.DataDriveCom.Service
{
	/// <summary>
	///  为客户端自定义的函数提供服务；处理函数用来处理数据，事件函数用来注册事件的回调函数；
	/// </summary>
	public class CustomFunctionService
	{
		#region 注册处理函数，这些函数用来处理数据；

		/// <summary>
		///  注册一个客户端方法；
		/// </summary>
		/// <param name="func"></param>
        public static bool AddDisposeFunction<T>(Func<T, bool> func, bool dontDestroy = false)
		{
			if (CustomFunctionLibrary<T>.ExternalDispose(func.Method.Name)) return false;

			CustomFunctionLibrary<T>.Add(func.Method.Name, func, dontDestroy);

			return true;
		}


        public static bool AddDisposeFunction<T>(Action action, bool dontDestroy = false)
        {
            if (CustomFunctionLibrary<T>.ExternalDisposeA(action.Method.Name)) return false;

            CustomFunctionLibrary<T>.Add(action.Method.Name, action, dontDestroy);

            return true;
        }


		/// <summary>
		///  添加一个对象下，所有符合 Func 类型的方法；
		/// 
		///  T1 为一个对象，T2 为对象的函数的参数；如果需要可以添加 T3为返回类型，这里默认返回类型必须为 bool；
		/// </summary>
		/// <param name="obj"></param>
        public static void AddScriptDisposeFunction<T1, T2>(bool dontDestory = false) where T1 : class
		{
			FunctionObtaining.AddAllObjectFunc<T1, T2>((s, f) =>
			{
				if (CustomFunctionLibrary<T2>.ExternalDispose(s)) return false;

				CustomFunctionLibrary<T2>.Add(s, f, dontDestory);
				return true;
			}) ;


            // TODO .....
		}

		#endregion

		#region 注册 Action 事件；

		/// <summary>
		///  注册一个客户端事件；
		/// </summary>
		/// <param name="func"></param>
		public static bool AddEventAction(Action action, bool dontDestroy = false)
		{
			if (EventFunctionLibrary.ExternalDispose(action.Method.Name)) return false;

			EventFunctionLibrary.Add(action.Method.Name, action, dontDestroy);

			return true;
		}


		/// <summary>
		///  注册一个客户端类中所有符合要求的客户端事件；
		/// </summary>
		public static bool AddScpritEventAction<T>(bool dontDestroy = false) where T : class
		{
			return FunctionObtaining.AddAllObjectAction<T>((s, f) =>
			{
				if (EventFunctionLibrary.ExternalDispose(s)) return false;

				EventFunctionLibrary.Add(s, f, dontDestroy);
				return true;
			});
		}

		#endregion

		#region 注册无参的 Func<bool> 事件；

		/// <summary>
		///  注册一个客户端事件；
		/// </summary>
		/// <param name="func"></param>
		public static bool AddEventFunc(Func<bool> func, bool dontDestroy = false)
		{
			if (EventFunctionLibrary.ExternalDisposeFunc(func.Method.Name)) return false;

			EventFunctionLibrary.Add(func.Method.Name, func, dontDestroy);

			return true;
		}

		/// <summary>
		///  注册一个客户端类中所有符合要求的客户端事件；
		/// </summary>
		public static bool AddScpritEventFunc<T>(bool dontDestroy = false) where T : class
		{
			return FunctionObtaining.AddAllObjectFunc<T>((s, f) =>
			{
				if (EventFunctionLibrary.ExternalDisposeFunc(s)) return false;

				EventFunctionLibrary.Add(s, f, dontDestroy);
				return true;
			});
		}

		#endregion

		#region 注册 Action<string> 事件；

		/// <summary>
		///  注册一个客户端事件；
		/// </summary>
		/// <param name="func"></param>
		public static bool AddEventAction(Action<string> action, bool dontDestroy = false)
		{
			if (EventFunctionLibrary.ExternalDisposeT(action.Method.Name)) return false;

			EventFunctionLibrary.Add(action.Method.Name, action, dontDestroy);

			return true;
		}

		#endregion

		#region 清空 事件 缓存；

		/// <summary>
		///  清空所有可以销毁的函数；
		/// </summary>
		public static void ClearNeedDestroyFunction()
		{
            CustomFunctionLibrary<JsonData>.ClearNeedDestroyFunction();

			EventFunctionLibrary.ClearNeedDestroyFunction();
		}

		#endregion

		/// <summary>
		///  为事件设置默认的 Action ,默认为执行配置文件的下一步；
		/// 
		///  你可以通过将Action 设置为 NULL，来屏蔽任何操作；
		/// </summary>
		/// <param name="action"></param>
		public static void SetDefualtEventAction(Action action)
		{
			EventFunctionLibrary.SetDefualtEventAction(action);
		}
	}
}