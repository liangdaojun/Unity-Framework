/********************************************************************************
** author：       Liang
** date：         2016-11-02 16:09:14
** description：  数据获取接口；
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.DataParsings
{
	/// <summary>
	///  数据获取的接口；
	/// </summary>
	public interface IDataObtain
	{
		/// <summary>
		///  请求当前操作步骤的数据；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <returns></returns>
		T CurStep<T>(string stepName = null);


		/// <summary>
		///  请求当前操作步骤的上一条数据；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <returns></returns>
		T ProStep<T>(string stepName = null);


		/// <summary>
		///  请求当前操作步骤的下一条数据；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <returns></returns>
		T NextStep<T>(string stepName = null);


		/// <summary>
		/// 根据名字获得操作步骤的数据；默认是从当前步骤正向查找 stepName；
		/// 
		/// 若 inverse = true,则从当前步骤反向查找；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		T GetStepData<T>(string stepName, bool inverse = false);
	}
}