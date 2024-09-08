using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public class IntArray
    {
        public IntArray(string csv) { }
    }

    public class NumberArray
    {
        public NumberArray(string csv) { }
    }

    public class StringArray
    {
        public StringArray(string csv) { }
    }

    public class IntTable
    {
        public IntTable(string csv) { }
    }

    public class NumberTable
    {
        public NumberTable(string csv) { }
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