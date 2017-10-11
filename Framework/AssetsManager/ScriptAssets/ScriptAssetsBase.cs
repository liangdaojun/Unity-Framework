/********************************************************************************
** author：        Liang
** date：          2016-11-04 16:52:11
** description：   脚本资源基类
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZF.DataDriveCom.Service;


namespace ZF.DataDriveCom.AssetsManagers
{
	/// <summary>
	///  脚本资源基类；
	/// </summary>
	public abstract class ScriptAssetsBase : MonoBehaviour
	{
		/// <summary>
		///  注册自身；
		/// </summary>
		protected void Awake()
		{
			ScriptAssetsManager.Instance.AddScript(this);
		}

		/// <summary>
		///  每个被管理的场景脚本，必须实现该抽象方法；
		/// </summary>
		public abstract void SceneAwake();

		/// <summary>
		///  实验结束的标志；这个可以重写，如果不重写，可由客户端默认调用该方法；
		/// 
		///  客户端当然也可以采用其他方法实现下一个场景的操作；
		/// 
		///	 最后的 Destory（this）是释放脚本所持有的资源；注意，这里组件还是持有的；
		/// </summary>
		public virtual void SceneEnd()
		{
			RequestExecuteService.ExecuteNextStep();

			Destroy(this); //串行不能这样 TODO
		}
	}
}