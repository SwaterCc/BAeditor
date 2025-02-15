#region

using System.Collections.Generic;

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        /// <summary>
        /// 重复执行节点，默认会记录循环次数
        /// 如果是遍历列表则会额外记录遍历对象
        /// </summary>
        private class ARepeatNode : ANode<RepeatNodeData>, IGPoolObject
        {
            public const string LoopCountKey = "__LoopCount__";
            public const string LoopValueKey = "__LoopValue__";

            public override void DoJob()
            {
                switch (Data.operationType)
                {
                    case ERepeatNodeOperationType.Repeat:
                        var repeatCount = ParseInt(Data.repeatCount);
                        for (int i = 0; i < repeatCount; i++)
                        {
                            AContext.VariableBoard.Set(LoopCountKey, i);
                            AContext.VariableBoard.Set(LoopValueKey, i);
                            DoChildrenJob();
                        }

                        AContext.VariableBoard.Delete(LoopValueKey);
                        AContext.VariableBoard.Delete(LoopCountKey);
                        break;
                    case ERepeatNodeOperationType.ETraverseList:
                        if (Data.traverseList.GetParamType() == typeof(List<int>))
                        {
                            var intList = ParseRef<List<int>>(Data.traverseList);
                            for (int i = 0; i < intList.Count; i++)
                            {
                                AContext.VariableBoard.Set(LoopCountKey, i);
                                AContext.VariableBoard.Set(LoopValueKey, intList[i]);
                                DoChildrenJob();
                            }

                            AContext.VariableBoard.Set(LoopCountKey, 0);
                            AContext.VariableBoard.Delete(LoopCountKey);
                        }

                        if (Data.traverseList.GetParamType() == typeof(List<float>))
                        {
                            var floatList = ParseRef<List<float>>(Data.traverseList);
                            for (int i = 0; i < floatList.Count; i++)
                            {
                                AContext.VariableBoard.Set(LoopCountKey, i);
                                AContext.VariableBoard.Set(LoopValueKey, floatList[i]);
                                DoChildrenJob();
                            }

                            AContext.VariableBoard.Set(LoopCountKey, 0);
                            AContext.VariableBoard.Delete(LoopCountKey);
                        }

                        break;
                }
            }

            protected override void OnChildrenJobFinish()
            {
                resetChildren();
            }

            public override void Recycle()
            {
                GPool<ARepeatNode>.Pool.Recycle(this);
            }
        }
    }
}