/********************************************************************************
** author：        Liang
** date：          2016-11-07 09:09:46
** description：   字符串提取
** version:        V_1.0.0
*********************************************************************************/


using System;
using System.Text.RegularExpressions;


namespace ZF.DataDriveCom.Tools
{
	/// <summary>
	///  将字符串转化成其他类型的工具类；
	/// </summary>
	public class CastString
	{
		/// <summary>
		///  从一个字符串中提取数字的模式；注意可以提取数字包含的符号；+号默认不提取；
		/// </summary>
		private static string pattern = @"-?[0-9]+(\.[0-9]+)?";


		/// <summary>
		///  从一个字符串中提取数字字符，并且返回 T 类型的数字依次组成的字符数组；
		/// </summary>
		/// <param name="vector3"></param>
		/// <returns></returns>
		public static T[] CastToNumbers<T>(string str)
		{
			Regex regex = new Regex(pattern);

			MatchCollection mc = regex.Matches(str);

			T[] results = new T[mc.Count];

			Func<string, object> func;

			if (typeof (T) == typeof (int))
			{
				func = s => int.Parse(s);
			}
			else if (typeof (T) == typeof (float))
			{
				func = s => float.Parse(s);
			}
			else if (typeof (T) == typeof (string))
			{
				func = s => s;
			}
			else if (typeof (T) == typeof (long))
			{
				func = s => long.Parse(s);
			}
			else if (typeof (T) == typeof (decimal))
			{
				func = s => decimal.Parse(s);
			}
			else if (typeof (T) == typeof (double))
			{
				func = s => double.Parse(s);
			}
			else
			{
				func = s => s;
			}

			for (int i = 0; i < mc.Count; i++)
			{
				results[i] = (T) func(mc[i].Value);
			}

			return results;
		}
	}
}