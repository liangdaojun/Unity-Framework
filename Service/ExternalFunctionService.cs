/********************************************************************************
** author：        Liang
** date：          2016-11-04 16:32:39
** description：   为外部定义的处理 JsonData的函数库
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using LitJson;
using ZF.DataDriveCom.DataParsings;
using ZF.DataDriveCom.FunctionLibrarys;
using ZF.DataDriveCom.Tools;


namespace ZF.DataDriveCom.Service
{
	/// <summary>
	///  为外部定义的处理 JsonData 的函数库 , 客户端根据自己的需要决定注册与否；
	/// 
	///  可以将该类全部注册，也可以选择特定的函数进行注册；
	/// </summary>
	public class ExternalFunctions
	{
		/// <summary>
		///  这里根据配置文件，进行场景的调用；
		/// </summary>
		/// <param name="jsonData"></param>
		private bool Scene(JsonData jsonData)
		{
            LitJsonParsing.Instance.SetRefDictNull();// 将该类中的引用计数清空；

			for (int i = 0; i < jsonData.Count; i++)
			{
				if (!FunctionLibrary.Scene(jsonData[i][0].ToString())) return false;
			}

			return true;
		}

        /// <summary>
        ///  重定位指针，该函数将根据当前指针位置按 Step 或 Position 重定位指针；
        /// 
        ///  格式为 goto:[step|position],count=(int)数值；
        /// 
        ///  注意： 正直是向前定位，负值是向后定位；
        /// </summary>
        /// <param name="josnData"></param>
        /// <returns></returns>
	    private bool Goto(JsonData jsonData)
        {
            int[] parameters= CastString.CastToNumbers<int>(jsonData[1].ToString());

            if (jsonData[0].ToString().ToLower() == "step")
            {
                LitJsonParsing.Instance.GotoStep(parameters[0],parameters[1]);
            }
            else
            {
                // TODO
            }

	        return true;
	    }

        /// <summary>
        ///  循环执行指定的范围；
        /// 
        ///  格式为 loop:[step|position],count=(int)数值 ,number=数值；
        /// 
        ///  注意： 正直是向前定位，从指定为位置执行到当前位置； 负值是向后定位，从当前位置执行到指定位置；
        /// </summary>
        /// <param name="josnData"></param>
        /// <returns></returns>
	    private bool Loop(JsonData jsonData)
	    {
            int[] parameters = CastString.CastToNumbers<int>(jsonData[1].ToString());

            if (jsonData[0].ToString().ToLower() == "step")
            {
                LitJsonParsing.Instance.ExecuteLoopStep(parameters[0], parameters[1]);
            }
            else
            {
                // TODO
            }

	        return true;
	    }

        /// <summary>
        ///  跳转场景；
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
	    private bool GotoScene(JsonData jsonData)
        {
            return LitJsonParsing.Instance.GotoScene(int.Parse(jsonData[0].ToString()));
        }




		/// <summary>
		///  显示 JsonData 中的一系列物体；
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		private bool Display(JsonData jsonData)
		{
			return FunctionLibrary.Display(jsonData);
		}
	}
}