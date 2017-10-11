/********************************************************************************
** auther：      Liang
** date：        2017-02-10 14:34:27
** descrition：  ********
** version:      V_1.0.0
*********************************************************************************/

using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.AssetsManagers;
using ZF.DataDriveCom.Service;

namespace ZF.DataDriveCom.TestDemo
{

    public class Scene1 : ScriptAssetsBase
    {

        // 该函数将被自动调用；
        public override void SceneAwake()
        {
            UnityTool.Log("Scene1 Awake.......");

            // 执行下一步操作；注意： 该操作从当前位置开始，一直执行到 ## 分隔符才结束；

            //RequestExecuteService.ExecuteNextStep();

            // 若要精确按步骤来执行，可以这样：

            // 注意： 在执行的过程中若遇到分隔符 ## ,则自动跳过该分隔符，分隔符 ## 不算做步骤；

            //RequestExecuteService.ExecuteNextStep(10); // 向后执行 10 步；

            // 若要执行指定的步骤，可以这样：

            // 注意： 该步骤的位置是从当前位置开始的，该步骤的位置 等于 要执行步骤的 前一个；

            //RequestExecuteService.ExecuteNextPosition(3); // 通知组件执行从当前步骤开始的第 3 个步骤；

            // 你可以定位到任意操作步骤，如：

            // 下面两句话，现将指针向后移动 5 个步骤，再执行移动步骤后的 前面第 2 个 步骤； 

            //RequestExecuteService.MoveToStepPosition(5); 

            //RequestExecuteService.ExecuteNextPosition(-2);
        }


        private void FunctionTest()
        {

            UnityTool.Log("FunctionTest Execute ......");


            RequestExecuteService.ExecuteNextStep();

        }

        private void FunctionTest22()
        {
            UnityTool.Log("FunctionTest Execute ......");

        }





    }
}
