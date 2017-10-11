/********************************************************************************
** auther：      Liang
** date：        2017-02-17 21:50:53
** descrition：  Undo/Redo 功能测试
** version:      V_1.0.0
*********************************************************************************/

using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.Service;

namespace ZF.DataDriveCom.TestDemo
{
    /// <summary>
    ///  Undo/Redo 功能测试
    /// </summary>
	public class UndoRedoTest : MonoBehaviour
    {

        private void Awake()
        {
            // 场景初始化时，记录一次为当前状态；

            // 如果继承了 ComController,就不用调用这一步了；

            UndoRedoService.Record();
        }

        private void Update()
        {
            // 按键 S 表示 Record (记录) 步骤操作；

            if (Input.GetKeyDown(KeyCode.S))
            {
                UndoRedoService.Record();


                UnityTool.Log("Record");
            }

            // 按键 U 表示 Undo （撤销） 操作；

            if (Input.GetKeyDown(KeyCode.U))
            {
                UndoRedoService.Undo();

                UnityTool.Log("Undo");
            }

            // 按键 R 表示 Redo (重做) 操作；

            if (Input.GetKeyDown(KeyCode.R))
            {
                UndoRedoService.Redo();

                UnityTool.Log("Redo");
            }
        }
	
	}
}
