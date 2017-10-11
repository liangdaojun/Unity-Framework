/********************************************************************************
** author：       Liang
** date：         2016-10-30 09:44:41
** description：  组件获取 GameObject 的基类
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZF.DataDriveCom.AssetsManagers
{
	/// <summary>
	///  搜索类型
	/// </summary>
	public enum SearchType
	{
		OnlySelf, // 只搜索自己       

		OnlyChildDisplay, // 只搜索显示的子节点

		OnlyChildHide, // 只搜索隐藏的子节点

		AllChildren, // 搜索所有的子节点

		All // 孩子节点和自身
	}


	/// <summary>
	///  组件只通过它来获取所需操控的 GameObject 资源；
	/// 
	///  注意，在 Service 接口里的类，继承该类；这里的两个虚方法可以不实现；
	/// </summary>
	public class GameObjectAssetsBase : MonoBehaviour
	{
		/// <summary>
		///  定义搜索方式；
		/// </summary>
		[SerializeField] [Tooltip("搜索方式")] internal SearchType searchType = SearchType.OnlySelf;


		/// <summary>
		///  是否进行递归搜索
		/// </summary>
		[SerializeField] [Tooltip("进行递归")] internal bool recursion = false;


		/// <summary>
		///  客户端自定义 GameObject 的列表，并且传入到组件内部；
		/// </summary>
		/// <returns></returns>
		public virtual List<GameObject> GetObjects()
		{
			return null;
		}


		/// <summary>
		///  组件向客户端发送操作完成后的 GameObject List；
		/// </summary>
		/// <param name="objectsList"></param>
		/// <returns></returns>
		public virtual bool SetObjects(List<GameObject> objectsList)
		{
			return true;
		}
	}
}