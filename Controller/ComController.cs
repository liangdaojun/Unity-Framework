/********************************************************************************
** author：        Liang
** date：          2016-10-17 10:01:14
** description：   这里实现控制权在组件内部的逻辑实现 ，这里实现它和 DataDriveCom 的解耦
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using LitJson;
using ZF.DataDriveCom.Configurations;
using ZF.DataDriveCom.DataParsings;
using ZF.DataDriveCom.Service;

namespace ZF.DataDriveCom.Controller
{
	/// <summary>
	///  实验端只负责提供实验资源，实验逻辑的验证和配置数据的获取，由组件完成；
	/// 
	///  这里是组件的逻辑控制入口； 如果组件只提供服务，就不用加载该组件；
	/// </summary>
	public abstract class ComController : MonoBehaviour
	{

		// 由客户端定义 url；

		[SerializeField]  private string url;

		// 通过 url 要加载的文件名；

		[SerializeField]  private string webHostFileName;

		// 默认本地文件，提供该文件的目地是在服务端加载失败的情况下，使用该默认文件；

		[SerializeField]  private TextAsset txt;

		// 下面是一些配置选项；

        // 在控制台打印组件执行的步骤信息；以便于跟踪执行过程；

		[SerializeField]  private bool ShowStateInfo = true;


		// 配置文件的数据；

		private string dataString;

		#region 初始化

		/// <summary>
		///  这里是整个控制类的入口；
		/// 
		///	 组件在 Awake 的时候进行资源的初始化，如：向服务器请求数据；
		/// </summary>
		private void Awake()
		{
			Init();

			// 当获取数据后，回调该方法，该方法由客户端实现；

            ComAwake();
		}


		/// <summary>
		/// 在接收到外部命令后，调用下一层进行解析配置文件，并且使它对该文件进行处理；
		/// 
		///  这个方法并不是再 Start 中调用的，而是再网页返回数据后调后调用的；TODO
		/// </summary>
		private void Start()
		{
			// 这里只是模拟接受到命令；

			if ( dataString != null ) DataParsing.Parsing( DataType.Json , dataString , true );

			// 向组件注册一些 ExternalFunctions类中的通用处理函数，这样做是为了扩展一些组件内部通用的功能；

			// 注意：组件默认是执行一次函数后，就销毁该函数，true为始终不销毁；

			CustomFunctionService.AddScriptDisposeFunction<ExternalFunctions , JsonData>( true );

			// 当获取数据后，回调该方法，该方法由客户端实现；

            ComStart();
		}

		#endregion

		#region 客户端方法；

		/// <summary>
		///  这里是客户端向组件注册组件；
		/// </summary>
		protected abstract void ComAwake();


		/// <summary>
		///  这里是客户端自定义的函数， 当组件返回数据后回调；
		/// </summary>
		protected abstract void ComStart();

		#endregion


		/// <summary>
		///  初始化资源；
		/// </summary>
		private void Init()
		{
            // 设置一些配置信息；

            ConfigurationInfo.ShowStateInfo = ShowStateInfo;

			// 初始化时必要的配置；

			new Configuration();

			// 读取配置文件；

			dataString = GetConfigureFile();
		}


		/// <summary>
		///  获取配置文件,并转化为 string 存储；
		/// </summary>
		/// <returns></returns>
		private string GetConfigureFile()
		{
			// 如果客户端提供了 url 和 web端文件名，则加载服务端。

			if ( !String.IsNullOrEmpty( webHostFileName ))
			{
                // 向服务端获取数据 ，这里 url 为空，就尝试读取默认配置，过不为空，就进行网络加载

                return RequestDataService.RequestWebHostData<string>(ServiceType.GetDataFromWebHost, webHostFileName, url);
			}
			if ( txt != null )
			{
				return txt.text; // 读取客户端提供的配置文件
			}

		    return null;
		}






	}
}