/********************************************************************************
** author：        Liang
** date：          2016-12-07 17:05:58
** description：   支持客户端自定义事件，相当于一个 Update 方法
** version:        V_1.0.0
*********************************************************************************/


namespace ZF.DataDriveCom.Events.GameObjectEvent
{
	/// <summary>
	/// 支持客户端自定义事件，相当于一个 Update 方法
	/// </summary>
	public class OnUpdate : EventBase
	{
		private void Update()
		{
			Execute();
		}
	}
}