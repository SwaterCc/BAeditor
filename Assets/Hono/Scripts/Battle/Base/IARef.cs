namespace Hono.Scripts.Battle.Base
{
    public abstract class ARef : IAPoolRefCount
    {
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