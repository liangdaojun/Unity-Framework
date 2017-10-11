/********************************************************************************
** auther：      Liang
** date：        2017-02-26 17:06:10
** descrition：  NGUI的 Mesh 显示；
** version:      V_1.0.0
*********************************************************************************/

using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.Tools{

    public static class NGUITool
    {

        //[MenuItem("NGUI/NGUIMeshView")]
        static public void NguiMeshView()
        {
            foreach (var panel in UIPanel.list)
            {
                foreach (var dc in panel.drawCalls)
                {
                    if (dc.gameObject.hideFlags != HideFlags.DontSave)
                    {
                        dc.gameObject.hideFlags = HideFlags.DontSave;
                    }
                    else
                    {
                        dc.gameObject.hideFlags = HideFlags.HideAndDontSave;
                    }
                }
            }
        }
		
	
	}
}
