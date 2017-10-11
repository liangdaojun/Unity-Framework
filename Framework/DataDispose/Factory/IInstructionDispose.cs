/********************************************************************************
** author：        Liang
** date：          2016-10-24 12:52:15
** description：   命令处理的接口
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace ZF.DataDriveCom.DataDispose
{
	/// <summary>
	///  所有能处理数据的类的继承这个接口
	/// </summary>
	public interface IInstructionDispose
	{
		/// <summary>
		///  每个处理都将传递 JsonData,若处理成功，返回 true；否则，返回 false；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		bool Dispose(JsonData jsonData);

		/// <summary>
		///  根据一个方法的方法名，判断该方法是否存在；
		/// </summary>
		/// <param name="methodName"></param>
		/// <returns></returns>
		bool Dispose(string methodName);
	}
}