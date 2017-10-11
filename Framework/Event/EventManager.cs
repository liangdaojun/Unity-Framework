/********************************************************************************
** author：        Liang
** date：          2016-11-14 13:10:36
** description：   串行的执行Unity事件
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections.Generic;
using ZF.DataDriveCom.Events.GameObjectEvent;
using ZF.DataDriveCom.Events.MouseEvents;


namespace ZF.DataDriveCom.Events
{
	/// <summary>
	///  串行的执行事件，当前事件没有触发，则不会执行下一个事件；
	/// 
	///  首先缓存客户端事件，然后逐个执行客户端事件；
	/// </summary>
	public class EventManager
	{
		/// <summary>
		///  将事件挂载到一个空物体上，使得事件对客户端透明；
		/// </summary>
		private static GameObject null_Go = new GameObject("Event");

		/// <summary>
		///  缓存一步事件；
		/// </summary>
		private static UnityEvent unityEvent;

		/// <summary>
		///  对于第一个事件不进行缓存，否则，事件队列将无法触发；
		/// </summary>
		private static bool begin = true;

		/// <summary>
		///  对于事件进行先进先出（FIFO），以保证事件的顺序；
		/// </summary>
		private static Queue<UnityEvent> eventQueue = new Queue<UnityEvent>();


		static EventManager()
		{
			//null_Go.hideFlags = HideFlags.HideInInspector; 
		}

		#region 设置事件参数

		/// <summary>
		///  对事件的类型进行设置； T 为无参数的委托；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="eventType"></param>
		/// <param name="gos"></param>
		/// <param name="function"></param>
		/// <param name="eventsManager"></param>
		public static void SetFunc<T>(EventsType eventType, GameObject[] gos, T function, EventParames eventsManager)
			where T : class
		{
			UnityEvent unityEvent = new UnityEvent();

			if (typeof (T) == typeof (Action))
			{
				unityEvent.action = (Action) (object) function;
			}
			else if (typeof (T) == typeof (Func<bool>))
			{
				unityEvent.func = (Func<bool>) (object) function;
			} 
			else if (typeof(T)==typeof(Nullable))
			{
				 UnityTool.Log("OK……");
			}
			else
			{
				throw new Exception("事件委托机制不支持该函数类型！");
			}

			SetFunc(unityEvent, eventType, gos, eventsManager);
		}

		/// <summary>
		///  对事件的类型进行设置； T 为有参数的委托；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="eventType"></param>
		/// <param name="gos"></param>
		/// <param name="function"></param>
		/// <param name="parameters"></param>
		/// <param name="eventsManager"></param>
		public static void SetFunc<T>(EventsType eventType, GameObject[] gos, T function, string parameters,
			EventParames EventParames) where T : class
		{
			UnityEvent unityEvent = new UnityEvent();

			if (typeof (T) == typeof (Action<string>))
			{
				unityEvent.AddActKeyPair(parameters, (Action<string>) (object) function);
			}
			else
			{
				throw new Exception("事件委托机制不支持该函数类型！");
			}

			SetFunc(unityEvent, eventType, gos, EventParames);
		}

		/// <summary>
		///  设置事件类型
		/// </summary>
		/// <param name="unityEvent"></param>
		/// <param name="eventType"></param>
		/// <param name="gos"></param>
		/// <param name="eventsManager"></param>
		private static void SetFunc(UnityEvent unityEvent, EventsType eventType, GameObject[] gos, EventParames EventParames)
		{
			foreach (var go in gos)
			{
				if (!go.GetComponent<Collider>())

					throw new Exception(go.name + "物体没有碰撞器，不能被检测！");
			}

			unityEvent.go = gos;

			unityEvent.EventType = eventType;

			unityEvent.EventParames = EventParames;

			if ((EventParames & EventParames.NoOrder) != 0)
			{
				// 这里向事件脚本中传递的是 客户端的Action;

				AddEvent(unityEvent, unityEvent.action);

				return;
			}

			eventQueue.Enqueue(unityEvent);

			if (begin)
			{
				NextTool();

				begin = false;
			}
		}

		#endregion

		#region 设置事件参数

		/// <summary>
		///  删除非托管事件；其实是不必要的；
		/// 
		///  注意：非托管事件，暂时不提供删除功能；
		/// </summary>
		/// <param name="eventsType"></param>
		/// <param name="go"></param>
		public static void OffEvent<T>(GameObject go) where T : EventBase
		{
			T[] t = null_Go.GetComponents<T>();

			for (int i = 0; i < t.Length; i++)
			{
				if (t[i].go == go)
				{
					GameObject.Destroy(t[i]);
				}
			}
		}

		/// <summary>
		///  删除某个物体上的某个事件； 如果有 order 的事件，必须从队列里删除该事件；TODO
		/// </summary>
		/// <param name="go"></param>
		/// <param name="eventName"></param>
		public static void OffEvent(GameObject go, string eventName)
		{
			EventBase[] events = null_Go.GetComponents<EventBase>();

			for (int i = 0; i < events.Length; i++)
			{
				if (events[i].go == go && events[i].GetType().Name.ToLower() == eventName.ToLower())
				{
					GameObject.Destroy(events[i]);
				}
			}
		}

		#endregion

		/// <summary>
		///  根据事件类型，执行下一个指令；
		/// </summary>
		private static void NextTool()
		{
			// 判断是否存在对应的时间类型，若存在，则调用相应的时间类型； 

			if (unityEvent.action != null)
			{
				unityEvent.action();

				if ((unityEvent.EventParames & EventParames.DontDestory) == 0) unityEvent.action = null;
			}
			else if (unityEvent.func != null)
			{
				if (!unityEvent.func())
				{
					unityEvent.EventParames |= EventParames.DontDestory;

					return;
				}

				unityEvent = default(UnityEvent);
			}
			else if (unityEvent.actKeyPair.Value != null)
			{
				unityEvent.actKeyPair.Value(unityEvent.actKeyPair.Key);

				if ((unityEvent.EventParames & EventParames.DontDestory) == 0)

					unityEvent.actKeyPair = default(KeyValuePair<string, Action<string>>);
			}

			if ((unityEvent.EventParames & EventParames.DontDestory) != 0) return;

			if (eventQueue.Count == 0)
			{
				begin = true;

				return;
			}

			unityEvent = eventQueue.Dequeue();

			AddEvent(unityEvent, NextTool);
		}


		/// <summary>
		///  添加一个事件；
		/// </summary>
		/// <param name="mouseEvent"></param>
		private static void AddEvent(UnityEvent myEvent, Action action)
		{
			switch (myEvent.EventType)
			{
				case EventsType.LeftMouseDown:

					null_Go.AddComponent<OnMouseLeftDown>().SetAction(myEvent.go[0], action, myEvent.EventParames);

					break;

				case EventsType.LeftMouseUp:

					null_Go.AddComponent<OnMouseLeftUp>().SetAction(myEvent.go[0], action, myEvent.EventParames);

					break;

				case EventsType.RightMouseDown:

					null_Go.AddComponent<OnMouseRightDown>().SetAction(myEvent.go[0], action, myEvent.EventParames);

					break;

				case EventsType.RightMouseUp:

					null_Go.AddComponent<OnMouseRightUp>().SetAction(myEvent.go[0], action, myEvent.EventParames);

					break;

				case EventsType.LeftMouseDoubleClick:

					null_Go.AddComponent<OnMouseDoubleClick>().SetAction(myEvent.go[0], action, myEvent.EventParames);

					break;

				case EventsType.MouseDrag:

					if (myEvent.go.Length == 1)
						null_Go.AddComponent<OnMouseDrag>().SetAction(myEvent.go[0], action, myEvent.EventParames);

					else null_Go.AddComponent<OnMouseDrag>().SetAction(myEvent.go[0], myEvent.go[1], action, myEvent.EventParames);

					break;

				case EventsType.Trigger:

					if (myEvent.go.Length < 2) throw new Exception("配置文件参数不对！");

					null_Go.AddComponent<OnTrigger>().SetAction(myEvent.go[0], myEvent.go[1], action, myEvent.EventParames);

					break;

				case EventsType.OnUpdate:

					null_Go.AddComponent<OnUpdate>().SetAction(myEvent.go[0], action, myEvent.EventParames);

					break;
			}
		}
	}
}