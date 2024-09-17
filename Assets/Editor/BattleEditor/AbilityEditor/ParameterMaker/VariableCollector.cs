using System;
using System.Collections.Generic;

namespace Editor.AbilityEditor
{
    public class VariableCollector
    {
        public Dictionary<Type, List<string>> _variables = new();

        public void Add(Type type, string name)
        {
            if (!_variables.TryGetValue(type, out var list))
            {
                list = new List<string>();
                _variables.Add(type,list);
            }

            if (!list.Contains(name))
            {
                list.Add(name);
            }
        }

        public List<string> GetVariables(Type type)
        {
            if (_variables.TryGetValue(type, out var list))
            {
                return list;
            }

            return new List<string>();
        }

        public void Clear()
        {
            _variables.Clear();
        }
    }
}