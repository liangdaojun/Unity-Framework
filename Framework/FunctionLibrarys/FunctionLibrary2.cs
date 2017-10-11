/********************************************************************************
** author：        Liang
** date：          2016-10-24 17:20:01
** description：   配置文件的响应事件
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using ZF.DataDriveCom.Events;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  鼠标事件响应；
	/// </summary>
	public partial class FunctionLibrary
	{
		// 解析 actionName，判断对函数是保留还是移除操作；TODO


		/// <summary>
		///  鼠标左键按下事件；
		/// </summary>
		public static bool OnMouseLeftDown(string gameObjectName, string actionName = null, string parameters = null)
		{
			// 在客户端自定义的函数库中搜寻 action；

			EventParames EventParames;

			GameObject go = GetGameObject(gameObjectName, out EventParames)[0];

			// 如果是有参委托；

			if (!string.IsNullOrEmpty(parameters))
			{
				go.OnMouseLeftDown(EventFunctionLibrary.GetActionT(actionName), parameters, EventParames);

				return true;
			}

			go.OnMouseLeftDown(EventFunctionLibrary.GetAction(actionName), EventParames);

			return true;
		}


		/// <summary>
		///  鼠标右键按下事件；
		/// </summary>
		public static bool OnMouseRightDown(string gameObjectName, string actionName = null, string parameters = null)
		{
			EventParames EventParames;

			GameObject go = GetGameObject(gameObjectName, out EventParames)[0];

			if (!string.IsNullOrEmpty(parameters))
			{
				go.OnMouseRightDown(EventFunctionLibrary.GetActionT(actionName), parameters, EventParames);

				return true;
			}

			go.OnMouseRightDown(EventFunctionLibrary.GetAction(actionName), EventParames);

			return true;
		}

		/// <summary>
		///  鼠标左键抬起；
		/// </summary>
		public static bool OnMouseLeftUp(string gameObjectName, string actionName = null, string parameters = null)
		{
			EventParames EventParames;

			GameObject go = GetGameObject(gameObjectName, out EventParames)[0];

			if (!string.IsNullOrEmpty(parameters))
			{
				go.OnMouseLeftUp(EventFunctionLibrary.GetActionT(actionName), parameters, EventParames);

				return true;
			}

			go.OnMouseLeftUp(EventFunctionLibrary.GetAction(actionName), EventParames);

			return true;
		}


		/// <summary>
		/// 鼠标右键抬起；
		/// </summary>
		public static bool OnMouseRightUp(string gameObjectName, string actionName = null, string parameters = null)
		{
			EventParames EventParames;

			GameObject go = GetGameObject(gameObjectName, out EventParames)[0];

			if (!string.IsNullOrEmpty(parameters))
			{
				go.OnMouseRightUp(EventFunctionLibrary.GetActionT(actionName), parameters, EventParames);

				return true;
			}

			go.OnMouseRightUp(EventFunctionLibrary.GetAction(actionName), EventParames);

			return true;
		}

		/// <summary>
		///  鼠标拖拽；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <param name="actionName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static bool OnMouseDrag(string gameObjectName, string actionName = null, string parameters = null)
		{
			EventParames EventParames;

			GameObject[] gameObjects = GetGameObject(gameObjectName, out EventParames);

			switch (gameObjects.Length)
			{
				case 1:

					if (!string.IsNullOrEmpty(parameters))
					{
						gameObjects[0].OnMouseDrag(EventFunctionLibrary.GetActionT(actionName), parameters, EventParames);

						return true;
					}

					gameObjects[0].OnMouseDrag(EventFunctionLibrary.GetAction(actionName), EventParames);

					break;

				case 2:

					if (!string.IsNullOrEmpty(parameters))
					{
						gameObjects[0].OnMouseDrag(gameObjects[1], EventFunctionLibrary.GetActionT(actionName), parameters, EventParames);

						return true;
					}

					if (actionName == null)
					{
						gameObjects[0].OnMouseDrag(gameObjects[1], null, EventParames);
					}
					else
					{
						gameObjects[0].OnMouseDrag(gameObjects[1], EventFunctionLibrary.GetAction(actionName), EventParames);
					}

					break;
			}

			return true;
		}


		/// <summary>
		///  鼠标双击事件；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <param name="actionName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static bool OnMouseDoubleClick(string gameObjectName, string actionName = null, string parameters = null)
		{
			EventParames EventParames;

			GameObject go = GetGameObject(gameObjectName, out EventParames)[0];

			if (!string.IsNullOrEmpty(parameters))
			{
				go.OnMouseDoubleClick(EventFunctionLibrary.GetActionT(actionName), parameters, EventParames);

				return true;
			}

			go.OnMouseDoubleClick(EventFunctionLibrary.GetAction(actionName), EventParames);

			return true;
		}


		/// <summary>
		///  销毁某个物体上的某个事件；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <param name="eventName"></param>
		/// <returns></returns>
		public static bool OffEvent(string gameObjectName, string eventName)
		{
			GetGameObject(gameObjectName).OffEvent(eventName);

			return true;
		}
	}
}