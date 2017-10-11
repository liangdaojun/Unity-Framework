/********************************************************************************
** author：       Liang
** date：         2016-10-30 09:26:07
** description：  获取场景中需要操控的 GameObject
** version:       V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using LitJson;


namespace ZF.DataDriveCom.AssetsManagers
{
	/// <summary>
	///  获取场景中需要操控的 GameObject ；懒汉式单例，在调用时，对场景中的资源搜寻一次；
	/// 
	///  注意，不是保存所有需要操控的 GameObject, 而只保存挂载 GameObjectAssetsBase 脚本的物体；
	/// </summary>
	public class GameObjectAssetsManager
	{
		#region 单例

		/// <summary>
		///  单例
		/// </summary>
		private static GameObjectAssetsManager _instance;

		private GameObjectAssetsManager()
		{
		}


		public static GameObjectAssetsManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObjectAssetsManager();

					// 在这里进行一次搜寻，也可以在私有的构造器中；

					gameObjectAssets = Resources.FindObjectsOfTypeAll<GameObjectAssetsBase>();
				}

				return _instance;
			}
		}

		#endregion

		/// <summary>
		///  保存场景中所有挂载 GameObjectAssetsBase 脚本的 GameObject；
		/// 
		///  注意： 如果客户端将两个搜寻的 GameObject 类型交叉了，这里只返回第一次搜到的物体并返回；
		/// </summary>
		public static GameObjectAssetsBase[] gameObjectAssets;

		/// <summary>
		/// 搜寻方法 ,根据名字查找相应的 GameObject ,不区分大小写；
		/// 
		/// 这里遇到第一个符合要求的 GameObject 不停止，找到一组名字一样的 GameObject 并返回；
		/// </summary>
		public List<GameObject> FindGameObjectAssets(string name)
		{
			var lists = new List<GameObject>();

			FindGameObjectAsset(name, true, lists.Add);

			return lists;
		}

		#region 查找一个物体；

		/// <summary>
		/// 搜寻方法 ,这里遇到第一个符合要求（名字相同）的 GameObject 就停止；
		/// </summary>
		public GameObject FindGameObjectAsset(string name, bool throwException = true, Action<GameObject> action = null)
		{
			/*foreach( var gameObjectAsset in gameObjectAssets )
			{
				Debug.Log( "===" + gameObjectAsset.name );
			}*/

			foreach (var goAsset in gameObjectAssets)
			{
				GameObject gameObject; // 存放要找的物体；

				switch (goAsset.searchType)
				{
					case SearchType.OnlySelf:

						if (name.ToLower() == goAsset.name.ToLower())
						{
							if (action == null)
							{
								gameObject = goAsset.gameObject;

								if (gameObject != null) return gameObject;
							}

							action(goAsset.gameObject);
						}

						break;

					case SearchType.OnlyChildDisplay:

						if (action == null)
						{
							gameObject = FindGameObjectDisplayOrHide(goAsset, name, false);

							if (gameObject != null) return gameObject;
						}

						FindGameObjectDisplayOrHide(goAsset, name, false, false, action);

						break;

					case SearchType.OnlyChildHide:

						if (action == null)
						{
							gameObject = FindGameObjectDisplayOrHide(goAsset, name, true);

							if (gameObject != null) return gameObject;
						}

						FindGameObjectDisplayOrHide(goAsset, name, true, false, action);

						break;

					case SearchType.AllChildren:

						if (action == null)
						{
							gameObject = FindGameObjectDisplayOrHide(goAsset, name, true, true);

							if (gameObject != null) return gameObject;
						}

						FindGameObjectDisplayOrHide(goAsset, name, true, true, action);

						break;

					case SearchType.All:

						if (action == null)
						{
							if (name.ToLower() == goAsset.name.ToLower())
							{
								gameObject = goAsset.gameObject;

								if (gameObject != null) return gameObject;
							}

							gameObject = FindGameObjectDisplayOrHide(goAsset, name, true, true);

							if (gameObject != null) return gameObject;
						}

						if (name.ToLower() == goAsset.name.ToLower()) action(goAsset.gameObject);

						FindGameObjectDisplayOrHide(goAsset, name, true, true, action);

						break;
				}
			}
#if UNITY_EDITOR

			if (throwException) throw new Exception("没有找到物体" + name + ", 检查物体是否被托管！");
#endif
			return null;
		}


		/// <summary>
		///  查找子节点；includeInactive 为是否搜索隐藏的物体，all 表示搜索全部，包括隐藏和显示的；
		/// 
		///  Action 是查找同名的 GameObject,并且返回；
		/// </summary>
		/// <param name="goAsset"></param>
		/// <param name="name"></param>
		/// <param name="includeInactive"></param>
		/// <returns></returns>
		private GameObject FindGameObjectDisplayOrHide(GameObjectAssetsBase goAsset,
			string name, bool includeInactive, bool all = false, Action<GameObject> action = null)
		{
			if (goAsset.recursion)
			{
				foreach (Transform childAsset in goAsset.GetComponentsInChildren<Transform>(includeInactive))
				{
					if (includeInactive ^ (childAsset.gameObject.activeSelf & !all)
					    && childAsset.name.ToLower() == name.ToLower())
					{
						if (action == null) return childAsset.gameObject;

						action(childAsset.gameObject);
					}
				}
			}
			else //goAsset.transform 无法返回 Transform[],所以两句 if 不能合并；
			{
				foreach (Transform childAsset in goAsset.transform)
				{
					if (includeInactive ^ (childAsset.gameObject.activeSelf & !all)
					    && childAsset.name.ToLower() == name.ToLower())
					{
						if (action == null) return childAsset.gameObject;

						action(childAsset.gameObject);
					}
				}
			}

			return null;
		}

		#endregion

		#region 将托管的物体的列表；

		/// <summary>
		/// 返回所有被托管的物体列表； 
		/// </summary>
		/// <returns></returns>
		public List<GameObject> GetAllGameObjectAssets()
		{
			List<GameObject> AllGameObjectsList = new List<GameObject>(); // 存放所找到的物体；

			foreach (var gameObjectAsset in gameObjectAssets)
			{
				switch (gameObjectAsset.searchType)
				{
					case SearchType.OnlySelf:

						if (!AllGameObjectsList.Contains(gameObjectAsset.gameObject)) AllGameObjectsList.Add(gameObjectAsset.gameObject);

						else throw new Exception(gameObjectAsset.name + "被重复搜索！");

						break;

					case SearchType.OnlyChildDisplay:

						AllGameObjectsList.AddRange(GetAllGameObjects(AllGameObjectsList, gameObjectAsset, false).ToArray());

						break;

					case SearchType.OnlyChildHide:

						AllGameObjectsList.AddRange(GetAllGameObjects(AllGameObjectsList, gameObjectAsset, true).ToArray());

						break;

					case SearchType.AllChildren:

						AllGameObjectsList.AddRange(GetAllGameObjects(AllGameObjectsList, gameObjectAsset, true, true).ToArray());

						break;

					case SearchType.All:

						AllGameObjectsList.Add(gameObjectAsset.gameObject);

						AllGameObjectsList.AddRange(GetAllGameObjects(AllGameObjectsList, gameObjectAsset, true).ToArray());

						break;
				}
			}

			return AllGameObjectsList;
		}


		/// <summary>
		/// 返回所有子节点；includeInactive 为是否搜索隐藏的物体，all 表示搜索全部，包括隐藏和显示的；
		/// 
		/// Action 是查找同名的 GameObject,并且返回；
		/// </summary>
		/// <param name="goAsset"></param>
		/// <param name="includeInactive"></param>
		/// <param name="all"></param>
		/// <returns></returns>
		private List<GameObject> GetAllGameObjects(List<GameObject> AllGameObjectsList, GameObjectAssetsBase goAsset,
			bool includeInactive, bool all = false)
		{
			List<GameObject> gameObjectList = new List<GameObject>();

			if (goAsset.recursion)
			{
				foreach (Transform childAsset in goAsset.GetComponentsInChildren<Transform>(includeInactive))
				{
					if (includeInactive ^ (childAsset.gameObject.activeSelf & !all))
					{
						if (AllGameObjectsList.Contains(childAsset.gameObject)) throw new Exception(childAsset.name + "被重复搜索！");

						//continue; // 如果包含该物体了，则跳过该物体；

						gameObjectList.Add(childAsset.gameObject);
					}
				}
			}
			else //goAsset.transform 无法返回 Transform[],所以两句 if 不能合并；
			{
				foreach (Transform childAsset in goAsset.transform)
				{
					if (includeInactive ^ (childAsset.gameObject.activeSelf & !all))
					{
						if (AllGameObjectsList.Contains(childAsset.gameObject)) throw new Exception(childAsset.name + "被重复搜索！");

						//continue; // 如果包含该物体了，则跳过该物体；

						gameObjectList.Add(childAsset.gameObject);
					}
				}
			}

			return gameObjectList;
		}

		#endregion

		/// <summary>
		///  递归搜索物体；
		/// </summary>
		/// <param name="searchType"></param>
		/// <param name="trans"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private GameObject RecursionGameObject(SearchType searchType, Transform trans, string name)
		{
			foreach (Transform tran in trans.transform)
			{
				if (tran.name == name) return tran.gameObject;

				return RecursionGameObject(searchType, tran, name);
			}

			return null;
		}
	}
}