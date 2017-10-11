/********************************************************************************
** auther：      Liang
** date：        2017-02-10 14:34:25
** descrition：  ********
** version:      V_1.0.0
*********************************************************************************/

using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.AssetsManagers;
using ZF.DataDriveCom.Service;

namespace ZF
{
    /// <summary>
    ///  该脚本必须挂载，可以挂载到任意物体上；组件负责找到它，并搜集其函数；
    /// 
    ///  组件将调用 重写的 SceneAwake 函数；
    /// </summary>
    public class Scene0 : ScriptAssetsBase
    {

        /// <summary>
        ///  配置文件根据界定符，目前是 ## ； 来将步骤分为粗细两种粒度； Step 是粗粒度的步骤，即以 ## 分割的步骤；
        /// 
        ///  Position 是细粒度的步骤，即安配置文件中的头描述定义分割的； 一般情况下是一行；
        /// </summary>
        public override void SceneAwake()
        {
            UnityTool.Log("Scene1 Awake.......");

            // 执行下一步操作；注意： 该操作从当前位置开始，一直执行到 ## 分隔符才结束；

            //RequestExecuteService.ExecuteNextStep();

            // 下面两句话先执行 第 3 个步骤，再执行 第 -2 个（相对位置）步骤；

            // 配置文件中的步骤将先向后执行，在以执行到的位置为起点，向前执行；

            RequestExecuteService.ExecuteNextStep(3);


            RequestExecuteService.ExecuteNextStep(-2);


            // 跳转场景；默认跳到下一个场景；

            // 注意： 当前场景值为0，正值往后跳，负值往前跳；

            // 当前场景不能跳转当前场景，避免发生循环调用；

            RequestExecuteService.GotoScene(4);

            
        }


       





    }
}
