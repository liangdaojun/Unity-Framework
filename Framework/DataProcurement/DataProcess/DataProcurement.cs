/********************************************************************************
** author：        Liang
** date：          2016-10-14 16:59:11
** description：   数据获取
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using LitJson;


namespace ZF.DataDriveCom.DataProcurements
{
	/// <summary>
	///  数据获取的抽象类
	/// </summary>
	public sealed class DataProcurement : DataProcurementBase
	{
		/// <summary>
		///  缓存加载的数据
		/// </summary>
		public static string jsonStr;


		/// <summary>
		///  从网页获取数据，并缓存到本地
		/// </summary>
		public override T GetDataFromWebHost<T>(string fileName, string url = null)
		{
			// 如果 url 不空，则从 Web端加载数据 , 否则加载本地。 如果 Web端加载异常，也从本地加载 TODO

			if (!String.IsNullOrEmpty(url))
			{
				// 对 url 做一些规则匹配 TODO

				Application.ExternalCall("DataProcurement," + fileName + "," + url, "Data_OK");

				// 进行网页通讯，获取数据

				// return   TODO
			}

			return GetDataFromLocalHost<T>(fileName);
		}


		/// <summary>
		///  从本地读取 Json 文件， 使用的是 LitJSon 
		/// </summary>
		public override T GetDataFromLocalHost<T>(string fileName)
		{
			TextAsset txt = Resources.Load<TextAsset>(fileName);

			if (txt == null) throw new Exception("缺少配置文件 " + fileName);

			System.Object obj = null;

			// T 的类型不同，操作也不同

			if (typeof (T) == typeof (string))
			{
				obj = txt.text;
			}
			else if (typeof (T) == typeof (JsonData))
			{
				obj = JsonMapper.ToObject(txt.text);
			}
			else if (typeof (T) == typeof (object))
			{
				obj = txt;
			}

			// 判断转换能否成功

			T t;

			try
			{
				t = (T) obj;
			}
			catch (InvalidCastException e)
			{
				throw e;
			}
			catch (Exception e)
			{
				throw e;
			}

			return t;
		}
	}
}