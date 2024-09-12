using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class AutoValue
    {
        private bool _boolValue;
        private int _intValue;
        private float _floatValue;
        private object _ref;
        private Type _finalType;

        private void check<T>()
        {
            if (_finalType != typeof(T))
                Debug.LogWarning($"AutoValue finalType is {_finalType} but cast to {typeof(T)}");
        }

        public AutoValue DeepCopy()
        {
            var clone = (AutoValue)this.MemberwiseClone();

            // 处理引用类型的深拷贝
            if (_ref is ICloneable cloneableRef)
            {
                clone._ref = cloneableRef.Clone();
            }
            else if (_ref != null && !(_ref is ValueType))
            {
                throw new InvalidOperationException(
                    $"The referenced object of type {_ref.GetType()} does not implement ICloneable and cannot be deeply copied.");
            }

            return clone;
        }

        public Type GetAutoType()
        {
            return _finalType;
        }

        public object Get<T>()
        {
            var type = typeof(T);
            
            if (type == typeof(int))
            {
                return _intValue;
            }

            if (type == typeof(float))
            {
                return _floatValue;
            }

            if (type == typeof(bool))
            {
                return _boolValue;
            }

            return _ref;
        }

        public void Set<T>(object value)
        {
            var type = typeof(T);
            _finalType = type;
            if (type.IsClass)
            {
                _ref = value;
                return;
            }

            if (type == typeof(int))
            {
                _intValue = Convert.ToInt32(value);
            }
            else if (type == typeof(float))
            {
                _floatValue = Convert.ToSingle(value);
            }
            else if (type == typeof(bool))
            {
                _boolValue = Convert.ToBoolean(value);
            }
            else
            {
                Debug.LogWarning($"AutoSet会将未定义的值类型装箱，该值类型为{type}");
                _ref = value;
            }
        }

        public void SetRef<T>(T classObj)
        {
            if (typeof(T).IsValueType)
            {
                throw new Exception($"使用值类型方法存储引用类型 {nameof(T)}!");
            }

            if (classObj == null)
            {
                Debug.LogWarning("尝试缓存一个null");
            }

            _finalType = typeof(T);
            _ref = classObj;
        }

        public int GetInt() => _intValue;
        public float GetFloat() => _floatValue;
        public bool GetBool() => _boolValue;
        public string GetString() => this;

        public T GetRef<T>()
        {
            if (_ref is T castRef)
            {
                return castRef;
            }

            throw new InvalidCastException("AutoValue GetRef Failed!");
        }

        public static implicit operator int(AutoValue auto)
        {
            auto.check<int>();
            return auto._intValue;
        }

        public static implicit operator float(AutoValue auto)
        {
            auto.check<float>();
            return auto._floatValue;
        }

        public static implicit operator bool(AutoValue auto)
        {
            auto.check<bool>();
            return auto._boolValue;
        }

        public static implicit operator string(AutoValue auto)
        {
            auto.check<string>();
            return (string)auto._ref;
        }

        public static implicit operator AutoValue(int value)
        {
            var auto = new AutoValue
            {
                _intValue = value,
                _finalType = value.GetType()
            };
            return auto;
        }

        public static implicit operator AutoValue(float value)
        {
            var auto = new AutoValue
            {
                _floatValue = value,
                _finalType = value.GetType()
            };
            return auto;
        }

        public static implicit operator AutoValue(bool value)
        {
            var auto = new AutoValue
            {
                _boolValue = value,
                _finalType = value.GetType()
            };
            return auto;
        }

        public static implicit operator AutoValue(string value)
        {
            var auto = new AutoValue
            {
                _ref = value,
                _finalType = value.GetType()
            };
            return auto;
        }
    }
}