#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        ///     ActionNode 执行动作
        /// </summary>
        private class AActionNode : ANode<ActionNodeData>, IAPoolObject
        {
            public object FuncResult { get; private set; }

            public override void DoJob()
            {
                if (Data.Function.paramType == EParamType.Function)
                {
                    FuncResult = Data.Function.Parse(AContext);
                    if (FuncResult == null)
                    {
                        Debug.LogError("函数执行失败！");
                    }
                }

                DoChildrenJob();
            }

            protected override void onReset()
            {
                FuncResult = null;
            }

            public override void Recycle()
            {
                AObjectPool<AActionNode>.Pool.Recycle(this);
            }

            public new void OnRecycle()
            {
                FuncResult = null;
                ((ANode)this).OnRecycle();
            }
        }
    }
}