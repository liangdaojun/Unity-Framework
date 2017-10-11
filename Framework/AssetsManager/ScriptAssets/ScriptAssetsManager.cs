/********************************************************************************
** author：        Liang
** date：          2016-11-04 16:55:28
** description：   管理场景中的脚本资源
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZF.DataDriveCom.AssetsManagers
{
	/// <summary>
	///  管理场景中的脚本资源；
	/// </summary>
	public class ScriptAssetsManager
	{
		#region 单例

		/// <summary>
		///  单例
		/// </summary>
		private static ScriptAssetsManager _instance;

		private ScriptAssetsManager()
		{
		}


		public static ScriptAssetsManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ScriptAssetsManager();
				}

				return _instance;
			}
		}

		#endregion

		/// <summary>
		///  保存场景中所有 ObjectAssetsBase 类型的 GameObject；这里为了提高效率，可以用户手动注册；
		/// </summary>
		private static Dictionary<string, ScriptAssetsBase> scriptDicts = new Dictionary<string, ScriptAssetsBase>();


		/// <summary>
		///  根据脚本名称，返回一个脚本；
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ScriptAssetsBase GetScriptByName(string name)
		{
			if (scriptDicts.ContainsKey(name)) return scriptDicts[name];

			return null;
		}


		/// <summary>
		///  保存场景中的脚本资源；
		/// </summary>
		/// <param name="script"></param>
		public void AddScript(ScriptAssetsBase script)
		{
			if (scriptDicts.ContainsKey(script.GetType().Name)) return;

			scriptDicts.Add(script.GetType().Name, script);
		}
	}
}