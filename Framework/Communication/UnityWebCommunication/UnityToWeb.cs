/********************************************************************************
** author：        Liang
** date：          2016-10-17 14:39:22
** description：   Unity 与 Web 页面通讯
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ZF.Communiction.ExchangeData
{
    /// <summary>
    ///  该类实现与 Web 页面交换数据
    /// </summary>
    public  class UnityToWeb
    {


        // 这里缓存用户发送的标识符和函数；

        private static Dictionary<string, Action<string>> RequestDict = new Dictionary<string, Action<string>>();



        # region Unity To Web

        //-----------------1、 请求-------------------

        /// <summary>
        ///  通过标识符向网页  请求数据；并回调相应的方法；若回调方法为空，则不进行回调；
        /// </summary>
        /// <param name="action"></param>
        /// <param name="indentifer"></param>
        public static void UnityRequestData( string identifer, Action<string> action=null)
        {
            // 一个标识符只能请求一次，多次请求将被拦截；

            if (RequestDict.ContainsKey(identifer)) return;

            // 缓存该请求；

            RequestDict.Add(identifer,action);

            // 向 web 服务器发送消息；

            UnityTransmitData(identifer);

        }

        /// <summary>
        ///  客户端调用该函数将数据存储到 Web 服务器；
        /// </summary>
        /// <param name="identifer"></param>
        /// <param name="message"></param>
        public static void UnityStoreData(string identifer,string message)
        {
            // 向 web 服务器发送消息；

            UnityTransmitData(identifer, message);
        }


        /// <summary>
        ///  web 端调用该函数和 Unity 进行交互，传递的数据 = 消息标识符 + 消息体；
        /// 
        ///  注意： Html 页面的两个函数名为： Unity 请求数据的函数名： UnityReqeustData; 
        /// 
        ///  Unity 存储数据的函数名： UnityStoreData;
        /// </summary>
        /// <param name="identifer"></param>
        /// <param name="message"></param>
        public static void UnityTransmitData(string identifer, string message=null)
        {
            // message == null 表示为请求数据，客户端通过标识符向 Web 服务器请求数据；

            if (message == null)
            {
                Application.ExternalCall("UnityRequestData", identifer);
            }
            
            // 否则为存储数据，客户端将数据存储到 Web 服务器；

            else
            {
                Application.ExternalCall("UnityStoreData", identifer, message);
            }
        }

        //-----------------2、 响应-------------------

        public void UnityResponseRequest()
        {
            //TODO
        }


        #endregion



        #region Web To Unity

        //-----------------1、 请求-------------------


        public void WebRequestData(string identifer,string function_name,string message)
        {
            // TODO
        }



        //-----------------2、 响应-------------------

        /// <summary>
        ///  Web 端响应 Unity 的请求，并将消息传递过来；
        /// 
        ///  message 为空与否，这里不进行判断，直接交给客户端；
        /// </summary>
        /// <param name="identifer"></param>
        /// <param name="message"></param>
        public void WebResponseRequest(string identifer,string message)
        {
            // 调用客户端函数，反馈该请求；同时，将该请求从客户端清除；

            if (RequestDict.ContainsKey(identifer))
            {
                if (RequestDict[identifer]!=null) RequestDict[identifer](message);

                RequestDict.Remove(identifer);
            }
        }


        #endregion





       
    }
}