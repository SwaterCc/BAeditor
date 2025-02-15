#region

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;
using Random = System.Random;

#endregion

namespace Hono.Scripts.Battle.Tools
{
    public static class CommonUtility
    {
        /// <summary>
        ///     根据时间使用MD5进行哈希计算并返回一个32位整型值
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

        public static bool GetCompareResult(ECompareResType compareResType, int flag)
        {
            switch (compareResType)
            {
                case ECompareResType.Less:
                    return flag < 0;
                case ECompareResType.LessAndEqual:
                    return flag <= 0;
                case ECompareResType.Equal:
                    return flag == 0;
                case ECompareResType.More:
                    return flag > 0;
                case ECompareResType.MoreAndEqual:
                    return flag >= 0;
            }

            return true;
        }

        public class IdGenerator
        {
            private int currentId = 0;

            /// <summary>
            ///     生成新的ID。ID 会自增并且始终大于0。
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
        ///     获取Id生成器
        /// </summary>
        /// <returns></returns>
        public static IdGenerator GetIdGenerator()
        {
            return new IdGenerator();
        }


        private static readonly Random Random = new();

        /// <summary>
        ///     从一个List<int>中随机选取指定数量的对象。
        /// </summary>
        /// <param name="list">源列表</param>
        /// <returns>选取的对象列表</returns>
        public static void Shuffle<T>(ref List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = Random.Next(0, i + 1);

                // 交换位置 list[i] 和 list[j]
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public static List<T> GetRandomItems<T>(List<T> list, int count)
        {
            // 检查参数有效性
            if (count > list.Count)
            {
                throw new ArgumentException("Count cannot be greater than the number of elements in the list.");
            }

            List<T> copyList = new List<T>(list);
            List<T> result = new List<T>();

            // Fisher-Yates 洗牌算法
            for (int i = 0; i < count; i++)
            {
                int index = Random.Next(i, copyList.Count);
                (copyList[i], copyList[index]) = (copyList[index], copyList[i]);
                result.Add(copyList[i]);
            }

            return result;
        }


        private static RayCastHitChecker _rayCastHitChecker = new();

        public static bool HitRayCast(CheckBoxData data, Vector3 centerPos, Quaternion rot, ref List<int> actorIds,
            bool showGizmos = false)
        {
            if (actorIds == null)
            {
                Debug.LogError("HitRayCast 不允许传入空的actorIds");
                return false;
            }

            Profiler.BeginSample("RayCastHitChecker");
            _rayCastHitChecker.OpenGizmos = showGizmos;
            int size = _rayCastHitChecker.GetHitActor(data, centerPos, rot, ref actorIds);
            _rayCastHitChecker.OpenGizmos = false;
            Profiler.EndSample();
            return size > 0;
        }
    }

    public static class ListExtensions
    {
        /// <summary>
        /// 移除列表中第一个匹配的元素，并将其与最后一个元素交换，高效地移除元素。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型</typeparam>
        /// <param name="list">要操作的列表</param>
        /// <param name="item">要移除的元素</param>
        /// <returns>如果成功移除元素则返回 true，否则返回 false</returns>
        public static bool RemoveSwapBack<T>(this List<T> list, T item)
        {
            // 找到元素的索引
            int index = list.IndexOf(item);
            if (index >= 0)
            {
                int lastIndex = list.Count - 1;

                if (index < lastIndex)
                {
                    // 将要删除的元素与最后一个元素交换
                    list[index] = list[lastIndex];
                }

                // 移除最后一个元素
                list.RemoveAt(lastIndex);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 使用自定义比较器移除列表中第一个匹配的元素，并将其与最后一个元素交换，高效地移除元素。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型</typeparam>
        /// <param name="list">要操作的列表</param>
        /// <param name="match">用于确定要移除的元素的条件</param>
        /// <returns>如果成功移除元素则返回 true，否则返回 false</returns>
        public static bool RemoveSwapBack<T>(this List<T> list, Predicate<T> match)
        {
            // 找到符合条件的元素的索引
            int index = list.FindIndex(match);
            if (index >= 0)
            {
                int lastIndex = list.Count - 1;

                if (index < lastIndex)
                {
                    // 将要删除的元素与最后一个元素交换
                    list[index] = list[lastIndex];
                }

                // 移除最后一个元素
                list.RemoveAt(lastIndex);
                return true;
            }

            return false;
        }
    }
}