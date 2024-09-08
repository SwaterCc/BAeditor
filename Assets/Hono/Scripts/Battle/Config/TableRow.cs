using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public class IntArray
    {
        private readonly List<int> _array;

        public IntArray(string csv)
        {
            _array = new List<int>();
            var intStr = csv.Split("=");
            foreach (var intValue in intStr)
            {
                _array.Add(int.Parse(intValue));
            }
        }

        public int this[int idx] => _array[idx];
    }

    public class NumberArray
    {
        private readonly List<float> _array;

        public NumberArray(string csv)
        {
            _array = new List<float>();
            var intStr = csv.Split("=");
            foreach (var floatValue in intStr)
            {
                _array.Add(float.Parse(floatValue));
            }
        }

        public float this[int idx] => _array[idx];
    }

    public class StringArray
    {
        private readonly List<string> _array;

        public StringArray(string csv)
        {
            _array = new List<string>();
            var intStr = csv.Split("=");
            foreach (var value in intStr)
            {
                _array.Add(value);
            }
        }

        public string this[int idx] => _array[idx];
    }

    public class IntTable
    {
        private List<IntArray> _tables;

        public IntTable(string csv)
        {
            _tables = new List<IntArray>();
            foreach (var arrayStr in csv.Split("|"))
            {
                _tables.Add(new IntArray(arrayStr));
            }
        }

        public IntArray this[int idx] => _tables[idx];
    }

    public class NumberTable
    {
        private List<NumberArray> _tables;

        public NumberTable(string csv)
        {
            _tables = new List<NumberArray>();
            foreach (var arrayStr in csv.Split("|"))
            {
                _tables.Add(new NumberArray(arrayStr));
            }
        }

        public NumberArray this[int idx] => _tables[idx];
    }

    public abstract class TableRow
    {
        public int Id => _id;
        private int _id;

        public CSVParser Parser { get; protected set; }

        public abstract class CSVParser
        {
            protected TableRow _row;

            protected CSVParser(TableRow row)
            {
                _row = row;
            }

            public void Parse(string line)
            {
                var datas = line.Split(",");
                _row._id = int.Parse(datas[0]);
                onParse(datas);
            }

            protected abstract void onParse(string[] line);

            protected int parseInt(string csvElement)
            {
                return int.Parse(csvElement);
            }

            protected float parseNumber(string csvElement)
            {
                return float.Parse(csvElement);
            }

            protected string parseString(string csvElement)
            {
                return csvElement;
            }

            protected bool parseBool(string csvElement)
            {
                return bool.Parse(csvElement);
            }

            protected IntArray parseIntArray(string csvElement)
            {
                return new IntArray(csvElement);
            }

            protected NumberArray parseNumberArray(string csvElement)
            {
                return new NumberArray(csvElement);
            }

            protected StringArray parseStringArray(string csvElement)
            {
                return new StringArray(csvElement);
            }

            protected IntTable parseIntTable(string csvElement)
            {
                return new IntTable(csvElement);
            }

            protected NumberTable parseNumberTable(string csvElement)
            {
                return new NumberTable(csvElement);
            }
        }
    }
}