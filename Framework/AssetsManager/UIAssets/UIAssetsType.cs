/********************************************************************************
** author：       Liang
** date：         2016-10-19 20:38:58
** description：  场景管理的资源类型
** version:       V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;


namespace ZF.DataDriveCom.AssetsManagers
{
	/// <summary>
	///  场景资源管理的 资源枚举值
	/// </summary>
	[Flags]
	public enum UIType
	{
		Label = 1 << 0,

		Sprite = 1 << 1,

		Widget = 1 << 2,

		Texture = 1 << 3,

		All = (1 << 4) - 1
	}
}