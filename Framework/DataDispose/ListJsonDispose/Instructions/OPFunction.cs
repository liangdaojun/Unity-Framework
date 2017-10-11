/********************************************************************************
** author：       Liang
** date：         2016-10-29 22:58:49
** description：  组件内部定义的处理命令的函数
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using LitJson;
using ZF.DataDriveCom.FunctionLibrarys;

namespace ZF.DataDriveCom.DataDispose.Instructions
{
	/// <summary>
	/// 组件内部定义的处理命令的函数，这里设计成偏类以便于扩充指令；
	/// 
	/// 这里的函数，操作函数库；
	/// </summary>
	public partial class OPFunction
	{
		/// <summary>
		///  显示提示消息
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool Tip(JsonData jsonData)
		{
			return FunctionLibrary.ChangeUILabelText(jsonData[1].ToString(), jsonData[2].ToString());
		}


		private bool ShowUI(JsonData jsonData)
		{
			return FunctionLibrary.ShowUI(jsonData[1].ToString());
		}

		private bool HideUI(JsonData jsonData)
		{
			return FunctionLibrary.HideUI(jsonData[1].ToString());
		}


		/// <summary>
		///  使物体高光显示
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool OnFlashing( JsonData jsonData )
		{
			return FunctionLibrary.OnFlashing(jsonData[1].ToString());
		}


		private bool OffFlashing(JsonData jsonData)
		{
			return FunctionLibrary.OffFlashing(jsonData[1].ToString());
		}


		private bool Color(JsonData jsonData)
		{
			return FunctionLibrary.Color(jsonData[1].ToString(), jsonData[2].ToString());
		}


		private bool Show(JsonData jsonData)
		{
			return FunctionLibrary.Show(jsonData[1].ToString());
		}

		private bool Hide(JsonData jsonData)
		{
			return FunctionLibrary.Hide(jsonData[1].ToString());
		}


		/// <summary>
		///  播放动画
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool Anim(JsonData jsonData)
		{
			return FunctionLibrary.PalyAmin(jsonData[1].ToString(), jsonData[2].ToString());
		}

		/// <summary>
		///  设置物体的位置；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool Pos(JsonData jsonData)
		{
			return FunctionLibrary.SetPosition(jsonData[1].ToString(), jsonData[2].ToString());
		}

		/// <summary>
		///  设置物体的旋转；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool Rot(JsonData jsonData)
		{
			return FunctionLibrary.SetRotation(jsonData[1].ToString(), jsonData[2].ToString());
		}

        
	}
}