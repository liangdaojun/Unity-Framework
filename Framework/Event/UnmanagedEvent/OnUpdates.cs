/********************************************************************************
** author：        Liang
** date：          2016-12-12 09:14:27
** description：   ********
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;

public class OnUpdates : MonoBehaviour
{
	private Func<bool> func;


	/// <summary>
	///  设置参数；
	/// </summary>
	/// <param name="func"></param>
	public void SetFunc(Func<bool> func)
	{
		this.func = func;
	}


	private void Update()
	{
		if (func())
		{
			Destroy(this);
		}
	}
}