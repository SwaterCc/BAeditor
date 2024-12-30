using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Tools;

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
        /// 源初树
        /// </summary>
        public AbilityData SerializableTree { get; }

        /// <summary>
        /// id生成器，每个树独有
        /// </summary>
        public IdGenerator IdGen { get; }

        /// <summary>
        /// 树的id
        /// </summary>
        public int TreeId { get; }

        public AEditorTreeHeadNode(AbilityData data, EAbilityCycle cycle) : base(data.HeadNodeDict[cycle])
        {
            IdGen = new IdGenerator();
            SerializableTree = data;
            Id = IdGen.Get();
            TreeId = SerializableTree.id;
            Root = this;
        }

        public void Build()
        {
            OnBuild(SerializableTree);
        }
        
        /// <summary>
        /// 重新构建树
        /// </summary>
        public void SaveTree() { }
    }
}