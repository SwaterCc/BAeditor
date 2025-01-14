#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private abstract class ANode
        {
            /// <summary>
            /// node配置数据
            /// </summary>
            public AbilityNodeData Data { get; private set; }

            /// <summary>
            /// Ability
            /// </summary>
            public Ability AContext { get; private set; }

            /// <summary>
            /// 周期控制
            /// </summary>
            protected AbilityCycle ACycles => AContext?._abilityCycle;

            /// <summary>
            /// 父节点
            /// </summary>
            protected ANode Parent { get; private set; }

            /// <summary>
            /// 子节点
            /// </summary>
            protected readonly List<ANode> Children = new(15);

            #region 节点周期

            public void OnRent(Ability ability, AbilityNodeData data)
            {
                AContext = ability;
                Data = data;
            }

            /// <summary>
            /// 构建树，递归补全子节点
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="nodeDatas"></param>
            public void Build(in ANode parent, in List<AbilityNodeData> nodeDatas)
            {
                //设置父节点
                Parent = parent;
                //填充子节点
                foreach (var nodeIdx in Data.childrenIndexes)
                {
                    var node = ACycles.GetNode(AContext, nodeDatas[nodeIdx]);
                    Children.Add(node);
                    node.Build(this, nodeDatas);
                }
            }

            /// <summary>
            /// 执行节点功能
            /// </summary>
            public abstract void DoJob();

            /// <summary>
            /// 执行子节点功能
            /// </summary>
            protected void DoChildrenJob()
            {
                foreach (var node in Children)
                {
                    //Debug.Log($"[Ability] AbilityId:{_executor.AbilityData.ConfigId} nodeId {NodeId} NodeType {NodeType} DoChildrenJob Next Node is {node.NodeType} : {node.NodeId}");
                    node.DoJob();
                }

                OnChildrenJobFinish();
            }

            /// <summary>
            /// 该节点的子节点全部执行完了
            /// </summary>
            protected virtual void OnChildrenJobFinish() { }

            /// <summary>
            /// 重置该节点，注意，同时该节点会被重置为未执行过
            /// </summary>
            protected void Reset()
            {
                onReset();
                resetChildren();
            }

            protected void resetChildren()
            {
                foreach (var child in Children)
                {
                    child.Reset();
                }
            }

            protected virtual void onReset() { }

            public abstract void Recycle();

            public void OnRecycle()
            {
                Data = null;
                AContext = null;
                Parent = null;
                foreach (var child in Children)
                {
                    child.Recycle();
                }

                Children.Clear();
            }

            #endregion

            #region AParam解析

            /// <summary>
            /// 解析获得Int
            /// </summary>
            /// <param name="aParams"></param>
            /// <returns></returns>
            public int ParseInt(AParams aParams)
            {
                try
                {
                    return IntParser.Parse(AContext, this, aParams);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return 0;
                }
            }

            /// <summary>
            /// 解析获取float
            /// </summary>
            /// <param name="aParams"></param>
            /// <returns></returns>
            public float ParseFloat(AParams aParams)
            {
                try
                {
                    return FloatParser.Parse(AContext, this, aParams);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return 0;
                }
            }

            /// <summary>
            /// 解析获取bool
            /// </summary>
            /// <param name="aParams"></param>
            /// <returns></returns>
            public bool ParseBoolean(AParams aParams)
            {
                try
                {
                    return BooleanParser.Parse(AContext, this, aParams);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }

            /// <summary>
            /// 解析获取Vector3
            /// </summary>
            /// <param name="aParams"></param>
            /// <returns></returns>
            public Vector3 ParseVector3(AParams aParams)
            {
                try
                {
                    return Vector3Parser.Parse(AContext, this, aParams);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return Vector3.zero;
                }
            }

            /// <summary>
            /// 解析引用对象
            /// </summary>
            /// <param name="aParams"></param>
            /// <returns></returns>
            public object ParseRef(AParams aParams)
            {
                try
                {
                    return RefParser.Parse(AContext, this, aParams);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return null;
                }
            }

            /// <summary>
            /// 解析对象
            /// </summary>
            /// <param name="aParams"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public T ParseRef<T>(AParams aParams) where T : class
            {
                try
                {
                    return (T)RefParser.Parse(AContext, this, aParams);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return null;
                }
            }

            #endregion

            #region 工具函数
            /// <summary>
            /// 获取指定类型的父节点
            /// </summary>
            /// <param name="result"></param>
            /// <typeparam name="TNodeType"></typeparam>
            /// <returns></returns>
            public bool TryGetParent<TNodeType>(out TNodeType result) where TNodeType : ANode
            {
                result = null;
                var parent = Parent;
                while (parent != null)
                {
                    if (parent is TNodeType tNode)
                    {
                        result = tNode;
                        return true;
                    }

                    parent = parent.Parent;
                }

                return false;
            }

            #endregion
        }

        private abstract class ANode<TNodeData> : ANode where TNodeData : AbilityNodeData
        {
            public new readonly TNodeData Data;

            protected ANode()
            {
                Data = (TNodeData)base.Data;
            }
        }
    }
}