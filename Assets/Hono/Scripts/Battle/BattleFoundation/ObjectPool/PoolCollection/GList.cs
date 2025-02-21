using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hono.Scripts.Battle.ObjectPool {
	/// <summary>
	/// 池化list
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GList<T> : IGPoolObject, IEnumerable {
		private readonly List<T> _list;

		public int Count => _list.Count;

		public GList() {
			_list = new(10);
		}

		public GList(int capacity = 10) {
			_list = new(capacity);
		}

		public T this[int idx] => _list[idx];

		public static implicit operator List<T>(GList<T> list) {
			return list._list;
		}

		public IEnumerator<T> GetEnumerator() {
			foreach (var item in _list) {
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void Add(T item) {
			_list.Add(item);
			if (item is IAPoolRefCount refCount) {
				refCount.RefCount.AddReference();
			}
		}

		public void AddRange(IEnumerable<T> enumerable) {
			foreach (var item in enumerable) {
				if (item is IAPoolRefCount refCount) {
					refCount.RefCount.AddReference();
				}
			}

			_list.AddRange(enumerable);
		}

		public bool Contains(T item) {
			return _list.Contains(item);
		}

		public bool Remove(T item) {
			if (_list.Remove(item)) {
				if (item is IGPoolObject poolObject) {
					GPoolManager.Instance.RecycleAObject(poolObject);
				}
			}

			return false;
		}

		public void Clear() {
			foreach (var item in _list) {
				if (item is IGPoolObject poolObject) {
					GPoolManager.Instance.RecycleAObject(poolObject);
				}
			}

			_list.Clear();
		}

		public void OnRecycle() {
			Clear();
		}
	}
}