using System.Collections.Generic;
using Battle.Def;

namespace BattleAbility
{
    /// <summary>
    /// 管理战斗逻辑中事件节点的注册和监听
    /// </summary>
    public class BattleEventMgr
    {
        private static BattleEventMgr _instance;

        public static BattleEventMgr Instance
        {
            get
            {
                //目前简单写，后续要判定线程加锁 @shirui
                return _instance ??= new BattleEventMgr();
            }
        }
        
        /// <summary>
        /// 事件注册列表
        /// </summary>
        private Dictionary<EBattleEventType, List<BattleEventHandle>> _eventDict = new();

        /// <summary>
        /// 
        /// </summary>
        public void Register(EBattleEventType type, BattleEventHandle handle)
        {
            if (!_eventDict.TryGetValue(type,out var handles))
            {
                handles = new List<BattleEventHandle>();
                _eventDict.Add(type,handles);
            }

            if (!handles.Contains(handle))
            {
                handles.Add(handle);
            }
            else
            {
                //发个warning
            }
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        public void Fired()
        {
        }
        
        /// <summary>
        /// 事件触发
        /// </summary>
        public void Fired<TKey>(TKey key)
        {
        }
    }
}