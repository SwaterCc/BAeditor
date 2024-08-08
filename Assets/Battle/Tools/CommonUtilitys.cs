
using System;
using System.Security.Cryptography;
using System.Text;

namespace BattleAbility
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
    }
}
