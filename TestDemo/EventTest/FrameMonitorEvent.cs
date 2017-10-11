/********************************************************************************
** author：        Liang
** date：          2017-01-20 08:53:28
** description：   帧监听事件测试
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.FunctionLibrarys;



namespace ZF.DataDriveCom.TestDemo
{
	/// <summary>
	///  帧监听事件检测；
	/// 
	///  封装这些事件，一方面是支持配置文件调用，另一方面是支持客户端的调用和方便编写 Lamda 表达式；
	/// </summary>
	public class FrameMonitorEvent : MonoBehaviour
	{


		private void Awake()
		{
			// 1、为该物体添加监听事件；该函数的签名要求有 bool 类型的返回值，没帧执行该函数，

			// 若该函数返回值为 true 时，停止执行；你可以用 if else 来控制该执行方法，就像在 Update 中一样；

			// gameObject.OnLoopEvent(MyLoopFunction);

			// 2、 上面一句代码是鼠标点击到该物体，才触发 OnLoopEvent, 若不希望有鼠标参与，可以这样：

			// gameObject.OnLoopEvent(MyLoopFunction,false);

			// 3、这些事件将不被托管，即没有事件的串行保证；要想事件被托管，且可以在配置文件里调用，使用下面的函数，可以这样：

			// gameObject.OnUpdate(MyUpdateFunction); 

			// 4、 若想更多调节，和 MouseEvent 一样，同样可以调节事件的 EventPrames 参数；如，可以这样：

			// gameObject.OnUpdate(MyUpdateFunction,EventParames.DontDestory|EventParames.NoOrder);

			// 5、第4条设置了非托管的事件，怎样在确定的时机关闭它呢，你可以这样：

			// gameObject.OffUpdate();	 // 注意：所有非托管的事件都可以用 Off+事件名 来关闭；

		}


		private void MyUpdateFunction()
		{
			UnityTool.Log("Update 事件测试");
		}


		private bool MyLoopFunction()
		{
			if (Input.GetMouseButtonDown(0))
			{
				UnityTool.Log("帧监听事件测试");

				return true;
			}

			return false;
		}

	}
}