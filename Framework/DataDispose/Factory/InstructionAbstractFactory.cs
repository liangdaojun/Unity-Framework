/********************************************************************************
** author：        Liang
** date：          2016-10-31 14:20:28
** description：   指令处理的抽象工厂
** version:        V_1.0.0
*********************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Xml;
using LitJson;


namespace ZF.DataDriveCom.DataDispose
{
	/// <summary>
	/// 指令处理的抽象工厂，这里是上一层的接口；
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class InstructionAbstractFactory
	{
		public static void CreateGroupDispose<T>(DisposeType disposeType, T data, bool transmit = true)
		{
			/* if (disposeType == DisposeType.LitJson)
            {
                LitJsonInstructionFactory.CreateGroupDispose((JsonData)(object) data , false );
            }*/
		}


		/* public static IInstructionDispose CreateDispose( string name , JsonData jsonData = null )
        {
            if ( disposeType == DisposeType.LitJson )
            {
                LitJsonInstructionFactory.CreateGroupDispose( ( JsonData ) ( object ) data , false );
            }

            return null;
        }*/
	}
}