using System;

namespace Hono.Scripts.Battle.Core
{
    public partial class World
    {
        public class WorldSingleton<T> where T : class, IWorldSystem
        {
            // 泛型类型的单例实例
            private static T _instance;

            // 私有构造函数，防止外部实例化
            protected WorldSingleton() { }

            // 获取单例实例的方法
            public static T Instance
            {
                get
                {
                    if (_instance != null) 
                        return _instance;
                
                    if (BattleManager.World == null)
                    {
                        throw new NullReferenceException("当前无World运行中");
                    }

                    _instance ??= (T)BattleManager.World._systems[typeof(T)];

                    return _instance;
                }
            }
        }
    }
}