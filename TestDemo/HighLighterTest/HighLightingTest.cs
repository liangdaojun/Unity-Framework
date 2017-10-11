/********************************************************************************
** author：        Liang
** date：          2017-01-19 14:37:33
** description：   测试几种高光效果
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.TestDemo
{
	/// <summary>
	///  测试几种高光效果
	/// </summary>
	public class HighLightingTest : MonoBehaviour
	{

		// 任意一个物体；

		private GameObject GameObject;


		private void Awake()
		{
			GameObject = GameObject.Find("Root");

			// UnityTool.Log() 函数可以在控制台打印自定义颜色的字体；

			UnityTool.Log("测试高光效果", Color.green);

			// 给物体添加高光效果, 只需要下面一句话，无需做任何工作，就可以实现高光效果了；

			gameObject.OnFlashing();

			// 设置物体的边缘发光； 这将和上面设置的效果叠加；

			gameObject.OnFlashingConstant(Color.cyan);

			// OnFlashing 的两个重载方法，对应于 HighLigter 里的几个重载方法，可以控制颜色和频率；如下：

			GameObject.OnFlashing(Color.green, Color.yellow, 1);



		}

	}
}