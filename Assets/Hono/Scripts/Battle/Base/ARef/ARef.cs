using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Base
{
    [Serializable]
    public abstract class ARef : IAPoolRefCount
    {
        [NonSerialized]
        private int _refCount;

        public abstract Type GetValueType();

        public abstract ARef DeepCopy();

        public abstract void ARefRecycle();

        public void AddReference()
        {
            ++_refCount;
        }

        public void RemoveReference()
        {
            --_refCount;
            if (_refCount <= 0)
            {
                ARefRecycle();
            }
        }

        public int GetReferenceCount()
        {
            return _refCount;
        }

        public static Type GetRefType<T>() where T : struct
        {
            var paramType = typeof(T);

            if (paramType == typeof(int))
            {
                return typeof(RefInt);
            }

            if (paramType == typeof(float))
            {
                return typeof(RefFloat);
            }

            if (paramType == typeof(bool))
            {
                return typeof(RefBoolean);
            }

            if (paramType == typeof(Vector3))
            {
                return typeof(RefVector3);
            }

            return null;
        }

        public static bool TryGetRefType(Type paramType, out Type refType)
        {
            refType = null;

            if (paramType.IsClass || paramType.IsEnum || paramType == typeof(void))
            {
                refType = paramType;
                return true;
            }

            if (paramType == typeof(int))
            {
                refType = typeof(RefInt);
                return true;
            }

            if (paramType == typeof(float))
            {
                refType = typeof(RefFloat);
                return true;
            }

            if (paramType == typeof(bool))
            {
                refType = typeof(RefBoolean);
                return true;
            }

            if (paramType == typeof(Vector3))
            {
                refType = typeof(RefVector3);
                return true;
            }

            return false;
        }
    }
}