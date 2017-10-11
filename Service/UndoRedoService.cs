/********************************************************************************
** auther：      Liang
** date：        2017-1-17 21:56:24
** descrition：  Undo/Redo 服务
** version:      V_1.0.0
*********************************************************************************/

using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.AssetsManagers;

namespace ZF.DataDriveCom.Service
{
    /// <summary>
    ///  提供 Undo/Redo 服务；组件默认在初始化的时候记录一次数据,即默认刚开始的状态为最初始状态；
    /// 
    ///  注意： 1、在使用 Undo/Redo 前，一定要先调用 Record 记录数据；
    /// 
    ///  2、只有被托管的物体才能被 Undo/Redo 操作，参考 GameObjectObtainService 了解物体怎样被托管；
    /// </summary>
	public class UndoRedoService  
    {
        /// <summary>
        ///  保存场景数据，客户端程序需要自定义步骤： 有可能一个操作算作一步，
        /// 
        ///  也有可能很多操作算作一步，客户端将定义什么样的状态算作一个步骤；
        /// </summary>
        public static void Record()
        {
            UndoRedoManager.Instance.RecordAllGameObjectsInfo();
        }


        /// <summary>
        ///  Undo 操作，即撤销操作，组件将整个场景的数据重置为上一个步骤；
        /// </summary>
        public static void Undo()
        {
            UndoRedoManager.Instance.UndoAllGameObjectsInfo();
        }

        /// <summary>
        ///  Redo 操作，即重做操作，组件将回到原来的操作步骤；
        /// </summary>
        public static void Redo()
        {
            UndoRedoManager.Instance.RedoAllGameObjectsInfo();
        }

	}
}
