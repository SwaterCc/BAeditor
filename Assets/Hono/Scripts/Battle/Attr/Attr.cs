using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public interface IAttr
    {
        public void InitDefaultValue();
        public ICommand AutoSet(AutoValue obj, bool isTempData = false);
        public AutoValue GetAuto();
    }
    
    /// <summary>
    /// 属性基类
    /// </summary>
    public class Attr<T> : IAttr, ICommandCollection
    {
        private T _value;
        private Type _valueType;
        private LinkedList<ICommand> _commands;
        private readonly Func<T, T, T> _onCommandChanged;

        public Attr(Func<T, T, T> onCommandChanged)
        {
            _onCommandChanged = onCommandChanged;
            _valueType = typeof(T);
        }

        public ICommand AutoSet(AutoValue auto, bool isTempData = false)
        {
            if (auto.GetAutoType() is not T) return null;
            
            if (isTempData)
            {
                return new AttrCommand<T>(this, _value);
            }

            if (_valueType.IsClass)
            {
                _value = auto.GetRef<T>();
            }
            else
            {
                _value = (T)auto.Get<T>();
            }
            
            return null;
        }

        public AutoValue GetAuto()
        {
            var auto = new AutoValue();
            if (typeof(T).IsClass)
            {
                auto.SetRef(_value);
            }
            else
            {
                auto.Set<T>(_value);
            }

            return auto;
        }

        public T Get()
        {
            return _value;
        }

        public ICommand Set(T value, bool isTempData = false)
        {
            _value = value;
            if (isTempData || _onCommandChanged == null)
            {
                return new AttrCommand<T>(this, value);
            }

            return null;
        }

        public void InitDefaultValue() { }

        public Type GetAttrType()
        {
            return typeof(T);
        }

        public void AddCommand(ICommand command)
        {
            _commands ??= new LinkedList<ICommand>();
            _commands.AddLast(command);
        }

        public void RemoveCommand(ICommand command)
        {
            if (_commands != null)
            {
                _commands.Remove(command);
            }
        }

        public void OnCommandChanged()
        {
            if (_onCommandChanged == null)
            {
                Debug.LogWarning($"Attr<{typeof(T)}> _onCommandChanged == null");
                return;
            }

            foreach (AttrCommand<T> command in _commands)
            {
                _value = _onCommandChanged.Invoke(_value, command.Value);
            }
        }
    }
}