#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Base
{
    public class AFuncParams : IAPoolObject
    {
        private readonly Queue<object> _paramQueue = new(10);
        public int Count => _paramQueue.Count;

        public void Push<T>(T obj) where T : class
        {
            _paramQueue.Enqueue(obj);
        }

        public T Pop<T>() where T : class
        {
            return (T)_paramQueue.Dequeue();
        }

        public void OnRecycle()
        {
            foreach (var obj in _paramQueue)
            {
                if (obj is IAPoolObject poolObject)
                {
                    ObjectPoolManager.Instance.RecycleAObject(poolObject);
                }
            }
            _paramQueue.Clear();
        }
    }

    
    public static class AParamsParser
    {
        public static bool TryParse<T>(this AParams aParams, in Ability ability, out T value) where T : class
        {
            value = null;
            object objectValue = null;
            if (aParams == null)
            {
                return false;
            }

            switch (aParams.paramType)
            {
                case EParamType.Simple:
                    objectValue = aParams.Value;
                    break;
                case EParamType.Function:
                    if (!aParams.TryCallFunction(ability, out objectValue))
                    {
                        return false;
                    }

                    break;
                case EParamType.Variable:
                    objectValue = ability.Vairables.Get(aParams.variableName);
                    break;
                case EParamType.Attr:
                    objectValue = ability.Actor.GetAttr(aParams.attrType);
                    break;
            }

            value = (T)objectValue;

            return true;
        }

        public static object Parse(this AParams aParams, in Ability ability)
        {
            if (aParams == null)
            {
                return null;
            }

            object value = null;

            switch (aParams.paramType)
            {
                case EParamType.Simple:
                    value = aParams.Value;
                    break;
                case EParamType.Function:
                    if (!aParams.TryCallFunction(ability, out value))
                    {
                        return null;
                    }

                    break;
                case EParamType.Variable:
                    value = ability.Vairables.Get(aParams.variableName);
                    break;
                case EParamType.Attr:
                    value = ability.Actor.GetAttr(aParams.attrType);
                    break;
            }

            return value;
        }

        public static bool TryCallFunction(this AParams aParams, in Ability ability, out object value)
        {
            value = null;
            if (string.IsNullOrEmpty(aParams.funcName))
            {
                Debug.LogError("函数名为空");
                return false;
            }

            var wrap = Ability.AFuncInvoker.Instance.Get(aParams.funcName);
            if (wrap == null)
            {
                Debug.LogError($"获取函数失败{aParams.funcName}");
                return false;
            }

            var @params = APool<AFuncParams>.Pool.Rent();
            for (var index = 0; index < aParams.funcParams.Count; index++)
            {
                var funcParam = aParams.funcParams[index];
                @params.Push(funcParam.Parse(ability));
            }
            wrap.Invoke(ability, @params, out value);
            APool<AFuncParams>.Pool.Recycle(@params);
            
            return true;
        }
    }
}