using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Core.Base
{
    /// <summary>
    /// 因为官方Vector3没使用序列化接口所以包装一下
    /// </summary>
    [Serializable]
    public struct SerializableVec3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // 重载运算符，用于隐式转换
        public static implicit operator Vector3(SerializableVec3 wrapper)
        {
            return new Vector3(wrapper.x, wrapper.y, wrapper.z);
        }
    }
}