using System;

namespace Hono.Scripts.Battle.Base
{
    [Serializable]
    public abstract class ARef : IAPoolRefCount
    {
        [NonSerialized]
        private int _refCount;
        
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
    }
}