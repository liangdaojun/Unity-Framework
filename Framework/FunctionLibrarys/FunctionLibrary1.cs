/********************************************************************************
** author：        Liang
** date：          2016-10-24 17:20:01
** description：   配置文件的响应事件
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using ZF.DataDriveCom.AssetsManagers;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.Service;
using ZF.DataDriveCom.Tools;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  Unity 普通事件；
	/// </summary>
	public partial class FunctionLibrary
	{
		/// <summary>
		///  Unity的等待事件，单位秒；
		/// </summary>
		/// <param name="waitTime"></param>
		/// <param name="actionName"></param>
		/// <returns></returns>
		public static bool OnWaitSecond(string waitTime, string actionName = null, string parameters = null)
		{
			float time = CastString.CastToNumbers<float>(waitTime)[0];

			if (string.IsNullOrEmpty(parameters))
				UnityEventService.OnWaitSecond(time, EventFunctionLibrary.GetAction(actionName));

			else UnityEventService.OnWaitSecond(time, EventFunctionLibrary.GetActionT(actionName), parameters);

			return true;
		}


		public static bool OnUpdate(string gameObejctName, string actionName)
		{
			EventParames EventParames;

			GetGameObject(gameObejctName, out EventParames)[0].OnUpdate(EventFunctionLibrary.GetAction(actionName), EventParames);

			return true;
		}


		/// <summary>
		///  保存场景数据；
		/// </summary>
		/// <returns></returns>
		public static bool OnRecord()
		{
			UndoRedoManager.Instance.RecordAllGameObjectsInfo();

			return true;
		}


		/// <summary>
		///  执行 Undo 事件；
		/// </summary>
		/// <returns></returns>
		public static bool OnUndo(string gameObjectName = null)
		{
			UndoRedoManager.Instance.UndoAllGameObjectsInfo();

			return true;
		}

		/// <summary>
		///  执行 Redo 事件；
		/// </summary>
		/// <returns></returns>
		public static bool OnRedo(string gameObjectName = null)
		{
			UndoRedoManager.Instance.RedoAllGameObjectsInfo();

			return true;
		}
	}
}