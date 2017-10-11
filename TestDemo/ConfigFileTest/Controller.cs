/********************************************************************************
** author：        Liang
** date：          2017-01-19 15:51:08
** description：   第一步继承组件的接口；
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using LitJson;
using ZF.DataDriveCom.Controller;
using ZF.DataDriveCom.Service;


namespace ZF.DataDriveCom.TestDemo
{
	public class Controller : ComController
	{
        
        /// <summary>
        /// 在这里向组件注册一些通用资源；

        /// 向 DataDriveComService 组件注册类 FunctionLibrary 里的函数；

        /// 可选参数为 true 时，表示不销毁，客户端可以决定次函数的销毁时机（此时，一般不用销毁）；

        /// 可选参数为 false 时，组件在先一次收集函数时会销毁所有标记有销毁的函数；

        /// CustomFunctionService.AddScriptDisposeFunc<// Your ClassName , JsonData>(true);

        /// 如果通用的处理函数比较少，不足以写成一个类，可以使用下面这句来替换上面这句，以便提高效率；

        /// 要添加的方法最好是 private 修饰的，以便更好的封装性；
        /// </summary>
		protected override void ComAwake()
		{
            // Todo Your Code ，not necessary!


            UnityTool.Log("Controller Awake......");
           

            //CustomFunctionService.AddDisposeFunc<JsonData>(Desc);


		}


        /// <summary>
        /// 组件初始化后（获取资源和获取数据）调用该方法， 通知组件下一步操作；
        /// </summary>
        protected sealed override void ComStart()
        {
            // 这个函数由组件回调，说明可以和组件交互了；

            // 下面这句话，通知组件开始处理数据；

            RequestExecuteService.ExecuteNextStep();
        }



        //  例子： 这里向组件注册一个客户端函数，该函数处理用户的数据；

        //  注意： 如果客户端没有调用该函数，最好将该函数声明成 private 类型； 
        
        // 返回类型是 bool 类型，组件将根据这一返回类型，决定是否将指针指向下一步操作数据；

        // 函数名和配置文件保持一致，名字不区分大小写； 参数类型可以自定义，目前为 Json 数据；

	    private bool Desc(JsonData jsonData)
	    {
           

            // Todo Your Code

            // 作为测试， 将获取的数据以绿色字体打印；

            UnityTool.Log(jsonData.ToJson(), Color.green);

            
            // 若执行成功，则返回 true;

	        return true;
	    }



	}
}
