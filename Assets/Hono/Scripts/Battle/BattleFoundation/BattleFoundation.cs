using Cysharp.Threading.Tasks;

namespace Hono.Scripts.Battle
{
    public interface IBattleFoundation
    {
        public UniTask AsyncLoad();
    }

    /// <summary>
    /// 战斗基础类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BattleFoundation<T> : IBattleFoundation where T : class, new()
    {
        // 用于锁定对象，确保线程安全
        private static readonly object _lock = new object();

        // 泛型类型的单例实例
        private static T _instance;

        // 私有构造函数，防止外部实例化
        protected BattleFoundation() { }

        // 获取单例实例的方法
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }
        
        public abstract UniTask AsyncLoad();
    }
}