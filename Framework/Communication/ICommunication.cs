/********************************************************************************
** author：        Liang
** date：          2016-10-17 14:37:29
** description：   实现 Unity 与外部数据的通讯
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.Communiction
{
	/// <summary>
	///  Unity 外部数据交换基类
	/// </summary>
	public interface ICommunication
	{
		/// <summary>
		/// 
		///  这个方法向目标位置传递数据。
		/// 
		///  可以将 T发到后台，还是将 T.toString() 发到后台？
		/// 
		///  若目标平台是 Web,只能传递 string。默认是传递字符串，即 T.ToString(); 
		/// 
		/// </summary>
		/// <param name="messages"></param>
		void TransmitData<T>(T messages, bool useTtype = false);


		/// <summary>
		///  
		///  从外部获得数据，这里，返回类型是 object,
		/// 
		///  是由用户自行判断，并进行显示类型转换。
		///  
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		object AcquireData(string message);
	}
}