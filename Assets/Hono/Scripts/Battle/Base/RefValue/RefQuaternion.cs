using System;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace Hono.Scripts.Battle.RefValue
{
    [Serializable]
    public class RefQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public RefQuaternion()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 1; // 单位四元数
        }

        public RefQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public RefQuaternion(Quaternion quaternion)
        {
            x = quaternion.X;
            y = quaternion.Y;
            z = quaternion.Z;
            w = quaternion.W;
        }

        // 将当前实例转换为 Quaternion
        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }

        // 从 Quaternion 更新当前实例
        public void FromQuaternion(Quaternion quaternion)
        {
            x = quaternion.X;
            y = quaternion.Y;
            z = quaternion.Z;
            w = quaternion.W;
        }

        // 重载运算符，用于隐式转换
        public static implicit operator Quaternion(RefQuaternion wrapper)
        {
            return new Quaternion(wrapper.x, wrapper.y, wrapper.z, wrapper.w);
        }

        public static implicit operator RefQuaternion(Quaternion quaternion)
        {
            return new RefQuaternion(quaternion);
        }

        // 方法：四元数乘法
        public static RefQuaternion operator *(RefQuaternion a, RefQuaternion b)
        {
            return new RefQuaternion(a.ToQuaternion() * b.ToQuaternion());
        }

        // 方法：四元数与标量乘法
        public static RefQuaternion operator *(RefQuaternion a, float scalar)
        {
            Quaternion q = new Quaternion(a.x, a.y, a.z, a.w);
            q *= scalar;
            return new RefQuaternion(q);
        }

        // 方法：四元数逆
        public RefQuaternion Inverse()
        {
            return new RefQuaternion(Quaternion.Inverse(ToQuaternion()));
        }

        // 方法：四元数长度
        public float Magnitude()
        {
            return Mathf.Sqrt(x * x + y * y + z * z + w * w);
        }

        // 方法：归一化四元数
        public void Normalize()
        {
            float magnitude = Magnitude();
            if (magnitude > 0)
            {
                x /= magnitude;
                y /= magnitude;
                z /= magnitude;
                w /= magnitude;
            }
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }
    }
}