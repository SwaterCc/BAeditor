using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public class AttrCollection {
		private readonly Func<int, IAttr> _creator;

		public AttrCollection(Func<int, IAttr> creator) {
			_creator = creator;
		}

		private readonly Dictionary<int, IAttr> _attrs = new(128);

		private IAttr getAttrAndSetDefault(int logicAttr) {
			var attr = _creator.Invoke(logicAttr);
			if (attr == null) {
				throw new NullReferenceException($"create attr {logicAttr} Failed!");
			}

			attr.InitDefaultValue();
			return attr;
		}

		public ICommand SetAttr<T>(int attrType, T value, bool isTempData) {
			var attrTypeInt = attrType;

			if (!_attrs.TryGetValue(attrTypeInt, out var attr)) {
				attr = getAttrAndSetDefault(attrType);
				_attrs.Add(attrTypeInt, attr);
			}

			if (attr is Attr<T> typedAttr) {
				return typedAttr.Set(value, isTempData);
			}
			else {
				Debug.LogError($"attrType {attrType} 没有{typeof(T)}类型的实现！");
			}
			
			return null;
		}

		//TODO：性能问题
		public ICommand SetAttrBox(int attrType, object value, bool isTempData) {
			var attrTypeInt = attrType;

			if (!_attrs.TryGetValue(attrTypeInt, out var attr)) {
				attr = getAttrAndSetDefault(attrType);
				_attrs.Add(attrTypeInt, attr);
			}

			return attr.BoxSet(value, isTempData);
		}

		//TODO：性能问题
		public object GetAttrBox(int attrType) {
			if (_attrs.TryGetValue(attrType, out var attr)) {
				return attr.GetBox();
			}

			throw new KeyNotFoundException($"Attribute type {attrType} not found");
		}

		public T GetAttr<T>(int attrType) {
			if (_attrs.TryGetValue(attrType, out var attr)) {
				if (attr is Attr<T> typedAttr) {
					return typedAttr.Get();
				}
				else {
					throw new InvalidCastException($"Cannot cast attribute of type {attrType} to {typeof(T)}");
				}
			}

			throw new KeyNotFoundException($"Attribute type {attrType} not found");
		}
	}
	
	
	public static class AttrEnumExtensions {
		public static int ToInt(this ELogicAttr logicAttr) {
			return (int)logicAttr;
		}

		public static int ToInt(this EShowAttr attr) {
			return (int)attr;
		}
	}
}