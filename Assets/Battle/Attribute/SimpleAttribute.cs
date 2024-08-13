using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 简单属性
    /// 仅作存储的属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleAttribute<T> : Attribute, IAttrCommandHandle<T>
    {
        private readonly LinkedList<AttrCommand<T>> _commands;
        private readonly EAttrCommandType _commandType;
        private T _value;
        private readonly Func<T, T, T> _add;

        public SimpleAttribute(bool openCommandCache = false, EAttrCommandType commandType = EAttrCommandType.Override,
            Func<T, T, T> add = null)
        {
            if (openCommandCache)
            {
                //值类型修改会记录修改记录
                if (!typeof(T).IsStruct())
                {
                    //发个警告
                }
                else
                {
                    _commands = new LinkedList<AttrCommand<T>>();
                }
            }

            _commandType = commandType;
            _value = default;
            _add = add;
        }

        private void updateValue()
        {
            switch (_commandType)
            {
                case EAttrCommandType.Add:
                    foreach (var command in _commands)
                    {
                        _value = _add.Invoke(_value, command.Value);
                    }

                    break;
                case EAttrCommandType.Override:
                    _value = _commands.Last.Value.Value;
                    break;
            }
        }

        private void onlySet(T value)
        {
            _value = value;
        }

        public void Set(AttrCommand<T> handle)
        {
            if (_commands == null)
            {
                //警告下
                onlySet(handle.Value);
            }
            else
            {
                _commands.AddLast(handle);
                updateValue();
            }
        }

        public AttrCommand<T> Set(T value, bool cancelCache = false)
        {
            if (_commands == null || cancelCache)
            {
                if (!cancelCache)
                {
                    //警告下
                }

                onlySet(value);
                return null;
            }
            else
            {
                var handle = new AttrCommand<T>(this, value);
                _commands.AddLast(handle);
                updateValue();
                return handle;
            }
        }

        public override ICValueBox GetBox()
        {
            return new CValueBox<T>(_value);
        }
        
        public T GetValue()
        {
            return _value;
        }

        public void Undo(AttrCommand<T> command)
        {
            if (_commands == null)
                return;

            var node = _commands.Find(command);
            if (node != null)
            {
                _commands.Remove(node);
                if (_commands.Count > 0)
                {
                    updateValue();
                }
                else
                {
                    _value = default;
                }
            }
            else
            {
                Debug.LogError("Undo Failed!");
            }
        }
    }
}