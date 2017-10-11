/********************************************************************************
** author：        Liang
** date：          2017-01-19 17:07:38
** description：   鼠标事件测试
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Events;
using ZF.DataDriveCom.FunctionLibrarys;


/// 这里的事件机制，保证了你可以在一个函数里编写多个事件；
/// 
/// 这些事件可以串行（按顺序执行）执行或者并行（分别的响应）执行；

namespace ZF.DataDriveCom.TestDemo
{

	/// <summary>
	///  鼠标事件测试；
	/// </summary>
	public class MouseEventTest : MonoBehaviour
	{

	   // public GameObject go;



		private void Awake()
		{
			//1、 物体只能拖拽一次； 

			gameObject.OnMouseDrag(); 

			//2、 想拖拽多次，可以这样：

			//gameObject.OnMouseDrag(EventParames.DontDestory);

            

			//3、 想要物体拖拽后不回到原来位置，可以这样：

			// gameObject.OnMouseDrag(EventParames.DontSetTrue);

			//4、 物体不会道原来的位置，且可以拖拽多次，可以这样：

			// gameObject.OnMouseDrag(EventParames.DontSetTrue|EventParames.DontDestory);

			//5、 这里讲一下事件托管机制，在一个函数里同时调用两个事件，需要保证事件的串行执行；

			// 所以，以下两行的第二行将不被执行，直到它的上一个事件被销毁；

			// gameObject.OnMouseDrag(EventParames.DontDestory);

			// gameObject.OnMouseRightDown(Actions); // 鼠标右击按下时触发事件；

			// 6、若不希望事件被托管，事件可以并行的被触发，下面的例子，鼠标拖拽和右击将独立被响应：

            //gameObject.OnMouseDrag(EventParames.DontDestory | EventParames.NoOrder);

			// gameObject.OnMouseRightDown( MyFunction );

			// 7、鼠标的其他事件，包括按下（down）和抬起(up)，单击、双击、右击等，如下：

			// 注意：这些事件和MouseDrag 一样，默认只响应一次，若响应多次，可以调节 EventPrames 参数；

			/*gameObject.OnMouseLeftDown(MyFunction, "鼠标左击事件");

			gameObject.OnMouseDoubleClick(MyFunction, "鼠标双击事件");

			gameObject.OnMouseRightDown(MyFunction, "鼠标右键事件");*/

		}


		private void MyFunction()
		{
			UnityTool.Log("UnityEvent 事件发生了！");
		}


		private void MyFunction(string str)
		{
			UnityTool.Log(str);
		}



	}
}
