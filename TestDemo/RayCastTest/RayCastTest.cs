/********************************************************************************
** author：        Liang
** date：          2017-01-20 10:27:26
** description：   射线事件测试
** version:        V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.TestDemo
{
	namespace ZF.DataDriveCom.TestDemo
	{

		/// <summary>
		///  射线事件测试
		/// </summary>
		public class RayCastTest : MonoBehaviour
		{

			private GameObject RayCastedObject;

			private void Awake()
			{
				RayCastedObject = GameObject.Find("RayCastedObject");
			}


            private void Update()
            {
                // 1、 RayHit 方法将返回指定方向上打到的物体；

                GameObject hit_Object = gameObject.RayHit(Vector3.down);

                if (hit_Object)
                {
                    UnityTool.Log(hit_Object.name);
                }

                // 2、 RayHit 的一个重载方法，判断物体是否击中了一个物体，返回 bool值； 

                /*bool is_Hit = gameObject.RayHit(RayCastedObject,Vector3.down);

        if(is_Hit) UnityTool.Log(string.Format("{0}物体发出的射线击中了{1}",gameObject.name,RayCastedObject.name));*/

                // 3、返回从一个物体发出的射线所击中的点；注意：若没有击中任何物体，返回(0,0,0)； 

                /*Vector3 hit_Point = gameObject.RayHitPoint(Vector3.down);

        UnityTool.Log(hit_Point);*/

                // 4、 判断该物体是否被射线击中；

               /* bool is_Hited = gameObject.RayHit(RayCastedObject, Vector3.down);

                if (is_Hited) UnityTool.Log(RayCastedObject.name + "被击中了。");*/
            }





		}
	}
}