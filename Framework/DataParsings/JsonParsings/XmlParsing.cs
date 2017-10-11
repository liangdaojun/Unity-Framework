/********************************************************************************
** author：       Liang
** date：         2016-10-29 21:01:10
** description：  Xml 的数据解析
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.DataParsings
{
	/// <summary>
	///  对Xml文件进行解析
	/// </summary>
	public class XmlParsing : IDataParsing
	{
		public void Parsing<T>(T data, bool dispose = false) where T : class
		{
			throw new System.NotImplementedException();
		}
	}
}