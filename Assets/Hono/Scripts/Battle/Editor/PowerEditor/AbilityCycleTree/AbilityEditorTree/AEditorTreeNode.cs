using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// Ability在Editor中直接使用很难用，先做一层解析，将其解析为更好用的数据结构
    /// 同时也是作为数据中间层,修改缓存层，他不会立刻序列化，也不会立刻被清除
    /// </summary>
    public class AEditorTreeNode
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// 节点序列化数据
        /// </summary>
        public AbilityNodeData Data { get; private set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public AEditorTreeNode Parent { get; protected set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<AEditorTreeNode> Children { get; protected set; } = new();

        /// <summary>
        /// 根节点
        /// </summary>
        public AEditorTreeHeadNode Root { get; protected set; }

        /// <summary>
        /// 子节点数量
        /// </summary>
        public int ChildrenCount => Children.Count;

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="nodeData"></param>
        public AEditorTreeNode(AbilityNodeData nodeData)
        {
            Data = nodeData.DeepCopy();
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="node"></param>
        public AEditorTreeNode(AEditorTreeNode node)
        {
            Data = node.DeepCopy();
            foreach (var editorNodeChild in node.Children)
            {
                var copyChild = new AEditorTreeNode(editorNodeChild);
                copyChild.Parent = this;
                Children.Add(copyChild);
            }
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public AbilityNodeData DeepCopy()
        {
            return Data.DeepCopy();
        }

        /// <summary>
        /// 深拷贝并做类型转换
        /// </summary>
        /// <returns></returns>
        public T DeepCopy<T>() where T : AbilityNodeData
        {
            if (Data is T)
            {
                return (T)Data.DeepCopy();
            }

            throw new InvalidCastException("类型不符合");
        }

        /// <summary>
        /// 构建树，头节点调用
        /// </summary>
        /// <param name="abilityData"></param>
        protected void OnBuild(AbilityData abilityData)
        {
            foreach (var id in Data.ChildrenIds)
            {
                var child = new AEditorTreeNode(abilityData.NodeDict[id]);
                AddChild(child);
                child.OnBuild(abilityData);
            }
        }

        /// <summary>
        /// 添加复数子节点
        /// </summary>
        /// <param name="children"></param>
        public void AddChildren(List<AEditorTreeNode> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(AEditorTreeNode node)
        {
            node.Parent = this;
            node.OnRootChange(Root);
            Children.Add(node);
        }

        /// <summary>
        /// 插入子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="idx"></param>
        public void InsertChild(AEditorTreeNode node, int idx)
        {
            node.Parent = this;
            node.OnRootChange(Root);
            Children.Insert(idx, node);
        }

        /// <summary>
        /// 将该节点与指定索引的节点交换位置
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        public void SwapChildIndex(AEditorTreeNode node, int index)
        {
            if (index >= Children.Count)
            {
                return;
            }

            var itemIdx = Children.IndexOf(node);
            if (itemIdx == index)
            {
                return;
            }

            var temp = Children[index];
            Children[index] = node;
            Children[itemIdx] = temp;
        }

        /// <summary>
        /// 更新root
        /// </summary>
        /// <param name="root"></param>
        private void OnRootChange(AEditorTreeHeadNode root)
        {
            if (Root == root)
                return;
            Root = root;
            Id = root.IdGen.Get();
            foreach (var child in Children)
            {
                child.OnRootChange(root);
            }
        }

        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(AEditorTreeNode node)
        {
            Children.Remove(node);
        }
        
        /// <summary>
        /// 从节点的父节点删除自己
        /// </summary>
        public void RemoveSelfFromParent()
        {
            Parent?.RemoveChild(this);
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="data"></param>
        public void SaveNodeDataChange(AbilityNodeData data)
        {
            Data = data;
            //if (autoSave)
            if (true)
            {
                Serialize();
            }
        }

        /// <summary>
        /// 是否已经序列化保存
        /// </summary>
        /// <returns></returns>
        public bool IsDirty()
        {
            return false;
        }

        /// <summary>
        /// 数据序列化
        /// </summary>
        public void Serialize()
        {
            
        }
    }
}