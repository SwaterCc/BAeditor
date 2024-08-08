using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

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
}
