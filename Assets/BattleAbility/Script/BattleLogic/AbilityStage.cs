using System;
using System.Collections.Generic;
using Battle;
using Battle.Def;
using BattleAbility.Editor;

namespace BattleAbility
{
    /// <summary>
    /// 逻辑阶段
    /// 以技能举例，一个技能可能有多个动作，每个动作就是一个阶段
    /// </summary>
    public partial class BattleAbilityBlock
    {
        public class AbilityStage
        {
            public int StageId;

            private BattleAbilityBlock _block;

            /// <summary>
            /// 事件树的root节点列表
            /// </summary>
            private List<TreeNodeBase> _rootList = new();

            private BattleAbilityLogicStage _stageData;

            public AbilityStage(BattleAbilityBlock block, BattleAbilityLogicStage stageData)
            {
                _block = block;
                _stageData = stageData;

                foreach (var treeData in stageData.SerializableTrees)
                {
                    var root = new EventTreeNode(_block, treeData.allNodes[treeData.rootKey]);
                    _rootList.Add(root);
                    buildTree(root, treeData);
                }
            }

            /// <summary>
            /// 构建当树，并返回root节点
            /// </summary>
            private void buildTree(TreeNodeBase parent, BattleAbilitySerializableTree treeData)
            {
                var childKeys = parent.TreeNode.childKeys;
                foreach (var key in childKeys)
                {
                    var childNodeData = treeData.allNodes[key];
                    TreeNodeBase child = null;
                    switch (childNodeData.eNodeType)
                    {
                        case ENodeType.Event:
                            child = new EventTreeNode(_block, childNodeData);
                            break;
                        case ENodeType.Condition:
                            child = new ConditionTreeNode(_block, childNodeData);
                            break;
                        case ENodeType.Action:
                            child = new ActionTreeNode(_block, childNodeData);
                            break;
                        case ENodeType.Variable:
                            child = new VariableTreeNode(_block, childNodeData);
                            break;
                        case ENodeType.Foreach:
                            child = new ForeachTreeNode(_block, childNodeData);
                            break;
                    }
                    parent.AddChild(child);
                    buildTree(child, treeData);
                }
            }

            //生命周期

            /// <summary>
            /// 阶段开始
            /// </summary>
            public void OnStart()
            {
                //注册事件
                foreach (var node in _rootList)
                {
                    ((EventTreeNode)node).Register();
                }

                //计数器更新
                ++(_block._state.StageRunningCount);
                _block._state.AddSpecialStageCount(StageId);

                //初始化完毕后设置状态为Running
                _block._state.State = EBattleAbilityState.StageRunning;

                _block._state.OnStartFinish?.Invoke();
            }

            /// <summary>
            /// 阶段更新
            /// </summary>
            public void OnTick()
            {
                _block._state.OnTickFinish?.Invoke();
            }

            /// <summary>
            /// 阶段结束
            /// </summary>
            public void OnEnd()
            {
                _block._state.State = EBattleAbilityState.StageEnd;

                _block._state.OnEndFinish?.Invoke();
            }
        }
    }
}