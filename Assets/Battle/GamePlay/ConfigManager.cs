using System;
using System.Collections.Generic;
using Battle.Tools;

namespace Battle.GamePlay
{
    public interface IConfig
    {
        
    }

    public interface  IConfigTable
    {
        //public object GetRow(int rowId);
    }

    public class ConfigTable<T> : IConfigTable
    {
        public T GetRow(int rowId)
        {
            return default;
        }
    }
    
    
    /// <summary>
    /// 配置管理
    /// </summary>
    public class ConfigManager : Singleton<ConfigManager>
    {
        private Dictionary<Type, IConfigTable> _tables = new Dictionary<Type, IConfigTable>();
        public void Init()
        {
            //加载路径中的所有xml
            
        }

        public T GetTable<T>()
        {
            if (_tables.TryGetValue(typeof(T), out var table))
            {
                return (T)table;
            }
            else
            {
                //抱错
                return default;
            }
        }
    }
}