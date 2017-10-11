/********************************************************************************
** author：        Liang
** date：          2017-01-19 15:30:43
** description：   组件初始化时进行的必要的配置
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using HighlightingSystem;
using ZF.DataDriveCom.AssetsManagers;

namespace ZF.DataDriveCom.Configurations
{
	/// <summary>
	///  组件初始化时进行的必要的配置
	/// </summary>
	public class Configuration
	{

		public Configuration()
		{
			// 对资源进行配置；

			AssetsConfiguration();

			// 高光效果的配置；

			HighLightingConfiguration();
		}


		/// <summary>
		///  对资源进行配置；
		/// </summary>
		private void AssetsConfiguration()
		{
			//获得场景中需要控制的资源, (这个可以不用客户端操作，也可以由客户端操作)

			UIAssetsManager.Instance.InitUIAssets();

			// 记录场景物体的信息，以便于还原场景信息；

			UndoRedoManager.Instance.RecordAllGameObjectsInfo();
		}

		/// <summary>
		///  高光效果的配置；
		/// </summary>
		private void HighLightingConfiguration()
		{

			if ( !Camera.main.GetComponent<HighlightingRenderer>() )

				Camera.main.gameObject.AddComponent<HighlightingRenderer>();

			if ( !Camera.main.GetComponent<HighlightingBlitter>() )

				Camera.main.gameObject.AddComponent<HighlightingBlitter>();

			Camera.main.GetComponent<HighlightingBlitter>()

				.highlightingRenderer = Camera.main.GetComponent<HighlightingRenderer>();
		}



	}
}