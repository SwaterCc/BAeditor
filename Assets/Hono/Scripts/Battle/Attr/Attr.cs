using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public interface IAttr {
		public void InitDefaultValue();
		public ICommand BoxSet(object obj, bool isTempData = false);
		public object GetBox();
	}

	/// <summary>
	/// 属性基类
	/// </summary>
	public class Attr<T> : IAttr, ICommandCollection {
		private T _value;
		private LinkedList<ICommand> _commands;
		private Func<T, T, T> _onCommandChanged;

		public Attr(Func<T, T, T> onCommandChanged) {
			_onCommandChanged = onCommandChanged;
		}
		
		public ICommand BoxSet(object obj, bool isTempData = false) {
			if (obj is T objTyped) {
				if (isTempData) {
					return new AttrCommand<T>(this, (T)obj);
				}
				else {
					_value = objTyped;
				}
			}
			else {
				//报个错
			}

			return null;
		}

		public object GetBox() {
			return _value;
		}
		
		public T Get() {
			return _value;
		}

		public ICommand Set(T value, bool isTempData = false) {
			if (isTempData) {
				return new AttrCommand<T>(this, value);
			}
			_value = value;
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
			foreach (AttrCommand<T> command in _commands) {
				_value = _onCommandChanged.Invoke(_value, command.Value);
			}
		}
	}
}