/********************************************************************************
** author：        Liang
** date：          2016-11-23 10:58:56
** description：   客户端请求组件执行特定的操作；
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.DataParsings;

namespace ZF.DataDriveCom.Service
{
	/// <summary>
	///  客户端请求组件要怎样执行操作；
	/// </summary>
	public class RequestExecuteService
	{
		
        /// <summary>
		///  执行一个客户端已经注册的函数； 遇到键“##”算作一步，将停止执行；
		/// 
		///  注意，如果是Json配置文件，如果该Json对象的值是数组，默认是停止执行的；
		/// 
		///  如果要让组件跨越数组执行，将 NoSealedArray(不屏蔽数组)设为 True；
		/// </summary>
		/// <returns></returns>
		public static void ExecuteNextStep(bool NoSealedArray = false)
		{
			LitJsonParsing.Instance.ExecuteNextStep(NoSealedArray);
		}
		

        /// <summary>
        ///  执行客户端指定的具体 Step，组件将执行 相对于当前位置的第 count 个Step；
        /// 
        ///  count 默认 =1 ,表示默认执行相对于当前 Step 的下一个 Step；
        /// 
        ///  注意： 若 count 为负值，则组件执行的位置是 当前步骤前面的步骤；
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
	    public static bool ExecuteNextStep(int count, bool noSealedArray = false)
	    {
            return LitJsonParsing.Instance.ExecuteNextStep(count, noSealedArray); 
	    }


        /// <summary>
        /// 连续执行 count 个 step，到达指定的 step ；
        /// </summary>
        /// <param name="count"></param>
        /// <param name="noSealedArray"></param>
	    public static bool ExecuteToNextStep(uint count, bool noSealedArray = false)
	    {
	        return LitJsonParsing.Instance.ExecuteToNextStep(count, noSealedArray);
	    }


		/// <summary>
        ///  执行指定的步骤，组件会从当前位置开始，执行 count 步；到达指定的 position ；
		/// 
		///  注意，如果中间某一步执行出错，将抛出异常；
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
        public static bool ExecuteToNextPosition(uint count)
		{
            return LitJsonParsing.Instance.ExecuteToNextPosition(count);
		}

        /// <summary>
        ///  执行一个特性的函数，组件会根据函数名，来匹配到指定的步骤；
        /// 
        ///  注意，慎用该方法，组件匹配后，指针将停止在该位置，其中，可能跳跃了很多步操作；
        /// 
        ///  默认是执行后，就删除该函数，dontDestroy 设为True,将保留该函数；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="dontDestory"></param>
        /// <returns></returns>
        public static bool ExecuteNextStep<T>(Func<T, bool> func, bool dontDestory = false)
        {
            return LitJsonParsing.Instance.ExecuteNextStep(func, dontDestory);
        }


        public static bool ExecuteNextStep<T>(Action action, bool dontDestory = false)
        {
            LitJsonParsing.Instance.ExecuteNextStep<T>(action, dontDestory);

            return true;
        }

	    /// <summary>
	    ///  移动指针指向的 Step； 组件将指针移动到 相对于当前位置的第 count 个 Step 的位置；
	    /// 
	    ///  注意： 如果 Step 为负数，则将指针向当前位置的前面 count 个 Step 移动；
	    /// </summary>
	    /// <param name="count"></param>
	    public static void GoToStep(int count)
	    {
            LitJsonParsing.Instance.MoveIndexToStep(count);
	    }


        /// <summary>
        ///  执行客户端指定的具体 Position，组件将执行 相对于当前位置的第 count 个Position；
        /// 
        ///  count 默认 =1 ,表示默认执行相对于当前位置的下一个位置；
        /// 
        ///  注意： 若 count 为负值，则组件执行的位置是 当前步骤前面的步骤；
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
	    public static bool ExecuteNextPosition(int count)
	    {
	        return LitJsonParsing.Instance.ExecuteNextPosition(count);
	    }

        /// <summary>
        ///  移动指针指向的位置； 组件将指针移动到 相对于当前位置的第 count 个步骤的位置；
        /// 
        ///  注意： 如果步骤为负数，则将指针向当前位置的前面 count 个步骤移动；
        /// 
        ///  默认忽略定界符；
        /// </summary>
        /// <param name="count"></param>
        public static void GoToPosition(int count)
	    {
	        LitJsonParsing.Instance.MoveIndexToPosition(count);
	    }


        /// <summary>
        /// 跳转场景；默认跳到下一个场景；
        /// 
        /// 注意： 当前场景值为0，正值往后跳，负值往前跳；
        /// 
        /// 当前场景不能跳转当前场景，避免发生循环调用；
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool GotoScene(int count=1)
        {
            return LitJsonParsing.Instance.GotoScene(count);
        }

	}
}