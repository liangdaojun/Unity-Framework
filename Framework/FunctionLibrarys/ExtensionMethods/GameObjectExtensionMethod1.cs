/********************************************************************************
** author：        Liang
** date：          2016-11-14 15:09:46
** description：   GameObject 扩展的一些非托管的常用事件
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.Events.GameObjectEvent;
using ZF.DataDriveCom.Service;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  GameObject 的公共方法；
	/// </summary>
	public static partial class GameObjectExtensionMethod
	{
		/// <summary>
		///  添加帧监听事件；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="action"></param>
		/// <param name="EventParames"></param>
		public static void OnUpdate(this GameObject gameObject, Action action, EventParames EventParames = 0)
		{
			EventManager.SetFunc(EventsType.OnUpdate, new[] {gameObject}, action, EventParames);
		}

		/// <summary>
		///  消除帧监听事件；
		/// </summary>
		/// <param name="gameObject"></param>
		public static void OffUpdate(this GameObject gameObject)
		{
			EventManager.OffEvent<OnUpdate>(gameObject);
		}


		/// <summary>
		///  判断一个物体是否被射线击中；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static bool RayHited(this GameObject gameObject)
		{
			return gameObject == UnityEventService.RayHit();
		}

		/// <summary>
		///  判断两个物体是否接触；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="go2"></param>
		/// <returns></returns>
		public static bool OnIntersect( this GameObject gameObject , GameObject go2 )
		{
			if (!gameObject.GetComponent<Collider>() || !go2.GetComponent<Collider>())
			{
				throw new Exception("必须保证两个物体都有Collider");
			}

			return gameObject.GetComponent<Collider>().bounds.Intersects(go2.GetComponent<Collider>().bounds);
		}


		/// <summary>
		///  从一个物体发出射线，并返回所击中的物体；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static GameObject RayHit(this GameObject gameObject_Ray, Vector3 direction)
		{
			return UnityEventService.RayHit(gameObject_Ray, direction);
		}


		/// <summary>
		///  判断从一个物体发出的射线是否击中另一个指定的物体；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <param name="gameObject_collider"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static bool RayHit(this GameObject gameObject_Ray, GameObject gameObject_collider, Vector3 direction)
		{
			return UnityEventService.RayHit(gameObject_Ray, gameObject_collider, direction);
		}


		/// <summary>
		///  返回从一个物体发出的射线所击中的点；注意：若没有击中任何物体，返回(0,0,0)；
		/// </summary>
		/// <param name="gameObject_Ray"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector3 RayHitPoint(this GameObject gameObject_Ray, Vector3 direction)
		{
			return UnityEventService.RayHitPoint(gameObject_Ray, direction);
		}


		/// <summary>
		/// 为物体添加循环检测事件
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="func"></param>
		/// <param name="rayHited"></param>
		public static void OnLoopEvent(this GameObject gameObject, Func<bool> func, bool rayHited = true)
		{
			gameObject.AddComponent<OnLoopEvent>().SetFunc(gameObject, func, rayHited);
		}

		/// <summary>
		///  由于用户直接调用 OnLoopEvent 事件，所以，要提供销毁的方法；
		/// </summary>
		/// <param name="gameObject"></param>
		public static void OffLoopEvent(this GameObject gameObject)
		{
			GameObject.Destroy(gameObject.GetComponent<OnLoopEvent>());
		}
	}
}