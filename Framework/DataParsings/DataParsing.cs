/********************************************************************************
** author：       Liang
** date：         2016-10-29 20:54:51
** description：  对各种文件进行解析
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.DataParsings
{
	/// <summary>
	///  这里文件类型基本上是确定的，为提高效率，把对各种文件进行解析的类设计成简单工厂模式；
	/// </summary>
	public class DataParsing
	{
		/// <summary>
		///  泛型 T 为源文件流类型，dataType是文件类型，dispose为是否进行处理；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataType"></param>
		/// <param name="data"></param>
		/// <param name="dispose"></param>
		public static void Parsing<T>(DataType dataType, T data, bool dispose = false) where T : class
		{
			IDataParsing dataParsing;

			switch (dataType)
			{
				case DataType.Json:

					dataParsing = LitJsonParsing.Instance;

					break;

				case DataType.Xml:

					dataParsing = new XmlParsing();

					break;

				case DataType.String:

					dataParsing = new StringParsing();

					break;

				default:
					return;
			}

			dataParsing.Parsing(data, dispose);
		}
	}
}