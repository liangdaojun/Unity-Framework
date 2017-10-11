/********************************************************************************
** author：        Liang
** date：          2016-11-30 15:01:08
** description：   Unity 的事件的函数的功能类
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using LitJson;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.DataDispose.Instructions
{
	/// <summary>
	///  Unity 普通事件；
	/// </summary>
	public partial class EventFunction
	{
		/// <summary>
		///   Unity的等待事件，单位秒；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool OnWaitSecond(JsonData jsonData)
		{
			switch (jsonData.Count)
			{
				case 1:

					throw new Exception("参数不匹配！");

				case 2:

					return FunctionLibrary.OnWaitSecond(jsonData[1].ToString());

				case 3:

					return FunctionLibrary.OnWaitSecond(jsonData[1].ToString(), jsonData[2].ToString());

				case 4:

					return FunctionLibrary.OnWaitSecond(jsonData[1].ToString(), jsonData[2].ToString(), jsonData[3].ToString());
			}

			return false;
		}


		/// <summary>
		///  Update 事件；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool OnUpdate(JsonData jsonData)
		{
			return FunctionLibrary.OnUpdate(jsonData[1].ToString(), jsonData[2].ToString());
		}

		/*public bool FixedUpdate(JsonData jsonData)
		{
			return true;
		}

		public bool LateUpdate(JsonData josnData)
		{
			return true;
		}

		public bool OnLoop(JsonData jsonData)
		{
			return true;
		}*/


		/// <summary>
		///  保存场景数据；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool OnRecord(JsonData jsonData)
		{
			return FunctionLibrary.OnRecord();
		}


		/// <summary>
		///  Undo 场景；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool OnUndo(JsonData jsonData)
		{
			return FunctionLibrary.OnUndo();
		}

		/// <summary>
		///  Redo 场景；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool OnRedo(JsonData jsonData)
		{
			return FunctionLibrary.OnRedo();
		}
	}
}