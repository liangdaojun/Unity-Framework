/********************************************************************************
** author：       Liang
** date：         2016-11-20 22:53:13
** description：  操作步骤中的 事件处理
** version:       V_1.0.0
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
	///  定义外部的事件操作；
	/// </summary>
	public class Event : IInstructionDispose
	{
		/// <summary>
		///  这里存储了此类的所有处理指令以及处理函数；
		/// </summary>
		private Dictionary<string, Func<JsonData, bool>> dicts = new Dictionary<string, Func<JsonData, bool>>();


		public bool Dispose(JsonData jsonData)
		{
			CollectFunciton();

			string functionName = jsonData[0].ToString().ToLower();

			// 如果不包含该函数名，则查找用户自定义的函数；如果用户自定义的函数也不能处理，不抛异常；

			if (!dicts.Keys.Contains(functionName)) return false;

			// 执行事件；

			dicts[functionName](jsonData);

			return true;
		}


		/// <summary>
		/// 如果不包含该函数名，则查找用户自定义的函数；如果用户自定义的函数也不能处理，不抛异常；
		/// </summary>
		/// <param name="mothedName"></param>
		/// <returns></returns>
		public bool Dispose(string mothedName)
		{
			CollectFunciton();

			/*Debug.Log(dicts.Count);

			foreach (var dict in dicts)
			{
				Debug.Log(dict);
			}*/

			return dicts.Keys.Contains(mothedName.ToLower());
		}


		/// <summary>
		///  收集该类的所有处理函数；
		/// </summary>
		private void CollectFunciton()
		{
			if (dicts.Count == 0)
			{
				FunctionObtaining.AddAllObjectFunc<EventFunction, JsonData>((s, f) =>
				{
					if (dicts.ContainsKey(s)) return false;

					dicts.Add(s, f);

					return true;
				});
			}
		}
	}
}