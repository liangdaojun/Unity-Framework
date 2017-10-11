/********************************************************************************
** author：        Liang
** date：          2016-11-30 16:08:13
** description：   ********
** version:        V_1.0.0
*********************************************************************************/


using System;


namespace UnityEngine
{
	public static class UnityTool
	{
		public static void print(object message, Color color = default(Color))
		{
			Log(message, color);
		}

		public static void Log(object message, Color color = default(Color))
		{
#if UNITY_EDITOR
			if (color == default(Color)) color = Color.yellow;

			Debug.Log(string.Format("<color=#{0}>{1}</color>", CastColorToHex(color), message));
#endif
		}


		public static string CastColorToHex(Color color)
		{
			string red = Convert.ToString((byte) ((int) 255.0f*color.r), 16);

			if (red.Length < 2) red += 0;

			string green = Convert.ToString((byte) ((int) 255.0f*color.g), 16);

			if (green.Length < 2) green += 0;

			string blue = Convert.ToString((byte) ((int) 255.0f*color.b), 16);

			if (blue.Length < 2) blue += 0;

			return red + green + blue;
		}
	}
}