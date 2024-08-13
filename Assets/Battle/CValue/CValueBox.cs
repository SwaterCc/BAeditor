using System;
using Unity.VisualScripting;
using UnityEngine.Pool;

namespace Battle
{
    public interface ICValueBox 
    {
        
    }
    
    /// <summary>
    /// 手动装箱管理，需要接对象池，规避内存分配的消耗和类型检测的消耗，TODO：目前缺少内存池实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CValueBox<T> : ICValueBox
    {
        protected T _value;

        public CValueBox()
        {
            _value = default;
        }
        
        public CValueBox(T value)
        {
            _value = value;
        }
        
        public T Get()
        {
            return _value;
        }

        public void Set(T value)
        {
            _value = value;
        }
    }
}