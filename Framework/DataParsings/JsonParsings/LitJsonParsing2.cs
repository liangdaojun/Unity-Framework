/********************************************************************************
** author：        Liang
** date：          2016-10-31 15:57:43
** description：   LitJson 的数据解析类；
** version:        V_1.0.0
*********************************************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using ZF.DataDriveCom.Configurations;
using ZF.DataDriveCom.DataDispose;
using ZF.DataDriveCom.FunctionLibrarys;


namespace ZF.DataDriveCom.DataParsings
{
    /// <summary>
    ///  LitJson 的数据解析类，对 LitJson 进行调用前预处理；
    /// 
    ///  根据外接提供的关键字，进行 Json 文件的解析 ，这些关键子就是Json 文件提供的；
    /// 
    ///  这里调用下一层服务，即 Json 的数据处理，为对下一层解耦，这里使用一个接口；
    /// 
    ///  先获的一级命令，根据反射获得相应的处理类，如果不存在该类，向下执行，不抛异常；
    /// </summary>
    public partial class LitJsonParsing : IDataObtain
    {
        // 配置文件中的分界符；

        private string delimiter = "##";

        // 对 Loop 操作的执行次数进行计数；

        private Dictionary<int,int> RefDict=new Dictionary<int, int>(); 
        

        #region Step 和 Position 定位

        /// <summary>
        ///  移动指针指向的 Step； 组件将指针移动到 相对于当前位置的第 count 个 Step 的位置；
        /// 
        ///  注意： 如果 Step 为负数，则将指针向当前位置的前面 count 个 Step 移动；
        /// </summary>
        /// <param name="count"></param>
        public bool MoveIndexToStep(int count)
        {
            int i_index = 0;

            if (count > 0 && index + 1 >= JsonItemList.Count|| count<0 && index-1<0) return false;

            while (i_index < Math.Abs(count))
            {
                // 每一步都要判断 index 是否越界；  index<0 是可以的，但客户端程序有可能进入死循环，所以这里保证其非负；

                if (count > 0 && index + 1 > JsonItemList.Count) throw new Exception("索引位置越界！");

                // 保证移动范围在 Scene 范围内；

                if (count > 0) index++;

                else if (count < 0) index--;

                if (JsonItemList[index].methodName == delimiter || JsonItemList[index].JsonData.IsArray) i_index++;

                if (i_index < Math.Abs(count) && JsonItemList[index].methodName == "scene") throw new Exception("索引范围不能跨越 scene");
            }

            return true;
        }


        /// <summary>
        ///  移动指针指向的 Position； 组件将指针移动到 相对于当前位置的第 count 个 Position 的位置；
        /// 
        ///  注意： 如果 Position 为负数，则将指针向当前位置的前面 count 个 Position 移动；
        /// 
        ///  默认忽略定界符；
        /// </summary>
        /// <param name="count"></param>
        /// <param name="ingoreDelimiter"></param>
        public void MoveIndexToPosition(int count, bool ingoreDelimiter = true)
        {
            // 判断一下 count 的范围；

            if (index + count < 0 || index + count > JsonItemList.Count) throw new Exception("索引位置越界！");

            // 如果不忽略定界符；

            if (!ingoreDelimiter)
            {
                index += count;

                return;
            }

            int i_index = 0;

            while (i_index < Math.Abs(count))
            {
                if (JsonItemList[index].methodName != delimiter) i_index++;

                if (count > 0)
                {
                    index++;

                    if (JsonItemList[index].methodName == "scene")

                        throw new Exception("索引范围不能跨越 scene");
                }

                else if (count < 0)
                {
                    if (JsonItemList[index].methodName == "scene")

                        throw new Exception("索引范围不能跨越 scene");

                    index--;
                }
            }

            //UnityTool.Log(index);
        }

        #endregion


        #region Scene 操作


        /// <summary>
        ///  跳转场景；
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool GotoScene(int count)
        {
            if (count <= 0) count--;

            int i_index = 0;

            while (i_index < Math.Abs(count))
            {
                if (count > 0) index++;

                else if (count < 0) index--;

                // 每一步都要判断 index 是否越界；  index<0 是可以的，但客户端程序有可能进入死循环，所以这里保证其非负；

                if (count < 0 && index - 1 < 0 || count > 0 && index + 1 > JsonItemList.Count) throw new Exception("索引位置越界！");

                if (JsonItemList[index].methodName == "scene") i_index++;
            }

            index--;

            // 执行当前步骤；

            ExecuteNextStep();

            

            return true;
        }

        #endregion

        #region goto 和 loop 操作

        /// <summary>
        ///  将执行计数的的字典置空；
        /// </summary>
        public void SetRefDictNull()
        {
            RefDict.Clear();
        }


        /// <summary>
        ///  重定位指针，该函数将根据当前指针位置按 Step 或 Position 重定位指针；
        /// 
        ///  格式为 goto:[step|position],count=(int)数值；
        /// 
        ///  注意： 正直是向前定位，负值是向后定位；
        /// </summary>
        /// <param name="count"></param>
        /// <param name="number"></param>
        public void GotoStep(int count, int number)
        {
            UnityTool.Log("...............");

            if (count == 0) return;

            if(!RefDict.ContainsKey(index)) RefDict.Add(index,number);

            if (RefDict[index] >= number)
            {
                MoveIndexToPosition(1); // 移向下一步并返回；
            }
            else
            {
                RefDict[index]++; // 引用计数加一；

                MoveIndexToStep(count);
            }
        }


        /// <summary>
        ///  循环执行指定的范围；
        /// 
        ///  格式为 loop:[step|position],count=(int)数值 ,number=数值；
        /// 
        ///  注意： 正直是向前定位，从指定为位置执行到当前位置； 负值是向后定位，从当前位置执行到指定位置； 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="number"></param>
        public void ExecuteLoopStep(int count, int number)
        {
            if (count == 0 || RefDict.ContainsKey(index)) return;

            RefDict.Add(index, number);

            int i_index=0;

            while (i_index <= number)
            {
                if (count < 0)
                {
                    MoveIndexToStep(count); // 移动指针；

                    ExecuteToNextStep((uint)-count);
                }
                else
                {
                    ExecuteToNextStep((uint)count);

                    MoveIndexToStep(-count);
                }
            }
                
            
        }

        #endregion


        #region 执行操作步骤, 按 Step 粗粒度执行；

        /// <summary>
        ///  执行一步操作，该步操作包含多个步骤，遇到 ## 停止；默认屏蔽数组；
        /// </summary>
        /// <returns></returns>
        public bool ExecuteNextStep(bool noSealedArray = false)
        {
            while (index + 1 < JsonItemList.Count && JsonItemList[index + 1].methodName != delimiter)
            {
                JsonData jsonData = NextStep<JsonData>();

                string methodName = JsonItemList[index].methodName;

#if UNITY_EDITOR
                if (ConfigurationInfo.ShowStateInfo)
                    
                    Debug.Log(string.Format(" {0} : {1}", methodName, jsonData.ToJson()));
#endif
                if (!DisposeInstruction(methodName, jsonData))

                    throw new Exception(methodName + " : " + jsonData[methodName]);

                if (!noSealedArray && jsonData.IsArray) break;
            }

            if (index<JsonItemList.Count && JsonItemList[index].methodName != delimiter) ++index;

            return true;
        }

        /// <summary>
        ///  执行客户端指定的具体 Step，组件将执行 相对于当前位置的第 count 个Step；
        /// 
        ///  count 默认 =1 ,表示默认执行相对于当前 Step 的下一个 Step；TODO
        /// 
        ///  注意： 若 count 为负值，则组件执行的位置是 当前步骤前面的步骤；
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ExecuteNextStep(int count, bool noSealedArray = false)
        {
            // 先定位到指定的位置；如果位置信息正确，则执行下一步；

            if (MoveIndexToStep(count - 1)) ExecuteNextStep(noSealedArray);

            return true;
        }


        /// <summary>
        /// 连续执行 count 个 step，到达指定的 step ；
        /// </summary>
        /// <param name="count"></param>
        /// <param name="noSealedArray"></param>
        /// <returns></returns>
        public bool ExecuteToNextStep(uint count, bool noSealedArray = false)
        {
            for (int i = 0; i < count; i++)
            {
                ExecuteNextStep(noSealedArray);
            }

            return true;
        }

        


        /// <summary>
        ///  执行客户端传递的函数，默认该函数只执行一次，不进行缓存；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public bool ExecuteNextStep<T>(Func<T, bool> func, bool dontDestroy = false)
        {
            string methodName = func.Method.Name.ToLower();

            JsonData jsonData = NextStep<JsonData>(methodName);

            if (!CustomFunctionLibrary<T>.ExternalDispose(methodName))
            {
                CustomFunctionLibrary<T>.Add(methodName, func, dontDestroy);
            }

            return DisposeInstruction(methodName, jsonData);
        }

        /// <summary>
        ///  执行客户端传递的函数，默认该函数只执行一次，不进行缓存；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="dontDestroy"></param>
        /// <returns></returns>
        public bool ExecuteNextStep<T>(Action action, bool dontDestroy = false)
        {
            string methodName = action.Method.Name.ToLower();

            JsonData jsonData = NextStep<JsonData>(methodName);

            if (!CustomFunctionLibrary<T>.ExternalDisposeA(methodName))
            {
                CustomFunctionLibrary<T>.Add(methodName, action, dontDestroy);
            }

            return DisposeInstruction(methodName, jsonData);
        }

        #endregion

        #region  执行操作步骤, 按 position 细粒度执行；


        /// <summary>
        ///  连续执行 count 个 position，到达指定的 position ；
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ExecuteToNextPosition(uint count)
        {
            if (count > JsonItemList.Count - index) throw new Exception("执行的步骤范围越界！");

            for (int i = 0; i < count; i++)
            {
                if (!ExecuteNextPositionByMethodName()) throw new Exception(string.Format("第{0}步有错；", i + 1));
            }

            // 将当前步骤设置为上一个步骤；

            MoveIndexToPosition(-1);

            return true;
        }



        /// <summary>
        ///  执行客户端指定的具体 Position，组件将执行 相对于当前位置的第 count 个Position；
        /// 
        ///  count 默认 =1 ,表示默认执行相对于当前位置的下一个位置；TODO
        /// 
        ///  注意： 若 count 为负值，则组件执行的位置是 当前步骤前面的步骤；
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool ExecuteNextPosition(int count=1)
        {
            // 移动指针；

            MoveIndexToPosition(count-1);

            // 执行步骤；

            ExecuteNextPositionByMethodName();

            return true;
        }


        /// <summary>
        ///  通过函数名，执行一个客户端已经注册的函数；
        /// 
        ///  如果没有该函数名，默认执行下一个函数；
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public bool ExecuteNextPositionByMethodName(string methodName = null)
        {
            JsonData jsonData = NextStep<JsonData>(methodName);

            if (methodName != null) methodName = methodName.ToLower();

            else
            {
                methodName = JsonItemList[index].methodName;

                // 如果该步骤是 ## 分隔符，则跳过该步骤；

                if (methodName == delimiter) return ExecuteNextPositionByMethodName();

            }

#if UNITY_EDITOR
            if (ConfigurationInfo.ShowStateInfo)
                
                Debug.Log(string.Format(" {0} : {1}", methodName, jsonData.ToJson()));
#endif
            return DisposeInstruction(methodName, jsonData);
        }


        #endregion

        #region 处理函数

        /// <summary>
        ///  先调用客户端注册的方法，进行处理；再调用组件内部方法进行处理；客户但方法会屏蔽组件内部方法；
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private bool DisposeInstruction(string methodName, JsonData jsonData)
        {
            if (CustomFunctionLibrary<JsonData>.ExternalDispose(methodName, jsonData)

                || CustomFunctionLibrary<JsonData>.ExternalDisposeAction(methodName)

                || LitJsonInstructionFactory.Dispose(jsonData, methodName)) return true;

            --index;

            return false;
        }

        #endregion

        #region 返回处理步骤的数据；


        /// <summary>
        /// 返回 index 处的数据；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stepName"></param>
        /// <returns></returns>
        public T CurStep<T>(string stepName = null)
        {
            if (stepName == null) return CastLitJsonData<T>(JsonItemList[index].JsonData);

            return GetStepData<T>(stepName);
        }

        /// <summary>
        ///  返回 index 处的上一步数据；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stepName"></param>
        /// <returns></returns>
        public T ProStep<T>(string stepName = null)
        {
            if (stepName == null) return CastLitJsonData<T>(JsonItemList[--index].JsonData);

            return GetStepData<T>(stepName, true);
        }

        /// <summary>
        ///  返回 index 处的下一步数据；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stepName"></param>
        /// <returns></returns>
        public T NextStep<T>(string stepName = null)
        {
            if (stepName == null) return CastLitJsonData<T>(JsonItemList[++index].JsonData);

            return GetStepData<T>(stepName);
        }


        /// <summary>
        ///  返回执行步骤的数据，组件返回 相对当前位置的第 count 个步骤的数据；
        /// 
        ///  注意： 若 count 为负数，返回的是当前步骤前面的步骤；  // 默认不改变指针位置，你也可以改变指针位置；TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
        public T NextPosition<T>(int count)
        {
            MoveIndexToPosition(count);

            return CurStep<T>();
        }


        /// <summary>
        /// 从 index 处往后找到第一个 stepName;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stepName"></param>
        /// <returns></returns>
        public T GetStepData<T>(string stepName, bool inverse = false)
        {
            if (inverse)
            {
                for (int i = 1; i <= index; i++)
                {
                    if (stepName.ToLower() == JsonItemList[i].methodName)
                    {
                        index = i;

                        return CastLitJsonData<T>(JsonItemList[i].JsonData);
                    }
                }
            }
            else
            {
                for (int i = index; i < JsonItemList.Count; i++)
                {
                    if (stepName.ToLower() == JsonItemList[i].methodName)
                    {
                        index = i;

                        return CastLitJsonData<T>(JsonItemList[i].JsonData);
                    }
                }
            }

            return default(T);
        }


        /// <summary>
        ///  对泛型 T 做一步解析；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private T CastLitJsonData<T>(JsonData jsonData)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)jsonData.ToJson();
            }

            return (T)(object)jsonData;
        }

        #endregion

    }
}