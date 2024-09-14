using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public interface IAttr {
		public void InitDefaultValue();
		public ICommand BoxSet(object obj, bool isTempData = false);
		public object GetBox(bool onlyBaseValue = false);
	}

	/// <summary>
	/// 属性基类
	/// </summary>
	public class Attr<T> : IAttr, ICommandCollection {
		private T _commandValue;
		private T _setValue;
		private LinkedList<ICommand> _commands;
		private readonly Func<T, T, T> _onCommandChanged;

		public Attr(Func<T, T, T> onCommandChanged) {
			_onCommandChanged = onCommandChanged;
		}
		
		public object GetBox(bool onlyBaseValue = false) {
			if (onlyBaseValue || _onCommandChanged == null) {
				return _setValue;
			}

			return _commands?.Count > 0 ? _onCommandChanged.Invoke(_setValue, _commandValue) : _setValue;
		}
		
		public T Get(bool onlyBaseValue = false) {
			if (onlyBaseValue || _onCommandChanged == null) {
				return _setValue;
			}

			return _commands?.Count > 0 ? _onCommandChanged.Invoke(_setValue, _commandValue) : _setValue;
		}

		public ICommand Set(T value, bool isTempData = false) {
			if (isTempData && _onCommandChanged != null) {
				return new AttrCommand<T>(this, value);
			}
			_setValue = value;
			return null;
		}
		
		public ICommand BoxSet(object obj, bool isTempData = false) {
			if (obj is not T objTyped) {
				throw new InvalidCastException("尝试存储一个类型错误的值");
			}

			if (isTempData && _onCommandChanged != null) {
				return new AttrCommand<T>(this, objTyped);
			}
			_setValue = objTyped;
			//报个错
			return null;
		}


		public void InitDefaultValue() { }

		public Type GetAttrType() {
			return typeof(T);
		}

		public void AddCommand(ICommand command) {
			_commands ??= new LinkedList<ICommand>();
			_commands.AddLast(command);
		}

		public void RemoveCommand(ICommand command) {
			if (_commands != null) {
				_commands.Remove(command);
			}
		}

		public void OnCommandChanged() {
			if (_onCommandChanged == null) {
				Debug.LogWarning($"Attr<{typeof(T)}> _onCommandChanged == null");
				return;
			}

			_commandValue = default;
			foreach (AttrCommand<T> command in _commands) {
				_commandValue = _onCommandChanged.Invoke(_commandValue, command.Value);
			}
		}
	}
}