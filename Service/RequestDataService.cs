/********************************************************************************
** author：        Liang
** date：          2016-10-17 10:07:16
** description：   客户端请求组件返回指定的数据
** version:        V_1.0.0
*********************************************************************************/


using ZF.DataDriveCom.DataParsings;
using ZF.DataDriveCom.DataProcurements;


namespace ZF.DataDriveCom.Service
{
	/// <summary>
	///  这里定义服务类型
	/// </summary>
	public enum ServiceType
	{
		GetDataFromWebHost,

		GetDataFromLocalHost,

		SendMessage
	}

	/// <summary>
	///  该组件面向外部的服务 , 外部程序通过它与内部组件交互
	/// </summary>
	public class RequestDataService : IDataObtain
	{
		#region 单例

		private static RequestDataService _instance;

		private RequestDataService()
		{
		}

		public static RequestDataService Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new RequestDataService();

					return _instance;
				}

				return _instance;
			}
		}

		#endregion

		/// <summary>
		///  如果实验的所有逻辑都由组件完成，这个函数是为外部提供的，因此，将不被调用
		/// 
		///  这里由外部发送一个请求消息，有时候不需要返回数据，有时候用户希望返回数据
		/// </summary>
		/// <param name="SType"></param>
		/// <param name="fileName"></param>
		/// <param name="url"></param>
		/// <returns></returns>
		public static T RequestWebHostData<T>(ServiceType SType, string fileName, string url = null) where T : class
		{
			return new DataProcurement().GetDataFromWebHost<T>(fileName, url);
		}

		/// <summary>
		///  请求当前操作步骤的数据；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <returns></returns>
		public T CurStep<T>(string stepName = null)
		{
			return LitJsonParsing.Instance.CurStep<T>(stepName);
		}


		/// <summary>
		///  请求当前操作步骤的上一条数据；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <returns></returns>
		public T ProStep<T>(string stepName = null)
		{
			return LitJsonParsing.Instance.ProStep<T>(stepName);
		}


		/// <summary>
		///  请求当前操作步骤的下一条数据；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <returns></returns>
		public T NextStep<T>(string stepName = null)
		{
			return LitJsonParsing.Instance.NextStep<T>(stepName);
		}

		/// <summary>
		/// 根据名字获得操作步骤的数据；默认是从当前步骤正向查找 stepName；
		/// 
		/// 若 inverse = true,则从当前步骤反向查找；
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stepName"></param>
		/// <param name="inverse"></param>
		/// <returns></returns>
		public T GetStepData<T>(string stepName, bool inverse = false)
		{
			return LitJsonParsing.Instance.GetStepData<T>(stepName);
		}

        /// <summary>
        ///  返回执行步骤的数据，组件返回 相对当前位置的第 count 个步骤的数据；
        /// 
        ///  注意： 若 count 为负数，返回的是当前步骤前面的步骤；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
	    public T GetStepData<T>(int count)
	    {
	        return LitJsonParsing.Instance.NextPosition<T>(count);
	    }


	}
}