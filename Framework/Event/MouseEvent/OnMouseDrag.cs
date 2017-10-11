/********************************************************************************
** author：       Liang
** date：         2016-11-02 20:25:54
** description：  添加该脚本，实现鼠标拖拽
** version:       V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using ZF.DataDriveCom.FunctionLibrarys;

namespace ZF.DataDriveCom.Events.MouseEvents
{
	/// <summary>
	///  添加该脚本，实现鼠标拖拽；
	/// </summary>
	public class OnMouseDrag : EventBase
	{
		/// <summary>
		///  存储原始位置；
		/// </summary>
		private Vector3 LatePosition;

		private Quaternion LateRotation;

		/// <summary>
		///  被碰撞的物体；
		/// </summary>
		private GameObject collider_Go;

		/// <summary>
		///  重写父类的方法，记下初始位置；
		/// </summary>
		/// <param name="go"></param>
		/// <param name="action"></param>
		/// <param name="destroy"></param>
		public override void SetAction(GameObject go, Action action, EventParames EventParames = 0)
		{
			base.SetAction(go, action, EventParames);

			GetPosition();
		}


		/// <summary>
		/// 当被拖拽物体碰到一个指定物体时，执行 Action；
		/// </summary>
		/// <param name="go"></param>
		/// <param name="collider_Go"></param>
		/// <param name="action"></param>
		/// <param name="dontSetPos"></param>
		/// <param name="dontDestroy"></param>
		public void SetAction(GameObject go, GameObject collider_Go, Action action, EventParames EventParames = 0)
		{
			base.SetAction(go, action, EventParames);

			this.collider_Go = collider_Go;

			GetPosition();
		}


		private void GetPosition()
		{
			LatePosition = go.transform.position;

			LateRotation = go.transform.rotation;

			if ((EventParames & EventParames.DontSetTrue) != 0) resetPosition = false;

			else resetPosition = true;
		}

		private void SetPosition()
		{
			go.transform.position = LatePosition;

			go.transform.rotation = LateRotation;
		}

		#region Update 拖拽

		private bool isDrag;

		private Vector3 offset;

		private Vector3 screenSpace;

		private bool isAction;

		private bool isIntersect;

		private bool resetPosition;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0) && go.RayHited())
			{
				screenSpace = Camera.main.WorldToScreenPoint(go.transform.position);

				offset = go.transform.position - Camera.main.ScreenToWorldPoint(
					new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

				isDrag = true;
			}

			if (Input.GetMouseButtonUp(0) && isDrag)
			{
				if (collider_Go && go.OnIntersect(collider_Go))
				{
					isIntersect = true;

					isAction = true;
				}
				else if (!collider_Go)
				{
					isAction = true;
				}

				// 判断是否需要设置物体的位置；

				if ((EventParames & EventParames.DontSetTrue) == 0) resetPosition = true;

				if (isAction)
				{
					if (isIntersect)
					{
						go.transform.position = collider_Go.transform.localPosition;

						go.transform.rotation = LateRotation;

						isIntersect = false;

						resetPosition = false;
					}

					Execute();

					isAction = false;
				}

				isDrag = false;
			}

			if (isDrag)
			{
				resetPosition = false;

				Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

				var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

				go.transform.position = curPosition;

				go.transform.rotation = LateRotation;
			}

			if (resetPosition) SetPosition();
		}

		#endregion

		#region IEnumerator OnMouseDown	的鼠标拖拽实现方式；

		/*
		/// <summary>
		///  首先，将三维物体坐标转屏幕坐标 
		/// 
		///  其次，将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离  
		/// </summary>
		/// <returns></returns>
		private IEnumerator OnMouseDown()
		{
			Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);

			var offset = transform.position - Camera.main.ScreenToWorldPoint(

				new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

			while (Input.GetMouseButton(0))
			{
				Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

				var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

				transform.position = curPosition;

				yield return new WaitForFixedUpdate();
			}
		}

		/// <summary>
		///  当鼠标抬起的时候，执行 func；
		/// </summary>
		private void OnMouseUp()
		{
			transform.position = LatePosition;

		    if (action != null)
		    {
			    if (go) Destroy(go.GetComponent<OnTrigger>());

				action = null;
			}
		}*/

		#endregion
	}
}