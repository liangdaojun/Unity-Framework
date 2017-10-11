/********************************************************************************
** author：        Liang
** date：          2016-11-21 09:06:39
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
	///  鼠标事件；	先解析 JsonData, 找到相应的 GameObject ,并为其添加事件；
	/// </summary>
	public partial class EventFunction
	{
		// 优化这里 TODO


		/// <summary>
		///  鼠标左键按下事件；
		/// </summary>
		public bool OnMouseLeftDown(JsonData jsonData)
		{
			switch (jsonData.Count)
			{
				case 1:

					throw new Exception("配置文件参数不对！");

				case 2:

					return FunctionLibrary.OnMouseLeftDown(jsonData[1].ToString());

				case 3:

					return FunctionLibrary.OnMouseLeftDown(jsonData[1].ToString(), jsonData[2].ToString());

				case 4:

					return FunctionLibrary.OnMouseLeftDown(jsonData[1].ToString(), jsonData[2].ToString(), jsonData[3].ToJson());
			}

			return false;
		}


		/// <summary>
		///  鼠标右键按下事件；
		/// </summary>
		public bool OnMouseRightDown(JsonData jsonData)
		{
			switch (jsonData.Count)
			{
				case 1:

					throw new Exception("配置文件参数不对！");

				case 2:

					return FunctionLibrary.OnMouseRightDown(jsonData[1].ToString());

				case 3:

					return FunctionLibrary.OnMouseRightDown(jsonData[1].ToString(), jsonData[2].ToString());

				case 4:

					return FunctionLibrary.OnMouseRightDown(jsonData[1].ToString(), jsonData[2].ToString(), jsonData[3].ToJson());
			}

			return false;
		}

		/// <summary>
		///  鼠标左键抬起；
		/// </summary>
		public bool OnMouseLeftUp(JsonData jsonData)
		{
			switch (jsonData.Count)
			{
				case 1:

					throw new Exception("配置文件参数不对！");

				case 2:

					return FunctionLibrary.OnMouseLeftUp(jsonData[1].ToString());

				case 3:

					return FunctionLibrary.OnMouseLeftUp(jsonData[1].ToString(), jsonData[2].ToString());

				case 4:

					return FunctionLibrary.OnMouseLeftUp(jsonData[1].ToString(), jsonData[2].ToString(), jsonData[3].ToJson());
			}

			return false;
		}


		/// <summary>
		/// 鼠标右键抬起；
		/// </summary>
		public bool OnMouseRightUp(JsonData jsonData)
		{
			switch (jsonData.Count)
			{
				case 1:

					throw new Exception("配置文件参数不对！");

				case 2:

					return FunctionLibrary.OnMouseRightUp(jsonData[1].ToString());

				case 3:

					return FunctionLibrary.OnMouseRightUp(jsonData[1].ToString(), jsonData[2].ToString());

				case 4:

					return FunctionLibrary.OnMouseRightUp(jsonData[1].ToString(), jsonData[2].ToString(), jsonData[3].ToJson());
			}

			return false;
		}


		/// <summary>
		///  鼠标拖拽；本方法可能有多个参数；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public bool OnMouseDrag(JsonData jsonData)
		{
			switch (jsonData.Count)
			{
				case 1:

					throw new Exception("配置文件参数不对！");

				case 2:

					return FunctionLibrary.OnMouseDrag(jsonData[1].ToString());

				case 3:

					return FunctionLibrary.OnMouseDrag(jsonData[1].ToString(), jsonData[2].ToString());

				case 4:

					return FunctionLibrary.OnMouseDrag(jsonData[1].ToString(), jsonData[2].ToString(), jsonData[3].ToJson());
			}

			return false;
		}


		/// <summary>
		///  鼠标双击事件；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public bool OnMouseDoubleClick(JsonData jsonData)
		{
			switch (jsonData.Count)
			{
				case 1:

					throw new Exception("配置文件参数不对！");

				case 2:

					return FunctionLibrary.OnMouseDoubleClick(jsonData[1].ToString());

				case 3:

					return FunctionLibrary.OnMouseDoubleClick(jsonData[1].ToString(), jsonData[2].ToString());

				case 4:

					return FunctionLibrary.OnMouseDoubleClick(jsonData[1].ToString(), jsonData[2].ToString(), jsonData[3].ToJson());
			}

			return true;
		}

		/// <summary>
		///  销毁某个物体上的某个事件；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public bool OffEvent(JsonData jsonData)
		{
			return FunctionLibrary.OffEvent(jsonData[1].ToString(), jsonData[2].ToString());
		}
	}
}