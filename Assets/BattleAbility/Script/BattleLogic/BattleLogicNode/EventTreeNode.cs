using System;
using BattleAbility.Editor;

namespace BattleAbility
{
    public class BattleEventHandle
    {
        protected readonly Action _onFire;

        public BattleEventHandle(Action onFire)
        {
            _onFire = onFire;
        }

        public void TryFire()
        {
            _onFire?.Invoke();
        }
    }

    public class BattleEventHandle<TKey> : BattleEventHandle
    {
        private TKey _key;

        public BattleEventHandle(Action onFire, TKey key) : base(onFire)
        {
            _key = key;
        }

        public void TryFire(TKey key)
        {
            if (_key.Equals(key) && _onFire != null)
            {
                _onFire.Invoke();
            }
        }
    }


    public class EventTreeNode : TreeNodeBase
    {
        private new readonly EventTreeNodeData _nodeData;
        private BattleEventHandle _handle;

        public EventTreeNode(BattleAbilityBlock block, BattleAbilitySerializableTree.TreeNode treeNode) : base(block,
            treeNode)
        {
            _nodeData = base._nodeData as EventTreeNodeData;
        }

        public void Register()
        {
            //注册事件
            BattleEventMgr.Instance.Register(_nodeData.EventType, _handle);
            if (_nodeData.EventType is EBattleEventType.Hit or EBattleEventType.SpecialMotionBegin or EBattleEventType.SpecialMotionEnd)
            {
                _handle = new BattleEventHandle<int>(onEventFired, (int)_nodeData.Params[0].Value);
            }
            else
            {
                _handle = new BattleEventHandle<int>(onEventFired, _block.UId);
            }
        }

        private new void RunLogic() {}
        private new TreeNodeBase GetNext() => null;
        

        /// <summary>
        /// 事件触发
        /// </summary>
        private void onEventFired()
        {
            //从第一个子节点开始
            if (_children.Count == 0)
                return;
            var nextChild = _children[0];
            while (nextChild != null)
            {
                nextChild.RunLogic();
                nextChild = nextChild.GetNext();
            }
        }
    }
}