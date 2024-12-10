using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public static class AbilityDataExtensions
    {
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AbilityNodeData CreateNode(EAbilityNodeType type)
        {
            AbilityNodeData nodeData = null;

            switch (type)
            {
                case EAbilityNodeType.EAbilityCycle:
                    nodeData = new CycleNodeData();
                    break;
                case EAbilityNodeType.EEvent:
                    nodeData = new ListenerNodeData();
                    break;
                case EAbilityNodeType.EBranchControl:
                    nodeData = new BranchNodeData();
                    break;
                case EAbilityNodeType.EVariableSetter:
                    nodeData = new VariableNodeData();
                    break;
                case EAbilityNodeType.EAttrSetter:
                    nodeData = new AttrNodeData();
                    break;
                case EAbilityNodeType.ERepeat:
                    nodeData = new RepeatNodeData();
                    break;
                case EAbilityNodeType.EAction:
                    nodeData = new ActionNodeData();
                    break;
                case EAbilityNodeType.ETimer:
                    nodeData = new TimerNodeData();
                    break;
                case EAbilityNodeType.EGroup:
                    nodeData = new GroupNodeData();
                    break;
                default:
                    throw new InvalidCastException("找不到对应的Node类型");
            }
           
            return nodeData;
        }

        /// <summary>
        /// 创建一个新节点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static T CreateNode<T>(EAbilityNodeType type) where T : AbilityNodeData
        {
            return (T)CreateNode(type);
        }
        
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="abilityData"></param>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        public static void AddNode(this AbilityData abilityData, AbilityNodeData parent, AbilityNodeData node)
        {
            if (node == null)
                throw new NullReferenceException("Node为空");
            if (node.NodeId == 0)
                throw new TypeInitializationException(nameof(node), new Exception("Node初始化未完成！Id不可以为 0"));

            abilityData.NodeDict.Add(node.NodeId, node);

            if (parent == null) return;
            parent.ChildrenIds.Add(node.NodeId);
            node.ParentId = parent.NodeId;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="abilityData"></param>
        /// <param name="node"></param>
        public static void RemoveNode(this AbilityData abilityData, AbilityNodeData node)
        {
            if (!abilityData.NodeDict.Remove(node.NodeId))
            {
                Debug.LogError($"Ability {abilityData.id} 删除Node {node.NodeId} 失败");
                return;
            }

            if (abilityData.NodeDict.TryGetValue(node.ParentId, out var parent))
            {
                parent.ChildrenIds.Remove(node.NodeId);
            }
        }
    }
}