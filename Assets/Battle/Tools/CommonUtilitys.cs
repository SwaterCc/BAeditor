using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Battle.Tools
{
    public static class CommonUtility
    {
        /// <summary>
        /// 根据时间使用MD5进行哈希计算并返回一个32位整型值
        /// </summary>
        /// <returns></returns>
        public static int GenerateTimeBasedHashId32()
        {
            // 获取当前时间，并转化为字符串格式
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            // 使用MD5进行哈希计算并返回一个32位整型值
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(currentTime);
                byte[] hashBytes = md5.ComputeHash(bytes);

                // 将前4个字节转换为32位整型值
                int hashValue = BitConverter.ToInt32(hashBytes, 0);
                hashValue = Math.Abs(hashValue);
                return hashValue;
            }
        }

        public class IdGenerator
        {
            private int currentId = 0;

            /// <summary>
            /// 生成新的ID。ID 会自增并且始终大于0。
            /// </summary>
            /// <returns>新的ID</returns>
            public int GenerateId()
            {
                // 使用Interlocked.Increment确保线程安全
                int newId = Interlocked.Increment(ref currentId);

                // 检查是否溢出（如果超出int.MaxValue，重置为1）
                if (newId == int.MaxValue)
                {
                    Interlocked.Exchange(ref currentId, 0);
                    newId = Interlocked.Increment(ref currentId);
                }

                return newId;
            }
        }

        /// <summary>
        /// 获取Id生成器
        /// </summary>
        /// <returns></returns>
        public static IdGenerator GetIdGenerator()
        {
            return new IdGenerator();
        }
    }
    
    #region ParamExtension

    public static class ParamExtension
    {
        public static bool TryCallFunc(this Param[] @params, out IValueBox valueBox)
        {
            var queue = new Queue<Param>(@params);
            return TryCallFunc(queue, out valueBox);
        }

        public static bool TryCallFunc(this Queue<Param> queue, out IValueBox valueBox)
        {
            valueBox = null;
            var func = queue.Dequeue();
            if (func.IsFunc)
            {
                valueBox = queue.CallFunc(func);
                return true;
            }

            Debug.LogError("队首不是函数");
            return false;
        }

        /// <summary>
        /// 执行指定函数
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IValueBox CallFunc(this Queue<Param> queue, Param func)
        {
            var funcInfo = AbilityPreLoad.GetFuncInfo(func.FuncName);
            object[] funcParams = new object[funcInfo.ParamCount];

            for (int idx = 0; idx < funcInfo.ParamCount; idx++)
            {
                var param = queue.Dequeue();
                
                if (param.IsValueType)
                {
                    funcParams[idx] = param.Value;
                }

                if (param.IsFunc)
                {
                    funcParams[idx] = CallFunc(queue, param);
                }
            }

            //TODO:有消耗
            var res = (IValueBox)funcInfo.Invoke(null, funcParams);
            return res;
        }
    }

    #endregion
}