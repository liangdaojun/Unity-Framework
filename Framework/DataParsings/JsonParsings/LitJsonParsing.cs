/********************************************************************************
** author：        Liang
** date：          2016-10-31 15:57:43
** description：   LitJson 的数据解析类；
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using ZF.DataDriveCom.DataDispose;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.DataParsings
{
	/// <summary>
	///  定义解析数据；
	/// </summary>
	public struct JsonItem
	{
		public bool dispose;

		public string methodName;

		public JsonData JsonData;

		public override string ToString()
		{
			return String.Format("dispose={0},  mothedName={1},  JsonData={2}", dispose, methodName, JsonData.ToJson());
		}
	}


	/// <summary>
	///  LitJson 的数据解析类，对 LitJson 进行调用前预处理；
	/// 
	///  根据外接提供的关键字，进行 Json 文件的解析 ，这些关键就是Json 文件提供的；
	/// 
	///  这里调用下一层服务，即 Json 的数据处理，为对下一层解耦，这里使用一个接口；
	/// 
	///  先获的一级命令，根据反射获得相应的处理类，如果不存在该类，向下执行，不抛异常；
	/// </summary>
	public partial class LitJsonParsing : IDataParsing
	{
		#region 单例

		private static LitJsonParsing _instance;

		private LitJsonParsing()
		{
		}

		public static LitJsonParsing Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LitJsonParsing();

					return _instance;
				}

				return _instance;
			}
		}

		#endregion

		/// <summary>
		/// 定义要分割的指令；
		/// </summary>
		private static string[] Instructions;

		/// <summary>
		///  保存当前的操作步骤；这里index 从1 开始，0 为Json文件的自描述数据；
		/// </summary>
		private static int index ;

		/// <summary>
		/// 将 Json 文件的数据进行转化，以便于读取和定位；
		/// </summary>
		public static List<JsonItem> JsonItemList = new List<JsonItem>();


		/// <summary>
		/// 对 LitJson 进行调用前处理；
		/// </summary>
		/// <param name="json_Str"></param>
		public void Parsing<T>(T data, bool dispose = false) where T : class
		{
			// 转化为 JsonData ；

			JsonData jsonData = JsonParse(data);

			// 预处理 LitJson ，读取 LitJson 的自描述文件；

			PreParsing(jsonData[0]);

			// 解析 LitJson；

			DisposeJsonData(jsonData);

			/*foreach (var jsonItem in JsonItemList)
			{
				Debug.Log(jsonItem);
			}*/
		}


		/// <summary>
		///  解析之前，对 Json 数据进行一步预处理；
		/// </summary>
		/// <param name="jsonData"></param>
		private void PreParsing(JsonData jsonData)
		{
			Instructions = new string[jsonData.Count];

			for (int i = 0; i < jsonData.Count; i++)
			{
				Instructions[i] = jsonData[i][0].ToString().ToLower();
			}
		}


		/// <summary>
		///  遍历，遍历到指令就看能不能处理；先按组件能处理的和不能处理的分成两组；
		/// 
		///  DisposeJsonData  和 DisposeJsonObject 两个方法相互调用，形成递归；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private void DisposeJsonData(JsonData jsonData)
		{
			if (jsonData.IsArray)
			{
				for (int i = 0; i < jsonData.Count; i++)
				{
					if (jsonData[i].IsArray)
					{
						DisposeJsonData(jsonData[i]); // 是数组就往下递归；
					}
					else
					{
						DisposeJsonObject(jsonData[i]); // 处理对象；
					}
				}
			}
			else
			{
				DisposeJsonObject(jsonData); // 处理对象；
			}
		}


		/// <summary>
		///  处理 Json 对象；判断该指令能否处理； 如果能处理，则实例化该类，并且收集该类的方法；
		///  
		///  根据 key 来反射类，根据 jsonData[key]来查找该方法； 注意：不区分大小写；
		/// </summary>
		/// <param name="jsonData"></param>
		private void DisposeJsonObject(JsonData jsonData)
		{
			foreach (string key in ((IDictionary) jsonData).Keys)
			{
				// 如果条件满足，则继续向下递归；

				if (Continue(key, jsonData[key]))
				{
					DisposeJsonData(jsonData[key]);
				}
				else
				{
                    // 是否能够处理该类数据；

					bool disposed = LitJsonInstructionFactory.CreateDispose(key, jsonData[key].ToString());

					// 非对象或者数组，则将该对象整个加进列表；跳出循环；

					if (!jsonData[key].IsArray && !jsonData[key].IsObject)
					{
						AddJsonItem(disposed, key.ToLower(), jsonData);

						break;
					}

					// 否则，将对象值加入列表；

					AddJsonItem(disposed, key.ToLower(), jsonData[key]);
				}
			}
		}


		/// <summary>
		///  决定继续遍历一个对象或者跳过该对象；
		/// 
		///  如果预定义指令集包含该指令，且它的 Value 是数组或对象，则返回true;
		/// </summary>
		private bool Continue(string key, JsonData jsonData)
		{
			return Instructions.Contains(key.ToLower()) && (jsonData.IsArray || jsonData.IsObject);
		}


		/// <summary>
		///  将该步骤添加进步骤列表；
		/// </summary>
		/// <param name="dispose"></param>
		/// <param name="Key"></param>
		/// <param name="jsonData"></param>
		private void AddJsonItem(bool dispose, string Key, JsonData jsonData)
		{
			JsonItem jsonItem = new JsonItem();

			jsonItem.dispose = dispose;

			jsonItem.methodName = Key;

			jsonItem.JsonData = jsonData;

			JsonItemList.Add(jsonItem);
		}


		/// <summary>
		///  对数据进一步确认
		/// </summary>
		/// <param name="json"></param>
		private static JsonData JsonParse<T>(T json)
		{
			// 这里要提供很多的异常检测 TODO

			if (typeof (T) == typeof (string))
			{
				return JsonMapper.ToObject((string) (object) json);
			}
			else
			{
				return (JsonData) (object) json;
			}
		}
	}
}