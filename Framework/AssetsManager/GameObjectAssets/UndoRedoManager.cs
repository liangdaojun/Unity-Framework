/********************************************************************************
** author：        Liang
** date：          2017-01-17 14:40:34
** description：   记录场景信息，对场景进行 Undo/Redo 操作
** version:        V_1.0.0
*********************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using ZF.DataDriveCom.Configurations;
using ZF.DataDriveCom.Tools;

namespace ZF.DataDriveCom.AssetsManagers
{
	/// <summary>
	///  记录所有被托管的物体，提供还原物体初始状态的方法；这里扩展了 Undo\Redo 功能的实现。
	/// 
	///  这里将物体信息存储到 Json 文件，而没有存储到内存，原因是为了在 Web 或者其他在线系统的场景还原；
	/// 
	///  注意：Undo/Redo 对 Delete 是无效的；
	/// </summary>
	public class UndoRedoManager
	{
		#region 单例

		/// <summary>
		///  单例
		/// </summary>
		private static UndoRedoManager _instance;

		private string filePath;

		/// <summary>
		/// 如果不存在目录，则创建目录，如果存在目录，则创建文件；
		/// </summary>
        private UndoRedoManager()
		{
			filePath = Application.streamingAssetsPath + @"/GameObjectsInfo.txt";

			gameObjectsList = GameObjectAssetsManager.Instance.GetAllGameObjectAssets();

			/*if ( !Directory.Exists( Application.streamingAssetsPath ) )
			{
				Directory.CreateDirectory( Application.streamingAssetsPath );
			}

			if ( !File.Exists( filePath ) )
			{
				File.Create( filePath );
			}*/
		}


        public static UndoRedoManager Instance
		{
			get
			{
				if (_instance == null)
				{
                    _instance = new UndoRedoManager();
				}

				return _instance;
			}
		}

		#endregion

		// Undo 栈；

		private Stack<JsonData> UndoStack = new Stack<JsonData>();

		// Redo 栈；

		private Stack<JsonData> RedoStack = new Stack<JsonData>();

		// 初始化场景物体列表；

		private List<GameObject> gameObjectsList = new List<GameObject>();

		#region Undo 物体；

		/// <summary>
		///  重置所有物体； 这里只还原物体的 Transform信息；
		/// </summary>
		public void UndoAllGameObjectsInfo()
		{
			JsonData jsonData = UndoJsonData();

			if (jsonData == null) return;

			for (int i = 0; i < jsonData.Count; i++)
			{
				ResetGameObjectInfo(jsonData[i]);
			}
#if UNITY_EDITOR
            if (ConfigurationInfo.ShowStateInfo) UnityTool.Log("Undo");
#endif
		}


		/// <summary>
		///  根据名字还原一个物体；
		/// </summary>
		/// <param name="name"></param>
		public void UndoGameObject(string name)
		{
			JsonData jsonData = UndoJsonData();

			if (jsonData == null) return;

			for (int i = 0; i < jsonData.Count; i++)
			{
				if (name == jsonData[i]["name"].ToString())
				{
					ResetGameObjectInfo(jsonData[i]);

					break; // 跳出循环，假定物体名字是唯一的；
				}
			}
		}

		/// <summary>
		///  根据名字还原物体的信息；
		/// </summary>
		/// <param name="names"></param>
		public void UndoGameObjects(string[] names)
		{
			foreach (var name in names)
			{
				UndoGameObject(name);
			}
		}

		#endregion

		#region Redo 物体；

		/// <summary>
		///  重置所有物体； 这里只还原物体的 Transform信息；
		/// </summary>
		public void RedoAllGameObjectsInfo()
		{
			JsonData jsonData = RedoJsonData();

			if (jsonData == null) return;

			for (int i = 0; i < jsonData.Count; i++)
			{
				ResetGameObjectInfo(jsonData[i]);
			}
#if UNITY_EDITOR
            if (ConfigurationInfo.ShowStateInfo) UnityTool.Log("Redo");
#endif
		}


		/// <summary>
		///  根据名字还原一个物体；
		/// </summary>
		/// <param name="name"></param>
		public void RedoGameObject(string name)
		{
			JsonData jsonData = RedoJsonData();

			if (jsonData == null) return;

			for (int i = 0; i < jsonData.Count; i++)
			{
				if (name == jsonData[i]["name"].ToString())
				{
					ResetGameObjectInfo(jsonData[i]);

					break; // 跳出循环，假定物体名字是唯一的；
				}
			}
		}

		/// <summary>
		///  根据名字还原物体的信息；
		/// </summary>
		/// <param name="names"></param>
		public void RedoGameObjects(string[] names)
		{
			foreach (var name in names)
			{
				UndoGameObject(name);
			}
		}

		#endregion

		/// <summary>
		///  记录所有资源的信息；将被托管的物体记录为 Json 文件，以便于将信息保存到服务器；
		/// 
		///  程序启动时，自动记录一次；
		/// </summary>
		public void RecordAllGameObjectsInfo()
		{
			// 要还原资源的状态，首先要保存资源的状态。并将以上获得物体持久化到文件；

			// {物体名：“  ”， 父物体：“” ， 标签：“” ， 层级：“” ， 位置：“” ， 旋转：“” ， 缩放： “” ，…… }

			StringBuilder strBuilder = new StringBuilder();

			JsonWriter jsonWriter = new JsonWriter(strBuilder);

			jsonWriter.WriteArrayStart();

			foreach (var gameObjectAsset in gameObjectsList)
			{
				jsonWriter.WriteObjectStart();

				jsonWriter.WritePropertyName("name");

				jsonWriter.Write(gameObjectAsset.name);

				jsonWriter.WritePropertyName("parent");

				jsonWriter.Write(gameObjectAsset.transform.parent != null ? gameObjectAsset.transform.parent.name : "");

				jsonWriter.WritePropertyName("active");

				jsonWriter.Write(gameObjectAsset.activeSelf);

				jsonWriter.WritePropertyName("tag");

				jsonWriter.Write(gameObjectAsset.tag);

				jsonWriter.WritePropertyName("layer");

				jsonWriter.Write(gameObjectAsset.layer);

				jsonWriter.WritePropertyName("pos");

				jsonWriter.Write(gameObjectAsset.transform.position.x + "," + gameObjectAsset.transform.position.y + "," +
				                 gameObjectAsset.transform.position.z);

				jsonWriter.WritePropertyName("rot");

				jsonWriter.Write(gameObjectAsset.transform.eulerAngles.x + "," + gameObjectAsset.transform.eulerAngles.y + "," +
				                 gameObjectAsset.transform.eulerAngles.z);

				jsonWriter.WritePropertyName("scale");

				jsonWriter.Write(gameObjectAsset.transform.localScale.x + "," + gameObjectAsset.transform.localScale.y + "," +
				                 gameObjectAsset.transform.localScale.z);

				jsonWriter.WriteObjectEnd();

				strBuilder.AppendLine();
			}

			jsonWriter.WriteArrayEnd();

			// 记录数据；

			RecordJsonData(strBuilder);
		}


		/// <summary>
		/// 做撤销时返回的数据；
		/// </summary>
		/// <returns></returns>
		private JsonData UndoJsonData()
		{
			if (UndoStack.Count > 1)
			{
				RedoStack.Push(UndoStack.Pop());

				return UndoStack.Peek();
			}

			return null;
		}

		/// <summary>
		/// 做重做时返回时的数据；
		/// </summary>
		/// <returns></returns>
		private JsonData RedoJsonData()
		{
			JsonData jsonData = null;

			if (RedoStack.Count > 0)
			{
				jsonData = RedoStack.Pop();

				UndoStack.Push(jsonData);
			}

			return jsonData;
		}

		/// <summary>
		///  记录 JsonData 数据；
		/// </summary>
		/// <param name="strBuilder"></param>
		private void RecordJsonData(StringBuilder strBuilder)
		{
			// 将此次　json 数据写入文件；这里保存在内存里，以便快速操作；

			// WriteGameObjectInfo( strBuilder );

			// 将此次操作加入 List_Indeex 的后面；

			UndoStack.Push(JsonMapper.ToObject(strBuilder.ToString()));

			RedoStack.Clear();
#if UNITY_EDITOR
            if (ConfigurationInfo.ShowStateInfo) UnityTool.Log("Record");
#endif
		}


		/// <summary>
		///  重置一个物体的所有属性；
		/// </summary>
		/// <param name="gameObjectInfo"></param>
		private void ResetGameObjectInfo(JsonData gameObjectInfo)
		{
			string gameObjectName = gameObjectInfo["name"].ToString();

			GameObject gameObject = GameObjectAssetsManager.Instance.FindGameObjectAsset(gameObjectName, false);

			if (gameObject == null)
			{
				gameObject = GameObject.Find(gameObjectName);

				if (gameObject == null) return;
			}

			// 还原父子关系；

			gameObjectName = gameObjectInfo["parent"].ToString();

			// 如果父子关系发生变化，则改变父子关系；

			if (gameObject.transform.parent == null || gameObject.transform.parent.name != gameObjectName)
			{
				if (gameObjectName == "") gameObject.transform.parent = null;

				else
				{
					GameObject parentGameObject = GameObjectAssetsManager.Instance.FindGameObjectAsset(gameObjectName, false);

					if (parentGameObject == null) parentGameObject = GameObject.Find(gameObjectName);

					if (parentGameObject != null) gameObject.transform.parent = parentGameObject.transform;
				}
			}


			// 还原激活状态、标签和层级；

			gameObject.SetActive(gameObjectInfo["active"].ToString() == "True");

			gameObject.tag = gameObjectInfo["tag"].ToString();

			gameObject.layer = int.Parse(gameObjectInfo["layer"].ToString());

			// 还原 Transform属性；

			float[] Vector3 = CastString.CastToNumbers<float>(gameObjectInfo["pos"].ToString());

			gameObject.transform.position = new Vector3(Vector3[0], Vector3[1], Vector3[2]);

			Vector3 = CastString.CastToNumbers<float>(gameObjectInfo["rot"].ToString());

			gameObject.transform.eulerAngles = new Vector3(Vector3[0], Vector3[1], Vector3[2]);

			Vector3 = CastString.CastToNumbers<float>(gameObjectInfo["scale"].ToString());

			gameObject.transform.localScale = new Vector3(Vector3[0], Vector3[1], Vector3[2]);
		}

		#region 读写 Json 文件；

		/// <summary>
		///  从 Json 文件读取物体的信息； 这一步是获取 Json 文件后的操作；
		/// </summary>
		private void ReadGameObjectsInfo()
		{
			//本地路径  
			var fileAddress = Path.Combine(Application.streamingAssetsPath, "GameObjectsInfo.txt");

			FileInfo fileInfo = new FileInfo(fileAddress);

			string s;

			if (fileInfo.Exists)
			{
				StreamReader r = new StreamReader(fileAddress);

				s = r.ReadToEnd();

				UndoStack.Push(JsonMapper.ToObject(s));

				RedoStack.Clear();

				//jsonData = JsonMapper.ToObject( s );
			}
		}


		/// <summary>
		///  从 Json 文件读取物体的信息； 这一步是获取 Json 文件后的操作；
		/// </summary>
		private void WriteGameObjectInfo(StringBuilder strBuilder)
		{
			StreamWriter streamWriter = new StreamWriter(filePath);

			streamWriter.WriteLine(strBuilder);

			streamWriter.Flush();

			streamWriter.Close();
		}

		#endregion
	}
}