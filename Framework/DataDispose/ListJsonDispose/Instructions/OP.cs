/********************************************************************************
** author：        Liang
** date：          2016-10-28 13:07:05
** description：   具体操作步骤
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using ZF.DataDriveCom.Tools;


namespace ZF.DataDriveCom.DataDispose.Instructions
{
	/// <summary>
	///  此类描述实验中具体的操作步骤；
	/// </summary>
	public class OP : IInstructionDispose
	{
		/// <summary>
		///  这里存储了此类的所有处理指令以及处理函数；
		/// </summary>
		private Dictionary<string, Func<JsonData, bool>> dicts = new Dictionary<string, Func<JsonData, bool>>();


		/// <summary>
		///  处理具体的操作，这里应该可以由客户端来指定先调用自定义的函数库还是先调用组件的函数库；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public bool Dispose(JsonData jsonData)
		{
			CollectFunciton();

			string functionName = jsonData[0].ToString().ToLower();

			// 如果不包含该函数名，则查找用户自定义的函数；如果用户自定义的函数也不能处理，不抛异常；

			if (!dicts.Keys.Contains(functionName)) return false;

			Func<JsonData, bool> func = dicts[functionName];

			return func(jsonData);
		}


		/// <summary>
		/// 如果不包含该函数名，则查找用户自定义的函数；如果用户自定义的函数也不能处理，不抛异常；
		/// </summary>
		/// <param name="mothedName"></param>
		/// <returns></returns>
		public bool Dispose(string mothedName)
		{
			CollectFunciton();

			return dicts.Keys.Contains(mothedName.ToLower());
		}


		/// <summary>
		///  收集该类的所有处理函数；
		/// </summary>
		private void CollectFunciton()
		{
			if (dicts.Count == 0) // 下面一个返回值没有用 TODO
			{
				FunctionObtaining.AddAllObjectFunc<OPFunction, JsonData>((s, f) =>
				{
					if (dicts.ContainsKey(s)) return false;

					dicts.Add(s, f);
					return true;
				});
			}
		}


		/// <summary>
		///  向该类添加处理函数；
		/// </summary>
		public bool AddDisposeFunction(Func<JsonData, bool> func)
		{
			string functionName = func.Method.Name.ToLower();

			if (dicts.ContainsKey(functionName)) return false;

			dicts.Add(functionName, func);

			return true;
		}


		/// <summary>
		///  向该类添加一组处理，若有一个添加错误，则返回 false；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public bool AddDisposeFunction<T>(Func<JsonData, bool> func)
		{
			return FunctionObtaining.AddAllObjectFunc<OPFunction, JsonData>((s, f) =>
			{
				if (dicts.ContainsKey(s)) return false;

				dicts.Add(s, f);
				return true;
			});
		}
	}
}