using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        /// <summary>
        /// Group相当于Ability中的轻量级的小状态，Group在Ability中是以状态机的小状态来运行的，无自动流转状态机，除非手动勾选了AutoNext或者使用切换节点否则不会停止
        /// Group是在Ability运行后的下一帧执行的
        /// </summary>
        private class AGroupNode : ANode<GroupNodeData>, ICPoolObject, ITickANode
        {
            /// <summary>
            /// 隶属Group的Timer节点
            /// </summary>
            private readonly List<ITickANode> _groupTicks = new(10);

            private readonly List<ITickANode> _tickRemoveList = new(5);

            public override void DoJob() { }

            public override void Recycle()
            {
                GPool<AGroupNode>.Pool.Recycle(this);
            }

            public void GroupEnter()
            {
                ACycles.NextGroupId = Data.defaultNextGroupId;
                DoChildrenJob();
            }

            public void AddTick(ITickANode node)
            {
                _groupTicks.Add(node);
            }

            public void RemoveTick(ITickANode node)
            {
                _tickRemoveList.Add(node);
            }

            public void Tick(float dt)
            {
                foreach (var tickANode in _groupTicks)
                {
                    tickANode.Tick(dt);
                }

                foreach (ITickANode removeTick in _tickRemoveList)
                {
                    _groupTicks.RemoveSwapBack(removeTick);
                }

                _tickRemoveList.Clear();

                if (_groupTicks.Count != 0)
                {
                    return;
                }

                //当计时器全部执行完毕后，如果勾选了自动退出，尝试退出Group
                if (Data.autoNext)
                {
                    ACycles.SwitchGroup();
                }
            }

            public void GroupExit()
            {
                Reset();
            }

            protected override void onReset()
            {
                _groupTicks.Clear();
                _tickRemoveList.Clear();
            }
        }
    }
}