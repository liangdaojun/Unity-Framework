/********************************************************************************
** author：        Liang
** date：          2016-10-28 15:01:07
** description：   保存用户自定义的函数列表
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZF.DataDriveCom.FunctionLibrarys
{
	/// <summary>
	///  这里存储客户端定义的处理指令的单例；
	/// </summary>
	public class CustomFunctionLibrary<T>
	{
		/// <summary>
		///  缓存事件函数；
		/// </summary>
		private static Dictionary<string, KeyValuePair<bool, Func<T, bool>>> DictsPairs =
			
            new Dictionary<string, KeyValuePair<bool, Func<T, bool>>>();


        /// <summary>
        ///  存储事件的函数库；
        /// </summary>
        private static Dictionary<string, KeyValuePair<bool, Action>> DictsAction =
            
            new Dictionary<string, KeyValuePair<bool, Action>>();

        #region Func<T, bool> 处理函数；

        /// <summary>
		///  为提高函数的调用效率，这里使用 Dictionary 来存储客户端函数；
		/// </summary>
		/// <param name="functionName"></param>
		/// <param name="op"></param>
		/// <returns></returns>
		public static bool ExternalDispose(string functionName, T op)
		{
			string funcName = functionName.ToLower();

			if (!ExternalDispose(funcName)) return false;

			Func<T, bool> func = DictsPairs[funcName].Value;

			if (!DictsPairs[funcName].Key) DictsPairs.Remove(funcName);

			return func(op);
		}

		/// <summary>
		///  是否包含指定的处理函数；
		/// </summary>
		/// <param name="functionName"></param>
		/// <returns></returns>
		public static bool ExternalDispose(string functionName)
		{
			return DictsPairs.ContainsKey(functionName.ToLower());
		}

		/// <summary>
		///  获取指定的处理函数；
		/// </summary>
		/// <param name="funcitonName"></param>
		/// <returns></returns>
		public static Func<T, bool> GetFunc(string funcitonName)
		{
			KeyValuePair<bool, Func<T, bool>> keyPair;

			DictsPairs.TryGetValue(funcitonName.ToLower(), out keyPair);

			return keyPair.Value;
		}


		/// <summary>
		///  添加一个处理函数；
		/// </summary>
		/// <param name="name"></param>
		/// <param name="keyPair"></param>
		public static void Add(string name, Func<T, bool> func, bool dontDestroy = false)
		{
			DictsPairs.Add(name.ToLower(), new KeyValuePair<bool, Func<T, bool>>(dontDestroy, func));
        }

        #endregion


        #region Action 处理函数；

        /// <summary>
        ///  为提高函数的调用效率，这里使用 Dictionary 来存储客户端函数；
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static bool ExternalDisposeAction(string functionName)
        {
            functionName = functionName.ToLower();

            if (!ExternalDisposeA(functionName)) return false;

            DictsAction[functionName].Value();

            return true;
        }

        /// <summary>
        ///  是否包含指定的处理函数；
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static bool ExternalDisposeA(string functionName)
        {
            return DictsAction.ContainsKey(functionName.ToLower());
        }

        /// <summary>
        ///  获取指定的处理函数；
        /// </summary>
        /// <param name="funcitonName"></param>
        /// <returns></returns>
        public static Action GetAction(string funcitonName)
        {
            KeyValuePair<bool, Action> keyPair;

            DictsAction.TryGetValue(funcitonName.ToLower(), out keyPair);

            return keyPair.Value;
        }


        /// <summary>
        /// 添加一个处理函数；
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="dontDestroy"></param>
        public static void Add(string name,Action action, bool dontDestroy = false)
        {
            DictsAction.Add(name.ToLower(), new KeyValuePair<bool, Action>(dontDestroy, action));
        }

        #endregion


        /// <summary>
        ///  清空所有 dontDestroy=false 的函数；即销毁所有需要销毁的函数；
        /// </summary>
        public static void ClearNeedDestroyFunction()
        {
            List<string> keyList = new List<string>();

            foreach (var key in DictsAction.Keys)
            {
                if (DictsAction[key].Key == false) keyList.Add(key);
            }

            foreach (var key in keyList)
            {
                DictsAction.Remove(key);
            }

            keyList.Clear();

            foreach (var key in DictsPairs.Keys)
            {
                if (!DictsPairs[key].Key) keyList.Add(key);
            }

            foreach (var key in keyList)
            {
                DictsPairs.Remove(key);
            }
        }

    }
}