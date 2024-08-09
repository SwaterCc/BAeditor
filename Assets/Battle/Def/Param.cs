using System;
using System.Collections.Generic;
using Battle.Auto.PropertyEnum;
using UnityEngine;

namespace Battle
{
    public sealed class Param
    {
        public class VariableParam
        {
            public EVariableRange Range;
            public string Name;
        }

        public class PropertyParam
        {
            public EPropertyType PropertyType;
        }

        public EParamType ParamType;

        private int _intValue;
        private long _longValue;
        private float _floatValue;
        private bool _boolValue;

        private object _obj;
        private VariableParam _variableParam;
        private PropertyParam _propertyParam;

        public bool IsVariable => ParamType == EParamType.Variable && _propertyParam != null;
        public bool IsProperty => ParamType == EParamType.Property && _variableParam != null;
        public bool NotCustom => ParamType is not (EParamType.Property or EParamType.Variable);

        public Param(EParamType paramType)
        {
            ParamType = paramType;
        }

        public bool TryGetInt(out int value)
        {
            value = default;
            if (ParamType == EParamType.Int)
            {
                value = _intValue;
                return true;
            }

            return false;
        }

        public bool TryGetLong(out long value)
        {
            value = default;
            if (ParamType == EParamType.Long)
            {
                value = _longValue;
                return true;
            }

            return false;
        }

        public bool TryGetFloat(out float value)
        {
            value = default;
            if (ParamType == EParamType.Float)
            {
                value = _floatValue;
                return true;
            }

            return false;
        }

        public bool TryGetBool(out bool value)
        {
            value = default;
            if (ParamType == EParamType.Bool)
            {
                value = _boolValue;
                return true;
            }

            return false;
        }

        public TClass GetClassObj<TClass>() where TClass : class
        {
            return _obj as TClass;
        }

        public VariableParam GetVariableParam()
        {
            return _variableParam;
        }

        public PropertyParam GetPropertyParam()
        {
            return _propertyParam;
        }
    }
}