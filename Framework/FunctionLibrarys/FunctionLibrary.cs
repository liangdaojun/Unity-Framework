/********************************************************************************
** author：        Liang
** date：          2016-10-24 17:20:01
** description：   提供通用的函数
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LitJson;
using ZF.DataDriveCom.AssetsManagers;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.Service;
using ZF.DataDriveCom.Tools;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  这里提供各种通用的函数，以便于操作资源；
	/// </summary>
	public partial class FunctionLibrary
	{
		/// <summary>
		///  这里缓冲一步 GameObject；
		/// </summary>
		private static Dictionary<string, GameObject> dicts = new Dictionary<string, GameObject>();


		/// <summary>
		///  保存场景中所有 ObjectAssetsBase 类型的 GameObject，并动态收集和注册该方法；
		/// </summary>
		public static bool Scene(string sceneName)
		{
			ScriptAssetsBase script = ScriptAssetsManager.Instance.GetScriptByName(sceneName);

			if (script == null) return false;

			// 清空上一次可以销毁的函数，以释放其内存；

			CustomFunctionService.ClearNeedDestroyFunction();

			Type type = script.GetType();

			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
			                     BindingFlags.Static | BindingFlags.DeclaredOnly;


		    foreach (var methodInfo in type.GetMethods(flags))
		    {

                

		    }


		    foreach (var methodInfo in type.GetMethods(flags))
			{
				// 方法的返回值类型不为 Void ,或者该方法是泛型方法，则跳过；

				if ( methodInfo.Name == "SceneAwake" ||
				    
                    methodInfo.IsGenericMethod || methodInfo.ContainsGenericParameters) continue;

				// 根据参数类型，获取相应的方法； 

				ParameterInfo[] parameters = methodInfo.GetParameters();

                if ( methodInfo.ReturnParameter.ToString() == "Boolean" &&
                    
                    parameters.Length == 1 && parameters[0].ParameterType == typeof(JsonData))
			    {
                    CustomFunctionService.AddDisposeFunction((Func<JsonData,bool>)

                            Delegate.CreateDelegate(typeof(Func<JsonData, bool>), script, methodInfo));

                    continue;
			    }

                if(methodInfo.ReturnParameter.ToString() != "Void" ) continue;

				// 创建一个委托，里使用反射和委托的静态创建方法将 T 中符合要求的方法添加进函数库；

				switch (parameters.Length)
				{
					case 0:

						CustomFunctionService.AddEventAction((Action)
							Delegate.CreateDelegate(typeof (Action), script, methodInfo));

                        CustomFunctionService.AddDisposeFunction<JsonData>((Action)
				            
                            Delegate.CreateDelegate(typeof (Action), script, methodInfo));

						break;

					case 1:

						if (parameters[0].ParameterType != typeof (string)) break;

						CustomFunctionService.AddEventAction((Action<string>)
							Delegate.CreateDelegate(typeof (Action<string>), script, methodInfo));

						break;
				}
			}

			// 调用客户端重写的函数，执行该脚本；

			script.SceneAwake();

			return true;
		}


		/// <summary>
		///  显示一系列物体；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public static bool Display(JsonData jsonData)
		{
			// 先把缓存的物体隐藏并移除；

			foreach (var go in dicts.Values) go.SetActive(false);

			dicts.Clear();

			// 显示所有的物体；

			for (int i = 0; i < jsonData.Count; i++)
			{
				GameObject go = GetGameObject(jsonData[i][0].ToString());

				if (go == null) return false;

				go.SetActive(true);

				dicts.Add(go.name, go);
			}

			return true;
		}


		/// <summary>
		///  改变 UI Label 显示的字体；
		/// </summary>
		/// <param name="labelName"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static bool ChangeUILabelText(string labelName, string context)
		{
			UILabel label = UIAssetsManager.Instance.FindCustomType<UILabel>(labelName);

			if (label != null)
			{
				label.text = context;

				return true;
			}

			return false;
		}


		public static bool PalyAmin(string gameObjectName, string animName)
		{
			GameObject go = GetGameObject(gameObjectName);

			Animation animation = go.GetComponent<Animation>();

			if (!animation)
			{
				Animator animator = go.GetComponent<Animator>();

				if (animator) animator.Play(animName);
#if UNITY_EDITOR
				else throw new Exception(string.Format("{0}物体上没有附加动画！", gameObjectName));
#endif
				return false;
			}

			if (animation.enabled == false)
			{
				if (animation.playAutomatically) return animation.enabled = true;

				animation.enabled = true;
			}

			animation.Play(animName);

			return true;
		}


		public static bool OnFlashing( string gameObjectName )
		{
			GetGameObject(gameObjectName).OnFlashing();

			return true;
		}

		public static bool OffFlashing( string gameObjectName )
		{
			GetGameObject(gameObjectName).OffFlashing();

			return true;
		}


		public static bool Color(string gameObjectName, string color)
		{
			float[] colors = CastString.CastToNumbers<float>(color);

			GetGameObject(gameObjectName).GetComponent<MeshRenderer>().material.color = new Color(colors[0], colors[1], colors[2]);

			return true;
		}


		/// <summary>
		///  根据名字找到一个 UIRect ,并显示它；
		/// </summary>
		/// <param name="UIName"></param>
		/// <returns></returns>
		public static bool ShowUI(string UIName)
		{
			GetUIRect(UIName).gameObject.SetActive(true);

			return true;
		}

		/// <summary>
		///  根据名字隐藏一个 UIRect ,并隐藏它；
		/// </summary>
		/// <param name="UIName"></param>
		/// <returns></returns>
		public static bool HideUI(string UIName)
		{
			GetUIRect(UIName).gameObject.SetActive(false);

			return true;
		}

		/// <summary>
		///  根据名字显示一个物体；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <returns></returns>
		public static bool Show(string gameObjectName)
		{
			GetGameObject(gameObjectName).SetActive(true);

			return true;
		}

		/// <summary>
		///  根据名字隐藏一个物体；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <returns></returns>
		public static bool Hide(string gameObjectName)
		{
			GetGameObject(gameObjectName).SetActive(false);

			return true;
		}

		/// <summary>
		/// 设置一个物体的位置；注意，如果该物体有父物体，则默认设置的是相对位置；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <param name="posStr"></param>
		/// <returns></returns>
		public static bool SetPosition(string gameObjectName, string posStr)
		{
			float[] position = CastString.CastToNumbers<float>(posStr);

			GameObject go = GetGameObject(gameObjectName);

			if (go.transform.parent) go.transform.localPosition = new Vector3(position[0], position[1], position[2]);

			else go.transform.position = new Vector3(position[0], position[1], position[2]);

			return true;
		}


		/// <summary>
		///  根据欧拉角设置一个物体的旋转；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <param name="rotStr"></param>
		/// <returns></returns>
		public static bool SetRotation(string gameObjectName, string rotStr)
		{
			float[] rotation = CastString.CastToNumbers<float>(rotStr);

			GetGameObject(gameObjectName).transform.position = new Vector3(rotation[0], rotation[1], rotation[2]);

			return true;
		}

		/// <summary>
		///  根据名字查找相应的 UIRect；
		/// </summary>
		/// <returns></returns>
		private static UIRect GetUIRect(string UIName)
		{
			return UIAssetsManager.Instance.FindCustomType<UIRect>(UIName);
		}


		/// <summary>
		///  根据名字获得物体；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <returns></returns>
		public static GameObject GetGameObject(string gameObjectName)
		{
			if (String.IsNullOrEmpty(gameObjectName))
			{
#if UNITY_EDITOR
				throw new NullReferenceException("没有为要查找的物体指定名称！");
#endif
				return null;
			}

			if (dicts.ContainsKey(gameObjectName)) return dicts[gameObjectName];

			GameObject go = GameObjectAssetsManager.Instance.FindGameObjectAsset(gameObjectName);

			if (go == null)
			{
#if UNITY_EDITOR
				throw new NullReferenceException(string.Format("没有找到名字为:{0}物体！", gameObjectName));
#endif
				return null;
			}

			return go;
		}


		/// <summary>
		///  根据名字获得物体；并且提取事件参数；注意：如果有多个物体，则返回一个数组；
		/// </summary>
		/// <param name="gameObjectName"></param>
		/// <param name="dontDestroy"></param>
		/// <returns></returns>
		public static GameObject[] GetGameObject(string gameObjectName, out EventParames EventParames)
		{
			gameObjectName = gameObjectName.ToLower();

			EventParames = 0;

			if (gameObjectName.Contains(",~destroy"))
			{
				EventParames |= EventParames.DontDestory;

				gameObjectName = gameObjectName.Replace(",~destroy", "");
			}

			if (gameObjectName.Contains(",~order"))
			{
				EventParames |= EventParames.NoOrder;

				gameObjectName = gameObjectName.Replace(",~order", "");
			}

			if (gameObjectName.Contains(",~setpos"))
			{
				EventParames |= EventParames.DontSetTrue;

				gameObjectName = gameObjectName.Replace(",~setpos", "");
			}

			// 返回所有与事件相关的物体；

			string[] gameObjectNames = gameObjectName.Split(',');

			GameObject[] gameObjects = new GameObject[gameObjectNames.Length];

			for (int i = 0; i < gameObjectNames.Length; i++)
			{
				gameObjects[i] = GetGameObject(gameObjectNames[i]);
			}

			return gameObjects;
		}
	}
}