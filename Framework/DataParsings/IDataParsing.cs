/********************************************************************************
** author：        Liang
** date：          2016-10-21 16:32:21
** description：   数据解析接口
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.DataParsings
{
	public enum DataType
	{
		String,

		Xml,

		Json,
	}


	/// <summary>
	///  数据解析的
	/// </summary>
	public interface IDataParsing
	{
		/// <summary>
		///  数据解析
		/// </summary>
		void Parsing<T>(T data, bool dispose = false) where T : class;
	}
}