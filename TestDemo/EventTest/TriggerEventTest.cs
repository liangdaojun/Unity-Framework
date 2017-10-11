/********************************************************************************
** author：        Liang
** date：          2017-01-20 09:55:29
** description：   触发器事件检测
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.TestDemo
{

	/// <summary>
	///  测试触发器事件；
	/// </summary>
	public class TriggerEventTest : MonoBehaviour
	{


		private GameObject TriggerReceptObject;


		private void Awake()
		{
			TriggerReceptObject = GameObject.Find("TriggerReceptObject");

			// 1、测试触发器事件，首先要是物体可以移动，这里，给物体添加了一个鼠标拖拽事件，让该物体可以移动；

			gameObject.OnMouseDrag(EventParames.DontDestory | EventParames.DontSetTrue);

			// 2、为该物体添加触发事件，当两物体接触时，将触发事件,并执行客户端定义的函数，下例中将执行客户端自定义的 MyTriggerFunction；

			// 注意该函数的参数表示： 碰撞物体.OnTrigger(被碰撞物体，触发函数，[控制参数设置]); 一般事件中后两个参数都是一样的；

			//gameObject.OnTrigger(TriggerReceptObject,MyTriggerFunction,EventParames.DontDestory);

			// 3、关闭该事件；

			//gameObject.OffTrigger();

			// 4、OnIntersect 判断两物体是否交叉；

			// 将它放在 Update 事件中，可以监听两物体是否交叉，用以模拟触发器效果；

			gameObject.OnUpdate(() =>
			{
				bool is_Trigger = gameObject.OnIntersect(TriggerReceptObject);

				if (is_Trigger)

					UnityTool.Log(string.Format("{0}物体和{1}物体接触了！", gameObject.name, TriggerReceptObject.name));

			}, EventParames.DontDestory | EventParames.NoOrder);

		}


		private void MyTriggerFunction()
		{
			UnityTool.Log("触发器事件测试");
		}





	}
}