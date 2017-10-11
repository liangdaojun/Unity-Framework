/********************************************************************************
** author：        Liang
** date：          2016-10-17 09:26:57
** description：   数据获取的抽象类
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.DataProcurements
{
	public abstract class DataProcurementBase
	{
		/// <summary>
		///  从 Web 页面获取数据
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		public abstract T GetDataFromWebHost<T>(string fileName, string url = null) where T : class;

		/// <summary>
		/// 从本地获取数据
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public abstract T GetDataFromLocalHost<T>(string fileName) where T : class;
	}
}