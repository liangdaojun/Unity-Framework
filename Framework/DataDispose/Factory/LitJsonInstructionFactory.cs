/********************************************************************************
** author：        Liang
** date：          2016-10-24 16:13:32
** description：   指令的抽象工厂
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using ZF.DataDriveCom.FunctionLibrarys;

namespace ZF.DataDriveCom.DataDispose
{
	/// <summary>
	/// 指令传递的抽象工厂变种 ；创建对象和传递消息都通过这个类；
	/// 
	/// 这个工厂类存储各个类的缓存，保证对每个类只创建一次；
	/// </summary>
	public class LitJsonInstructionFactory
	{
		/// <summary>
		///  这里提供一个默认的命名空间；
		/// </summary>
		private static string fullName = "ZF.DataDriveCom.DataDispose.Instructions";

		/// <summary>
		///  这里存储所有的能处理指令的 对象名 和 对象；
		/// </summary>
		private static Dictionary<string, IInstructionDispose> dicts = new Dictionary<string, IInstructionDispose>();


		/// <summary>
		///  如果 key 类不存在，则创建 key 类，如果存在，则收集其方法；
		/// 
		///  判断该方法是否包含该 mothedName；
		/// </summary>
		/// <param name="key"></param>
		/// <param name="mothedName"></param>
		/// <returns></returns>
		public static bool CreateDispose(string className, string mothedName)
		{
			// 在组件内部反射响应的类,如果没有找到，不抛异常；进而调用客户端注册的方法；

			// 可以加一个配置文件，以读取相应的配置文件，找对应的类；TODO

			string functionName = mothedName.ToLower();

			IInstructionDispose dispose = FindDispose(className);

			if (dispose != null) return dispose.Dispose(functionName);

			Type type = Type.GetType(fullName + "." + className, false, true);

			if (type == null) // 调用外部的函数进行处理；
			{
                return CustomFunctionLibrary<JsonData>.ExternalDispose(className) || CustomFunctionLibrary<JsonData>.ExternalDisposeA(functionName);
			}

			try
			{
				dispose = ((IInstructionDispose) type.Assembly.CreateInstance(type.FullName, true));
			}
			catch (InvalidCastException e)
			{
				e = new InvalidCastException("没有实现 IInstructionDispose 接口！");

				throw e;
			}

			// 将处理类缓存起来；

			dicts.Add(className.ToLower(), dispose);

			// 下面的方法是判断方法是否存在，并不是正真的处理； 

			return dispose.Dispose(functionName);
		}


		/// <summary>
		/// 单播，根据指令，通过反射创建一个对象，返回该处理方, 并默认不传递消息；
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="name"></param>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public static bool Dispose(JsonData jsonData, string className = null, bool order = false)
		{
			if (className == null) className = jsonData.FirstKey().ToLower();

			IInstructionDispose dispose = FindDispose(className);

			if (!order && dispose != null) return dispose.Dispose(jsonData);

			/*Debug.Log("");

			Func<JsonData, bool> func;

			CustomFunctionLibrary<JsonData>.dicts.TryGetValue( className.ToLower() , out func );

			if( func != null ) return func( jsonData );

/*#if UNITY_EDITOR
			throw new NullReferenceException( "没有找到指定的类" );
#endif#1#

			if (order && dispose != null ) return dispose.Dispose( jsonData );*/

			return false;
		}


		/// <summary>
		///  根据名字查找一个对象是否存在于该工厂；
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private static IInstructionDispose FindDispose(string name)
		{
			string className = name.ToLower();

			IInstructionDispose dispose;

			dicts.TryGetValue(className, out dispose);

			return dispose;
		}
	}
}