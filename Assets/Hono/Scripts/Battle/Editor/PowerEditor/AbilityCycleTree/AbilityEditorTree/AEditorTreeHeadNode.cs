using System.Collections.Generic;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Tools;
using UnityEditor;

namespace Editor.AbilityEditor
{
    public class AEditorTreeHeadNode : AEditorTreeNode
    {
        public class IdGenerator
        {
            private int _idCount = 1;

            public int Get()
            {
                return ++_idCount;
            }
        }

        /// <summary>
        /// id生成器，每个树独有
        /// </summary>
        public IdGenerator IdGen { get; }
        
        /// <summary>
        /// 所属周期
        /// </summary>
        public EAbilityCycle Cycle { get; }
        
        /// <summary>
        /// 周期节点数据
        /// </summary>
        public CycleNodeData CycleData { get; }
        
        /// <summary>
        /// Ability数据
        /// </summary>
        public AbilityData AbilitySerializableData { get; private set; }

        public AEditorTreeHeadNode(AbilityData data, EAbilityCycle cycle) : base(data.HeadNodeDict[cycle])
        {
            IdGen = new IdGenerator();
            Id = IdGen.Get();
            Root = this;
            Cycle = cycle;
            CycleData = (CycleNodeData)Data;
            AbilitySerializableData = data;
        }

        public void Build()
        {
            OnBuild(CycleData.SerializableNodeList);
        }

        /// <summary>
        /// 序列化树
        /// </summary>
        public void SerializeCycleTree()
        {
            List<AbilityNodeData> datas = new List<AbilityNodeData>();
            OnSerialize(ref datas);
            CycleData.SerializableNodeList = datas;
            AbilitySerializableData.HeadNodeDict[Cycle] = CycleData;
            EditorUtility.SetDirty(AbilitySerializableData);
            AssetDatabase.SaveAssetIfDirty(AbilitySerializableData);
        }
    }
}