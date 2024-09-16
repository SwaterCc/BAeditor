using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class AutoValue
    {
        private bool _boolValue;
        private int _intValue;
        private float _floatValue;
        private string _stringValue;
        private object _serializableRef;
        private Type _finalType;

        private void check<T>()
        {
            if (_finalType != typeof(T))
                Debug.LogWarning($"AutoValue finalType is {_finalType} but cast to {typeof(T)}");
        }

        public Type GetAutoType()
        {
            return _finalType;
        }
        
        public int GetInt() => _intValue;
        public float GetFloat() => _floatValue;
        public bool GetBool() => _boolValue;
        public string GetString() => this;

        public T GetSerializableRef<T>() where T : class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new InvalidCastException("必须是可序列化对象");
            }

            return (T)_serializableRef;
        }
        
        public void SetSerializableRef<T>(T obj) where T : class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new InvalidCastException("必须是可序列化对象");
            }

            _serializableRef = obj;
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
            return auto._stringValue;
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
                _stringValue = value,
                _finalType = value.GetType()
            };
            return auto;
        }
    }
}