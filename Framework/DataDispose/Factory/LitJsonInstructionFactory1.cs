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
	public class LitJsonInstructionFactory1
	{
		/// <summary>
		///  这里提供一个默认的命名空间；
		/// </summary>
		private static string fullName = "ZF.DataDriveCom.DataDispose.Instructions";

		/// <summary>
		///  这里存储所有的对象类；
		/// </summary>
		private static Dictionary<string, IInstructionDispose> dicts = new Dictionary<string, IInstructionDispose>();


		/// <summary>
		/// 
		///  组播，支持多个方法的并行，同时处理一组命令；这里将返回一组数据； 
		/// 
		///  这里根据 Json 的 Key,利用反射，调用所有对应的处理方法；
		/// 
		///  这里， 约定 Json 文件中对象中的多个属性值中的第一个，为处理指令；
		/// 
		///  注意，这里不反回一组对象，而且 LitJson 不支持同级的同名属性值；
		/// 
		/// </summary>
		/// <param name="JData"></param>
		/// <param name="fullName"></param>
		public static void CreateGroupDispose(JsonData jsonData, bool transmit = true)
		{
			foreach (string key in ((IDictionary) jsonData).Keys)
			{
				if (transmit)
				{
					CreateDispose(jsonData[key], key);
				}
				else
				{
					CreateDispose(null, key);
				}
			}
		}


		/// <summary>
		/// 单播，根据指令，通过反射创建一个对象，返回该处理方, 并默认不传递消息；
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="name"></param>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public static bool CreateDispose(JsonData jsonData = null, string name = null)
		{
			// 这里因为 IDictionary 的 key 和 value 都是 ICollection ，所以只能这样遍历；

			if (name == null) name = jsonData.FirstKey();

			IInstructionDispose dispose = FindDispose(name);

			if (dispose != null)
			{
				return dispose.Dispose(jsonData);
			}


			// 在组件内部反射响应的类,如果没有找到，不抛异常；进而调用客户端注册的方法；

			// 可以加一个配置文件，以读取相应的配置文件，找对应的类；TODO

			// 这里先调用客户端注册的方法；

			Type type = Type.GetType(fullName + "." + name, false, true);

			if (type == null)
			{
				Func<JsonData, bool> func = CustomFunctionLibrary<JsonData>.GetFunc(name);

			    Action action = CustomFunctionLibrary<JsonData>.GetAction(name);

				if (func != null)
				{
					return func(jsonData);
                }
                if (action!=null)
                {
                    action();

                    return true;
                }

#if UNITY_EDITOR
				throw new NullReferenceException("没有找到指定的类");
#endif
				return false; // 不抛异常
			}

			try
			{
				dispose = ((IInstructionDispose) type.Assembly.CreateInstance(type.FullName, true));
			}
			catch (InvalidCastException e)
			{
				e = new InvalidCastException("没有继承 CommandDisposeBase 类");

				throw e;
			}

			dicts.Add(name, dispose);

			return dispose.Dispose(jsonData);
		}


		/// <summary>
		///  根据名字查找一个对象是否存在于该工厂；
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private static IInstructionDispose FindDispose(string name)
		{
			IInstructionDispose dispose;

			dicts.TryGetValue(name, out dispose);

			return dispose;
		}
	}
}