/********************************************************************************
** author：        Liang
** date：          2016-10-28 15:16:11
** description：   获取特定的函数
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Reflection;


namespace ZF.DataDriveCom.Tools
{
	/// <summary>
	///  通过函数签名来获取制定类型的函数
	/// </summary>
	public class FunctionObtaining
	{
		/// <summary>
		///  将函数名和自身添加进去；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <param name="getInherit"></param>
		/// <returns></returns>
		public static bool AddAllObjectAction<T>(Func<string, Action, bool> func, bool getInherit = false) where T : class
		{
			bool isSuccess = false;

			foreach (var methodInfo in GetMethods<T>())
			{
				if (methodInfo.IsGenericMethod || methodInfo.ContainsGenericParameters) continue;

				/*// 方法的返回值类型不为 null ,则跳过；

				if ( methodInfo.ReturnType != typeof(Nullable) ) continue;*/

				// 若方法有一个 string 型的参数则满足要求；

				ParameterInfo[] parameters = methodInfo.GetParameters();

				if (parameters.Length != 0) continue;

				// 这里使用反射和委托的静态创建方法将 T 中符合要求的方法添加进 dictionary List； 

				isSuccess = func(methodInfo.Name.ToLower(), (Action) Delegate.CreateDelegate(
					typeof (Action), methodInfo.IsStatic ? null : Activator.CreateInstance<T>(), methodInfo));
			}

			return isSuccess;
		}

		/// <summary>
		///  获取 T 类型中的所有的 Func<bool>方法；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <param name="getInherit"></param>
		/// <returns></returns>
		public static bool AddAllObjectFunc<T>(Func<string, Func<bool>, bool> func, bool getInherit = false) where T : class
		{
			bool isSuccess = false;

			foreach (var methodInfo in GetMethods<T>())
			{
				if (methodInfo.IsGenericMethod || methodInfo.ContainsGenericParameters) continue;

				// 若方法有一个 string 型的参数则满足要求；

				ParameterInfo[] parameters = methodInfo.GetParameters();

				// 这里使用反射和委托的静态创建方法将 T 中符合要求的方法添加进 dictionary List；

				if (methodInfo.ReturnType == typeof (bool) && parameters.Length == 0)
				{
					if (!func(methodInfo.Name.ToLower(), (Func<bool>) Delegate.CreateDelegate(
						typeof (Func<bool>), methodInfo.IsStatic ? null : Activator.CreateInstance<T>(), methodInfo))) return false;
				}
			}

			return isSuccess;
		}


		/// <summary>
		///  添加一个对象下，所有符合 Func 类型的方法；
		/// </summary>
		/// <param name="obj"></param>
		public static bool AddAllObjectFunc<T1, T2>(Func<string, Func<T2, bool>, bool> func, bool getInherit = false)
			where T1 : class
		{
			foreach (var methodInfo in GetMethods<T1>())
			{
				// 不支持泛型方法，若为泛型方法，则跳过；

				if (methodInfo.IsGenericMethod || methodInfo.ContainsGenericParameters) continue;

				// 方法的返回值类型不为 bool ,则跳过；

				if (methodInfo.ReturnType != typeof (bool)) continue;

				// 若方法有一个 string 型的参数则满足要求；

				ParameterInfo[] parameters = methodInfo.GetParameters();

				if (parameters.Length != 1) continue;

				// 这里使用反射和委托的静态创建方法将 T 中符合要求的方法添加进 dictionary List； 

				if (parameters.Length == 1 && parameters[0].ParameterType == typeof (T2))
				{
					bool success = func(methodInfo.Name.ToLower(), (Func<T2, bool>) Delegate.CreateDelegate(
						typeof (Func<T2, bool>), methodInfo.IsStatic ? null : Activator.CreateInstance<T1>(), methodInfo));

					if (success == false) return false;
				}
			}

			return true;
		}


		/// <summary>
		///  getInherit 为 true，则搜索继承的方法，否则，只从当前类搜索；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="getInherit"></param>
		/// <returns></returns>
		public static MethodInfo[] GetMethods<T>(bool getInherit = false) where T : class
		{
			Type type = typeof (T);

			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
			                     BindingFlags.Static | (getInherit == true ? BindingFlags.Default : BindingFlags.DeclaredOnly);

			return type.GetMethods(flags);
		}
	}
}