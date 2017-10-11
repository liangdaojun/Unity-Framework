/********************************************************************************
** author：       Liang
** date：         2016-11-27 17:57:51
** description：  客户端非托管事件服务
** version:       V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.Tools;


namespace ZF.DataDriveCom.Service
{
	/// <summary>
	///  这里提供Unity 里的通用事件；
	/// </summary>
	public class UnityEventService
	{
		private static GameObject null_GO = new GameObject("EventService");


		static UnityEventService()
		{
			null_GO.hideFlags = HideFlags.HideInInspector;
		}


		/// <summary>
		///  更换鼠标图片；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="texture"></param>
		/// <returns></returns>
		public static bool ChangeMouseCursor(Texture texture = null)
		{
			if (texture == null)
			{
				GameObject.Destroy(Camera.main.gameObject.GetComponent<ChangeMouseCursor>());

				Screen.showCursor = true;
			}
			else
			{
				Screen.showCursor = false;

				Camera.main.gameObject.AddComponent<ChangeMouseCursor>().SetMouseTexture(texture);
			}

			return true;
		}


		/// <summary>
		/// Unity的等待事件，单位秒；
		/// </summary>
		/// <param name="time"></param>
		/// <param name="action"></param>
		public static void OnWaitSecond(float time, Action action = null)
		{
			null_GO.AddComponent<OnWaitSecond>().SetAction(time, action);
		}

		/// <summary>
		/// Unity的等待事件，单位秒；
		/// </summary>
		/// <param name="time"></param>
		/// <param name="action"></param>
		/// <param name="parameters"></param>
		public static void OnWaitSecond(float time, Action<string> action, string parameters)
		{
			null_GO.AddComponent<OnWaitSecond>().SetAction(time, action, parameters);
		}


		/// <summary>
		///  每帧检测一个Func<bool>函数，若返回 true ,则停止检测；
		/// </summary>
		/// <param name="func"></param>
		public static void OnLoopEvent(Func<bool> func)
		{
			null_GO.AddComponent<OnLoopEvent>().SetFunc(null_GO, func, false);
		}


		public static void OnUpate(Func<bool> func)
		{
			//null_GO.AddComponent<OnUpdate>().SetFunc(func);
		}


		/// <summary>
		///  从摄像机，创建射线，并返回射线击中的物体；
		/// </summary>
		/// <returns></returns>
		public static GameObject RayHit()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo))
			{
				return hitInfo.collider.gameObject;
			}

			return null;
		}

		/// <summary>
		///  判断物体是否被从摄像机发出的射线击中；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <returns></returns>
		public static bool RayHit(GameObject gameObject_Ray)
		{
			return RayHit() == gameObject_Ray;
		}


		/// <summary>
		///  从一个物体发出射线，并返回 RaycastHit的实例；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static RaycastHit RaycastHit(GameObject gameObject_Ray, Vector3 direction)
		{
			Ray ray = new Ray(gameObject_Ray.transform.position, direction);

			RaycastHit hit;

			if (!Physics.Raycast(ray, out hit)) return default(RaycastHit);

			return hit;
		}


		/// <summary>
		///  从一个物体发出射线，并返回所击中的物体；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static GameObject RayHit(GameObject gameObject_Ray, Vector3 direction)
		{
			if (RaycastHit(gameObject_Ray, direction).collider == null) return null;

			return RaycastHit(gameObject_Ray, direction).collider.gameObject;
		}


		/// <summary>
		///  返回从一个物体发出的射线所击中的点；注意：若没有击中任何物体，返回物体自身的坐标；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3 RayHitPoint(GameObject gameObject_Ray, Vector3 direction)
		{
			return RaycastHit(gameObject_Ray, direction).point;
		}


		/// <summary>
		///  判断从一个物体发出的射线是否击中另一个指定的物体；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <param name="gameObject_collider"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static bool RayHit(GameObject gameObject_Ray, GameObject gameObject_collider, Vector3 direction)
		{
			if ( RayHit( gameObject_Ray , direction ) == gameObject_collider ) return true;

			return false;
		}


		/// <summary>
		///  判断两个物体是否相接触；
		/// </summary>
		/// <param name="go1"></param>
		/// <param name="go2"></param>
		/// <returns></returns>
		public static bool Intersect(GameObject go1, GameObject go2)
		{
			if (!go1.GetComponent<Collider>() || !go2.GetComponent<Collider>())
			{
				throw new Exception("必须保证两个物体都有Collider");
			}

			return go1.GetComponent<Collider>().bounds.Intersects(go2.GetComponent<Collider>().bounds);
		}
	}
}