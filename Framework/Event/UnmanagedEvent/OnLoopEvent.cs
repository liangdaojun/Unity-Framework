/********************************************************************************
** author：       Liang
** date：         2016-11-27 08:58:04
** description：  为物体添加循环检测事件
** version:       V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.FunctionLibrarys;

namespace ZF.DataDriveCom.Events
{
	/// <summary>
	///  某物体循环执行某一函数并检测某一条件，当该条件满足时，销毁自身；
	/// </summary>
	public class OnLoopEvent : MonoBehaviour
	{
		private GameObject go;

		private Func<bool> func;

		private bool rayHited = true;


		/// <summary>
		///  设置参数；
		/// </summary>
		/// <param name="go"></param>
		/// <param name="func"></param>
		/// <param name="rayHited"></param>
		public void SetFunc(GameObject go, Func<bool> func, bool rayHited = true)
		{
			this.go = go;

			this.func = func;

			this.rayHited = rayHited;
		}


		private void Update()
		{
			if (rayHited && go.RayHited() && func())
			{
				Destroy(this);
			}
			else if (!rayHited && func())
			{
				Destroy(this);
			}
		}
	}
}