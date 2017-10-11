/********************************************************************************
** auther：      Liang
** date：        2017-01-17 22:38:02
** descrition：  组件初始化时的配置信息
** version:      V_1.0.0
*********************************************************************************/

using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.Configurations
{
    /// <summary>
    ///  组件初始化时的配置信息,这里共享一些有客户端设置的配置参数；
    /// </summary>
	public class ConfigurationInfo
    {

        
        /// <summary>
        ///  在控制台打印组件执行的步骤信息；以便于跟踪执行过程；
        /// </summary>
        public static bool ShowStateInfo { get; set; }
        
		
	
	}
}
