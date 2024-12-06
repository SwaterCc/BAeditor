using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// Ability在Editor中直接使用很难用，先做一层解析，将其解析为更好用的数据结构
    /// 同时也是作为数据中间层,修改缓存层，他不会立刻序列化，也不会立刻被清除
    /// </summary>
    public class ATreeEditorNode
    {
        public class IdGenerator
        {
            private int _idCount = 0;

            public int Get()
            {
                return ++_idCount;
            }
        }

        public int Id { get; private set; }

        public AbilityNodeData Data { get; private set; }
        public ATreeEditorNode Parent;
        public readonly List<ATreeEditorNode> Children = new();

        private AbilityData _abilityOriginData;
        
        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="nodeData"></param>
        public ATreeEditorNode(AbilityNodeData nodeData)
        {
            Data = nodeData.DeepCopy();
        }

        /// <summary>
        /// 拷贝构造函数
        /// </summary>
        /// <param name="editorNode"></param>
        public ATreeEditorNode(ATreeEditorNode editorNode)
        {
            Data = editorNode.DeepCopy();
            foreach (var editorNodeChild in editorNode.Children)
            {
                var copyChild = new ATreeEditorNode(editorNodeChild);
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
        /// 构建树，头节点调用
        /// </summary>
        /// <param name="abilityData"></param>
        /// <param name="idGenerator"></param>
        public void Build(AbilityData abilityData, IdGenerator idGenerator)
        {
            Id = idGenerator.Get();
            foreach (var id in Data.ChildrenIds)
            {
                var child = new ATreeEditorNode(abilityData.NodeDict[id]);
                AddChild(child);
                child.Build(abilityData, idGenerator);
            }
        }

        /// <summary>
        /// 添加复数子节点
        /// </summary>
        /// <param name="children"></param>
        public void AddChildren(List<ATreeEditorNode> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }
        
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="editorNode"></param>
        public void AddChild(ATreeEditorNode editorNode)
        {
            editorNode.Parent = this;
            Children.Add(editorNode);
        }

        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="editorNode"></param>
        public void RemoveChild(ATreeEditorNode editorNode)
        {
            Children.Remove(editorNode);
        }

        /// <summary>
        /// 从节点的父节点删除自己
        /// </summary>
        public void RemoveSelfFromParent()
        {
            Parent?.RemoveChild(this);
        }

        /// <summary>
        /// 数据序列化
        /// </summary>
        public void Serialize() { }
    }
}