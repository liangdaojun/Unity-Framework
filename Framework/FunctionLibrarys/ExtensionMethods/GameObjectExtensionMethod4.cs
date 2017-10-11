/********************************************************************************
** author：        Liang
** date：          2016-11-14 15:09:46
** description：   GameObject 的扩展方法
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Tools;
using HighlightingSystem;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  GameObject 的附加操作；
	/// </summary>
	public static partial class GameObjectExtensionMethod
	{
		/// <summary>
		///  为物体添加高光，可调节频率,默认过渡颜色；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="freq"></param>
		public static void OnFlashing(this GameObject gameObject,float freq=2f)
		{
			if ( !gameObject.GetComponent<Highlighter>() ) gameObject.AddComponent<Highlighter>();

			gameObject.GetComponent<Highlighter>().FlashingOn( freq );
		}

		/// <summary>
		///  为物体添加高光效果；可调节过渡颜色和频率；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static void OnFlashing(this GameObject gameObject, Color beginColor,Color endColor, float freq=2f)
		{
			if (!gameObject.GetComponent<Highlighter>()) gameObject.AddComponent<Highlighter>();

			gameObject.GetComponent<Highlighter>().FlashingOn( beginColor , endColor , freq );
		}

		/// <summary>
		///  为物体添加高光，可设置颜色，默认为黄色；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="color"></param>
		public static void OnFlashingConstant(this GameObject gameObject , Color color=default(Color))
		{
			if (!gameObject.GetComponent<Highlighter>()) gameObject.AddComponent<Highlighter>();

			if ( color == default( Color ) ) gameObject.GetComponent<Highlighter>().ConstantOn( Color.yellow ); 

			else gameObject.GetComponent<Highlighter>().ConstantOn(color);
		}


		/// <summary>
		///  关闭高光效果；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static void OffFlashing(this GameObject gameObject)
		{
			GameObject.Destroy(gameObject.GetComponent<Highlighter>());
		}


		/// <summary>
		///  显示一个物体；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static void Show(this GameObject gameObject)
		{
			if (gameObject == null) return;

			gameObject.SetActive(true);
		}


		/// <summary>
		///  隐藏一个物体；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static void Hide(this GameObject gameObject)
		{
			if (gameObject == null) return;

			gameObject.SetActive(false);
		}


		/// <summary>
		///  设置物体的位置；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="posStr"></param>
		public static void SetPosition(this GameObject gameObject, string posStr)
		{
			float[] positions = CastString.CastToNumbers<float>(posStr);

			gameObject.transform.position = new Vector3(positions[0], positions[1], positions[2]);
		}

		/// <summary>
		///  设置物体的位置；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="go"></param>
		public static void SetPosition(this GameObject gameObject, GameObject go)
		{
			gameObject.transform.position = go.transform.position;
		}

		/// <summary>
		///  设置物体的旋转；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="rotStr"></param>
		public static void SetRotation(this GameObject gameObject, string rotStr)
		{
			float[] rotations = CastString.CastToNumbers<float>(rotStr);

			gameObject.transform.eulerAngles = new Vector3(rotations[0], rotations[1], rotations[2]);
		}

		/// <summary>
		/// 设置物体的旋转；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="go"></param>
		public static void SetRotation(this GameObject gameObject, GameObject go)
		{
			gameObject.transform.eulerAngles = go.transform.eulerAngles;
		}

		/// <summary>
		///  设置物体的缩放；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="scaleStr"></param>
		public static void SetScale(this GameObject gameObject, string scaleStr)
		{
			float[] scales = CastString.CastToNumbers<float>(scaleStr);

			gameObject.transform.localScale = new Vector3(scales[0], scales[1], scales[2]);
		}

		/// <summary>
		/// 设置物体的缩放；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="go"></param>
		public static void SetScale(this GameObject gameObject, GameObject go)
		{
			gameObject.transform.localScale = go.transform.localScale;
		}


		/// <summary>
		///  设置物体的层级关系；
		/// </summary>
		/// <param name="gameObject"></param>
		/// <param name="layerName"></param>
		public static void SetLayer(this GameObject gameObject, string layerName)
		{
		}
	}
}