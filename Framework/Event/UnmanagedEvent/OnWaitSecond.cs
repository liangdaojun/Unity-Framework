/********************************************************************************
** author：        Liang
** date：          2016-11-30 15:44:28
** description：   Unity的等待事件，单位秒
** version:        V_1.0.0
*********************************************************************************/


using System;
using System.Collections;
using UnityEngine;

namespace ZF.DataDriveCom.Events
{
	/// <summary>
	/// Unity的等待事件，单位秒；
	/// </summary>
	public class OnWaitSecond : MonoBehaviour
	{
		private Action action;

		private Action<string> actionT;

		private float time;

		private string parameters;


		public void SetAction(float time, Action action = null)
		{
			this.time = time;

			this.action = action;
		}

		public void SetAction(float time, Action<string> actionT, string parameters)
		{
			this.time = time;

			this.actionT = actionT;

			this.parameters = parameters;
		}


		private void Start()
		{
			StartCoroutine(WaitSomeMinute());
		}

		public IEnumerator WaitSomeMinute()
		{
			yield return new WaitForSeconds(time);

			if (action != null) action();

			else if (actionT != null) actionT(parameters);

			Destroy(this);
		}
	}
}