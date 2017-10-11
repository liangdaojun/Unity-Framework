/********************************************************************************
** author：       Liang
** date：         2016-10-29 21:03:02
** description：  对字符串进行解析
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.DataParsings
{
	/// <summary>
	///  对字符串文件进行解析
	/// </summary>
	public class StringParsing : IDataParsing
	{
		public void Parsing<T>(T data, bool dispose = false) where T : class
		{
			throw new System.NotImplementedException();
		}
	}
}