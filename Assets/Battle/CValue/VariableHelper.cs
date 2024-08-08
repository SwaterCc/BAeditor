using System;

namespace Battle
{
    public static class CollectionHelper
    {
        public static void InsertVariable(this VariableCollection collection, EVariableType type, string name,
            object value)
        {
            CValue variable = null;
            switch (type)
            {
                case EVariableType.Int:
                    variable = new Variable<int>(name, (int)value);
                    break;
                case EVariableType.Long:
                    variable = new Variable<long>(name, (long)value);
                    break;
                case EVariableType.Bool:
                    variable = new Variable<bool>(name, (bool)value);
                    break;
                case EVariableType.Float:
                    variable = new Variable<float>(name, (float)value);
                    break;
                case EVariableType.String:
                    variable = new Variable<string>(name, (string)value);
                    break;
            }

            collection.Add(name,variable);
        }
        
    }
}