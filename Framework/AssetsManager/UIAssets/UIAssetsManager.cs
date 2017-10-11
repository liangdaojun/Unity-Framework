/********************************************************************************
** author：        Liang
** date：          2016-10-17 17:17:14
** description：   持有并管理客户端所有的资源
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace ZF.DataDriveCom.AssetsManagers
{
	/// <summary>
	///  持有并管理客户端所有的资源 , 将此类设计成单例
	/// </summary>
	public class UIAssetsManager
	{
		#region 单例

		private static UIAssetsManager _instance;

		private UIAssetsManager()
		{
		}

		public static UIAssetsManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new UIAssetsManager();
				}
				return _instance;
			}
		}

		#endregion

		// 这里存储的对象类型为一对多，客户端可以同时改变多个UI。

		private static Lookup<string, UIRect> _rectLookup;


		/// <summary>
		///　在场景初始化的时候就得到所有的 UILabel,包括显示的隐藏的。 目前只提供 UILabel　
		/// </summary>
		public void InitUIAssets(UIType UI_Type = UIType.All)
		{
			// 获得各种资源

			if (UI_Type == UIType.All)
			{
				object obj=  FindAllUIObjectByType<UIRect>();

				if (obj==null) return;
				
				_rectLookup = (Lookup<string, UIRect>) ((List<UIRect>)obj).ToLookup(r => r.name.ToLower());

				return;
			}

			var list = new List<UIRect>();

			if ((UI_Type & UIType.Label) != 0)
			{
				list.AddRange(FindAllUIObjectByType<UILabel>().ToArray());
			}
			if ((UI_Type & UIType.Sprite) != 0)
			{
				list.AddRange(FindAllUIObjectByType<UISprite>().ToArray());
			}
			if ((UI_Type & UIType.Texture) != 0)
			{
				list.AddRange(FindAllUIObjectByType<UITexture>().ToArray());
			}
			if ((UI_Type & UIType.Widget) != 0)
			{
				list.AddRange(FindAllUIObjectByType<UIWidget>().ToArray());
			}

			_rectLookup = (Lookup<string, UIRect>) list.ToLookup(r => r.name.ToLower());

			/*foreach (var item in _rectLookup)
			{
				foreach (var uiRect in item)
				{
					UnityTool.Log(uiRect.GetType()+"   "+uiRect.name);
				}
			}*/
		}


		/// <summary>
		///  通过名字来查找相应的对象，返回的对象可能有多个
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public List<T> FindCustomTypes<T>(string name) where T : class
		{
			var list = new List<T>();

			if (!_rectLookup.Contains(name)) return null;

			foreach (var item in _rectLookup[name])
			{
				if (typeof (T) == item.GetType())
				{
					list.Add((T) (object) item);
				}
			}

			return list;
		}


		public T FindCustomType<T>(string name) where T : class
		{
			if (string.IsNullOrEmpty(name)) throw new Exception("UI名字不能为空");

			name = name.ToLower();

			if (!_rectLookup.Contains(name))
			{
#if UNITY_EDITOR
				throw new Exception(string.Format("没有找到UI {0}", name));
#endif
				return null;
			}
			foreach (var item in _rectLookup[name])
			{
				return (T) (object) item;
			}

			return null;
		}

		/// <summary>
		///  查找在指定的UI物体 ,目前只提供 UIRect类型的UI。
		/// </summary>
		private List<T> FindAllUIObjectByType<T>() where T : UIRect
		{
			// 查找 UI Root 下所有可被控制的对象

			GameObject[] UIRoot = FindUIRoot();

			if (UIRoot==null) return null;

			var list = new List<T>();

		    foreach (var UIRooter in UIRoot)
		    {
                foreach (var UI_Object in UIRooter.GetComponentsInChildren<T>(true))
                {
                    if (UI_Object.Controller)
                    {
                        list.Add(UI_Object);
                    }
                }
		    }

			return list;
		}


		/// <summary>
		///  查找场景中的 UI Root ,若场景中不存在UI Root 或者 UI Root 被隐藏！
		/// </summary>
		/// <returns></returns>
		private GameObject[] FindUIRoot()
		{
			List<GameObject> UIRoot=new List<GameObject>();

			try
			{
			    foreach (var UIRooter in GameObject.FindObjectsOfType<UIRoot>())
			    {
			        UIRoot.Add(UIRooter.gameObject);
			    }
			}
			catch (NullReferenceException e)
			{
				/*e = new NullReferenceException("场景中不存在UI Root 或者 UI Root 被隐藏！");

				throw e;
*/
				UIRoot = null;
			}

			return UIRoot.ToArray();
		}
	}
}