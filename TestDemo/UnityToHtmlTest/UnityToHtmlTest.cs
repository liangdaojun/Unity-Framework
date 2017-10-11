/********************************************************************************
** author：        Liang
** date：          2017-01-20 14:34:18
** description：   Unity 和 Html 的通讯测试
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using ZF.Communiction.ExchangeData;
using ZF.DataDriveCom.Communiction;


namespace ZF.DataDriveCom.TestDemo
{

	public class UnityToHtmlTest : MonoBehaviour
	{
		// Unity 请求的标识符；
		private string UnityRequestIdentifer="";

		// Web 返回的数据；
		private string WebResponseMessage="";


		#region GUI 部分；

		private void OnGUI()
		{
			// Unity 请求部分；

			UnityRequestGUI();

			// Web 响应部分；

			WebResponseGUI();

			
		}


		/// <summary>
		///  Unity 向 Web 发送请求；
		/// </summary>
		private void UnityRequestGUI()
		{
			GUI.Label( new Rect( 50 , 30 , 200 , 30 ) , "Unity 向 Web 发送请求 " );

			UnityRequestIdentifer = GUI.TextArea( new Rect( 50 , 70 , 200 , 30 ) , UnityRequestIdentifer );

			if ( GUI.Button( new Rect( 50 , 110 , 200 , 30 ) , "发送" ) )
			{
                // Unity 请求数据；

                UnityToWeb.UnityRequestData(UnityRequestIdentifer, WebResponseRequest);
			}
		}

		/// <summary>
		/// Web 响应 Unity 的请求； 
		/// </summary>
		private void WebResponseGUI()
		{
			GUI.Label( new Rect( 400 , 30 , 200 , 30 ) , "Web 响应 Web 的请求 " );

            WebResponseMessage = GUI.TextArea(new Rect(400, 70, 200, 30), WebResponseMessage);
		}


		#endregion

		/// <summary>
		///  Web 响应 Unity 的请求；
		/// </summary>
		public void WebResponseRequest(string message)
		{
			WebResponseMessage = message;
		}
		

	}
}
