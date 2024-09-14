using System;
using System.Collections;
using System.Collections.Generic;

namespace Hono.Scripts.Battle {
	public class IntArray : IEnumerable<int> {
		private readonly List<int> _array;

		public IntArray(string csv) {
			_array = new List<int>();
			if (string.IsNullOrEmpty(csv)) return;
			var intStr = csv.Split("=");
			foreach (var intValue in intStr) {
				_array.Add(int.Parse(intValue));
			}
		}

		public static implicit operator List<int>(IntArray array) {
			List<int> list = new();
			list.AddRange(array._array);
			return list;
		}
		
		public int this[int idx] => _array[idx];

		public IEnumerator<int> GetEnumerator() {
			foreach (var num in _array) {
				yield return num;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}

	public class NumberArray : IEnumerable<float> {
		private readonly List<float> _array;

		public NumberArray(string csv) {
			_array = new List<float>();
			var intStr = csv.Split("=");
			if (string.IsNullOrEmpty(csv)) return;
			foreach (var floatValue in intStr) {
				_array.Add(float.Parse(floatValue));
			}
		}

		public static implicit operator List<float>(NumberArray array) {
			List<float> list = new();
			list.AddRange(array._array);
			return list;
		}
		
		public float this[int idx] => _array[idx];

		public IEnumerator<float> GetEnumerator() {
			foreach (var num in _array) {
				yield return num;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}

	public class StringArray : IEnumerable<string> {
		private readonly List<string> _array;

		public StringArray(string csv) {
			_array = new List<string>();
			var intStr = csv.Split("=");
			if (string.IsNullOrEmpty(csv)) return;
			foreach (var value in intStr) {
				_array.Add(value);
			}
		}

		public string this[int idx] => _array[idx];

		public static implicit operator List<string>(StringArray array) {
			List<string> list = new();
			list.AddRange(array._array);
			return list;
		}
		
		public IEnumerator<string> GetEnumerator() {
			foreach (var str in _array) {
				yield return str;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}

	public class IntTable : IEnumerable<IntArray> {
		private List<IntArray> _tables;

		public IntTable(string csv) {
			_tables = new List<IntArray>();
			if (string.IsNullOrEmpty(csv)) return;
			foreach (var arrayStr in csv.Split("|")) {
				_tables.Add(new IntArray(arrayStr));
			}
		}

		public IntArray this[int idx] => _tables[idx];

		public static implicit operator List<List<int>>(IntTable intTable) {
			List<List<int>> table = new();
			foreach (var array in intTable) {
				table.Add(array);
			}
			return table;
		}
		
		public IEnumerator<IntArray> GetEnumerator() {
			foreach (var item in _tables) {
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}

	public class NumberTable : IEnumerable<NumberArray> {
		private List<NumberArray> _tables;

		public NumberTable(string csv) {
			_tables = new List<NumberArray>();
			if (string.IsNullOrEmpty(csv)) return;
			foreach (var arrayStr in csv.Split("|")) {
				_tables.Add(new NumberArray(arrayStr));
			}
		}

		public NumberArray this[int idx] => _tables[idx];

		public static implicit operator List<List<float>>(NumberTable numTable) {
			List<List<float>> table = new();
			foreach (var array in numTable) {
				table.Add(array);
			}
			return table;
		}
		
		public IEnumerator<NumberArray> GetEnumerator() {
			foreach (var iNumberArray in _tables) {
				yield return iNumberArray;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}

	public abstract class TableRow {
		public int Id => _id;
		private int _id; 
		private readonly Dictionary<string, object> _rowKeyValue = new();

		public object TryGet(string filedName, out object value) {
			return _rowKeyValue.TryGetValue(filedName, out value);
		}
		
		public CSVParser Parser { get; protected set; }

		public abstract class CSVParser {
			protected TableRow _row;

			protected CSVParser(TableRow row) {
				_row = row;
			}

			public void Parse(string line) {
				var datas = line.Split(",");
				_row._id = int.Parse(datas[0]);
				onParse(datas);
			}

			protected abstract void onParse(string[] line);

			protected int parseInt(string csvElement) {
				return int.Parse(csvElement);
			}

			protected float parseNumber(string csvElement) {
				return float.Parse(csvElement);
			}

			protected string parseString(string csvElement) {
				return csvElement;
			}

			protected bool parseBool(string csvElement) {
				return bool.Parse(csvElement);
			}

			protected IntArray parseIntArray(string csvElement) {
				return new IntArray(csvElement);
			}

			protected NumberArray parseNumberArray(string csvElement) {
				return new NumberArray(csvElement);
			}

			protected StringArray parseStringArray(string csvElement) {
				return new StringArray(csvElement);
			}

			protected IntTable parseIntTable(string csvElement) {
				return new IntTable(csvElement);
			}

			protected NumberTable parseNumberTable(string csvElement) {
				return new NumberTable(csvElement);
			}
		}
	}
}