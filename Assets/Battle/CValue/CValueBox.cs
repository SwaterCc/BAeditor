using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Battle
{
    public interface IValueBox
    {
        public void Set(IValueBox newBox);
        public Type GetValueType();
    }
    
    /// <summary>
    /// 手动装箱管理，需要接对象池，规避内存分配的消耗和类型检测的消耗，TODO：目前缺少内存池实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueBox<T> : IValueBox
    {
        protected T _value;

        public ValueBox()
        {
            _value = default;
        }
        
        public ValueBox(T value)
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

        public void Set(IValueBox newBox)
        {
            var selfType = GetType();
            if (selfType.IsInstanceOfType(newBox))
            {
                Set(((ValueBox<T>)newBox).Get());
            }
            else
            {
                Debug.LogError("SetBOx failed!");
            }
        }

        public Type GetValueType()
        {
            return GetType();
        }
    }
}