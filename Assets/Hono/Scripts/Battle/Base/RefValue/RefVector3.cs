using UnityEngine;

namespace Hono.Scripts.Battle.RefValue
{
    public class RefVector3
    {
        public float x;
        public float y;
        public float z;

        public RefVector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public RefVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public RefVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        // 将当前实例转换为 Vector3
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        // 从 Vector3 更新当前实例
        public void FromVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        // 重载运算符，用于隐式转换
        public static implicit operator Vector3(RefVector3 wrapper)
        {
            return new Vector3(wrapper.x, wrapper.y, wrapper.z);
        }

        public static implicit operator RefVector3(Vector3 vector)
        {
            return new RefVector3(vector);
        }

        // 方法：向量加法
        public static RefVector3 operator +(RefVector3 a, RefVector3 b)
        {
            return new RefVector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        // 方法：向量减法
        public static RefVector3 operator -(RefVector3 a, RefVector3 b)
        {
            return new RefVector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        // 方法：向量数乘
        public static RefVector3 operator *(RefVector3 a, float scalar)
        {
            return new RefVector3(a.x * scalar, a.y * scalar, a.z * scalar);
        }

        // 方法：向量数除
        public static RefVector3 operator /(RefVector3 a, float scalar)
        {
            return new RefVector3(a.x / scalar, a.y / scalar, a.z / scalar);
        }

        // 方法：求向量长度
        public float Magnitude()
        {
            return Mathf.Sqrt(x * x + y * y + z * z);
        }

        // 方法：归一化向量
        public void Normalize()
        {
            float magnitude = Magnitude();
            if (magnitude > 0)
            {
                x /= magnitude;
                y /= magnitude;
                z /= magnitude;
            }
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }
}