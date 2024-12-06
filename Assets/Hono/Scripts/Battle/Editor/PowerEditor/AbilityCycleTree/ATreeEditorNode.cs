using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// Ability在Editor中直接使用很难用，先做一层解析，将其解析为更好用的数据结构
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

        private readonly AbilityNodeData _data;
        private ATreeEditorNode _parent;
        private readonly List<ATreeEditorNode> _children = new();

        public ATreeEditorNode(AbilityNodeData nodeData)
        {
            _data = nodeData.Copy();
        }

        public ATreeEditorNode GetParent() => _parent;
        public AbilityNodeData GetDataRef() => _data;
        public T GetDataRef<T>() where T : AbilityNodeData => (T)GetDataRef();

        /// <summary>
        /// 获取Data拷贝
        /// </summary>
        /// <returns></returns>
        public AbilityNodeData GetData()
        {
            return _data.Copy();
        }

        /// <summary>
        /// 获取Data拷贝并做类型转换
        /// </summary>
        /// <returns></returns>
        public T GetData<T>() where T : AbilityNodeData
        {
            if (_data is T)
            {
                return (T)GetData();
            }

            throw new InvalidCastException("类型不符合");
        }

        /// <summary>
        /// 构建树，头节点调用
        /// </summary>
        /// <param name="abilityData"></param>
        /// <param name="idGenerator"></param>
        public void Build(AbilityData abilityData, IdGenerator idGenerator)
        {
            Id = idGenerator.Get();
            foreach (var id in _data.ChildrenIds)
            {
                var child = new ATreeEditorNode(abilityData.NodeDict[id]);
                AddChild(child);
                child.Build(abilityData, idGenerator);
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="editorNode"></param>
        public void AddChild(ATreeEditorNode editorNode)
        {
            editorNode._parent = this;
            _children.Add(editorNode);
        }

        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="editorNode"></param>
        public void RemoveChild(ATreeEditorNode editorNode)
        {
            _children.Remove(editorNode);
        }

        /// <summary>
        /// 数据序列化
        /// </summary>
        public void Serialize() { }
    }
}