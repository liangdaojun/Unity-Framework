/********************************************************************************
** auther：      Liang
** date：        2017-02-14 15:35:35
** descrition：  自定义 UnityUtility 弹出菜单项，可方便的添加自定义功能；
** version:      V_1.0.0
*********************************************************************************/

using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.Tools
{

    //[InitializeOnLoad]
	/*public class HierarchyPopItem  {

        [MenuItem("Window/Test/exe1")]
        static void Test1()
        {
            UnityTool.Log("exe1 Execute.....");
        }

        [MenuItem("Window/Test/exe2")]
        static void Test2()
        {
            UnityTool.Log("exe2 Execute.....");
        }
        [MenuItem("Window/Test/exe3")]
        static void Test3()
        {
            UnityTool.Log("exe3 Execute.....");
        }

        /// <summary>
        ///  静态初始化函数，添加菜单项；
        /// </summary>
        static  HierarchyPopItem()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

       /// <summary>
       ///  OnGUI 函数，检测并绘制窗口；
       /// </summary>
       /// <param name="instanceID"></param>
       /// <param name="selectionRect"></param>
        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
           if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)

               && Event.current.button == 1 && Event.current.type <= EventType.mouseUp)
           {
               //这里可以判断selectedGameObject的条件

               GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

               Vector2 mousePosition = Event.current.mousePosition;

               if (selectedGameObject)

                   EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "Window/Test", null);

               else

                   EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "GameObject/Create Other", null);

               Event.current.Use();

           }
           /*else if(Event.current != null/* && selectionRect.Contains(Event.current.mousePosition)#2#

               && Event.current.button == 1 && Event.current.type <= EventType.mouseUp)
           {
               Vector2 mousePosition = Event.current.mousePosition;

               EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "GameObject/Create Other",new MenuCommand(null));

               Event.current.Use();
           }#1#
           
        }
	
	}*/
}
