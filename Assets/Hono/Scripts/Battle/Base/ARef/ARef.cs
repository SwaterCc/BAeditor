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

        public static Type ParseValueTypeToARefType(Type paramType)
        {
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
                return  typeof(RefBoolean);
            }

            if (paramType == typeof(Vector3))
            {
                return typeof(RefVector3);
            }

            return paramType;
        }
        
        public static Type ParseARefTypeToValueType(Type paramType)
        {
            if (paramType == typeof(RefInt))
            {
                return typeof(int);
            }

            if (paramType == typeof(RefFloat))
            {
                return typeof(float);
            }

            if (paramType == typeof(RefBoolean))
            {
                return  typeof(bool);
            }

            if (paramType == typeof(RefVector3))
            {
                return typeof(Vector3);
            }

            return paramType;
        }
    }
}