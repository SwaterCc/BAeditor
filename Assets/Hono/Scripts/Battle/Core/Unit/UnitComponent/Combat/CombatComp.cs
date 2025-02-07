using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 战斗组件
    /// </summary>
    public class CombatComp : UnitComponent
    {
        public enum ECombatRTType
        {
            SkillId = 1,

            BuffId = 100000,
        }

        /// <summary>
        /// 战斗数据运行时，类似属性
        /// </summary>
        public abstract class CombatRT { }

        public class SkillRT : CombatRT
        {
            public int SkillLevel;

            public TagCollection TagCollection;
        }

        public class BuffRT : CombatRT
        {
            public int BuffLayer;
        }

        /// <summary>
        /// 战斗组件的快照
        /// </summary>
        public class CombatCompSnapshot
        {
            //技能运行时数据快照
            //Buff运行时数据快照
        }
        public override void Init() { }

        protected override void onClear() { }
        
        public SkillRT GetSkillRt(int skillId)
        {
            return null;
        }

        public BuffRT GetBuffRT(int buffId)
        {
            return null;
        }
    }
}