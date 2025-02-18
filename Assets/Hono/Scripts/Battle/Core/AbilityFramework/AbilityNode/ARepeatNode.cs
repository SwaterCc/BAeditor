#region

using System.Collections.Generic;
using Hono.Scripts.Battle.ObjectPool;

#endregion

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        /// <summary>
        /// 重复执行节点，默认会记录循环次数
        /// 如果是遍历列表则会额外记录遍历对象
        /// </summary>
        private class ARepeatNode : ANode<RepeatNodeData>, IGPoolObject, ILocalVariableBoardHandle
        {
            public VariableBoard LocalVariableBoard { get; } = new();
            private const string LoopCountKey = "__LoopCount__";
            private const string LoopValueKey = "__LoopValue__";

            public override void DoJob()
            {
                switch (Data.operationType)
                {
                    case ERepeatNodeOperationType.Repeat:
                        var repeatCount = ParseInt(Data.repeatCount);
                        for (int i = 0; i < repeatCount; i++)
                        {
                            LocalVariableBoard.Set(LoopCountKey, i);
                            LocalVariableBoard.Set(LoopValueKey, i);
                            DoChildrenJob();
                        }

                        LocalVariableBoard.Delete(LoopValueKey);
                        LocalVariableBoard.Delete(LoopCountKey);
                        break;
                    case ERepeatNodeOperationType.ETraverseList:
                        if (Data.traverseList.GetParamType() == typeof(List<int>))
                        {
                            var intList = ParseRef<List<int>>(Data.traverseList);
                            for (int i = 0; i < intList.Count; i++)
                            {
                                LocalVariableBoard.Set(LoopCountKey, i);
                                LocalVariableBoard.Set(LoopValueKey, intList[i]);
                                DoChildrenJob();
                            }
                            LocalVariableBoard.Delete(LoopValueKey);
                            LocalVariableBoard.Delete(LoopCountKey);
                        }
                        
                        if (Data.traverseList.GetParamType() == typeof(GList<int>))
                        {
                            var intList = ParseRef<GList<int>>(Data.traverseList);
                            for (int i = 0; i < intList.Count; i++)
                            {
                               LocalVariableBoard.Set(LoopCountKey, i);
                               LocalVariableBoard.Set(LoopValueKey, intList[i]);
                                DoChildrenJob();
                            }

                            LocalVariableBoard.Delete(LoopValueKey);
                            LocalVariableBoard.Delete(LoopCountKey);
                        }

                        if (Data.traverseList.GetParamType() == typeof(List<float>))
                        {
                            var floatList = ParseRef<List<float>>(Data.traverseList);
                            for (int i = 0; i < floatList.Count; i++)
                            {
                                LocalVariableBoard.Set(LoopCountKey, i);
                                LocalVariableBoard.Set(LoopValueKey, floatList[i]);
                                DoChildrenJob();
                            }

                            LocalVariableBoard.Delete(LoopValueKey);
                            LocalVariableBoard.Delete(LoopCountKey);
                        }
                        
                        if (Data.traverseList.GetParamType() == typeof(GList<float>))
                        {
                            var floatList = ParseRef<GList<float>>(Data.traverseList);
                            for (int i = 0; i < floatList.Count; i++)
                            {
                                LocalVariableBoard.Set(LoopCountKey, i);
                                LocalVariableBoard.Set(LoopValueKey, floatList[i]);
                                DoChildrenJob();
                            }

                            LocalVariableBoard.Delete(LoopValueKey);
                            LocalVariableBoard.Delete(LoopCountKey);
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