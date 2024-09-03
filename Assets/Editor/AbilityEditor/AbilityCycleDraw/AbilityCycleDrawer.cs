using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;


namespace Editor.AbilityEditor
{
    public class OnPreAwardCheckDrawer : AbilityCycleDrawBase
    {
        public OnPreAwardCheckDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout()
        {
            return Data.Type == EAbilityType.Buff;
        }

        protected override void drawEx()
        {
            
        }
    }

    public class OnPreExecuteCheckDrawer : AbilityCycleDrawBase
    {
        private ResItem _removeItem;
        private List<ResItem> _removeList;


        public OnPreExecuteCheckDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout()
        {
            return Data.Type == EAbilityType.Skill;
        }

        protected override void drawEx()
        {
            if (Data.Type == EAbilityType.Skill)
            {
                //DrawResList(Data, "SkillResCheck", "设置技能释放检测");
            }
        }
    }

    public class OnInitDrawer : AbilityCycleDrawBase
    {
        public OnInitDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override void drawEx() { }
    }

    public class OnPreExecuteDrawer : AbilityCycleDrawBase
    {
        private EResCostType _resCostType;
        private bool _hasType;

        public OnPreExecuteDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout() => false;

        protected override void drawEx()
        {
           
            if (Data.Type == EAbilityType.Skill && _hasType && _resCostType == EResCostType.BeforeExecute)
            {
                Foldout = true;
                //DrawResList(Data, "SkillResCost", "设置技能消耗：");
            }
        }
    }

    public class OnExecutingDrawer : AbilityCycleDrawBase
    {
        public OnExecutingDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }
        protected override bool getDefaultFoldout() => true;
    }

    public class OnEndExecuteDrawer : AbilityCycleDrawBase
    {
        private EResCostType _resCostType;
        private bool _hasType;

        public OnEndExecuteDrawer(EAbilityCycleType cycleType, AbilityData data) : base(cycleType, data) { }

        protected override bool getDefaultFoldout() => false;

        protected override void drawEx()
        {
           
            if (Data.Type == EAbilityType.Skill && _hasType && _resCostType == EResCostType.AfterExecute)
            {
                Foldout = true;
                //DrawResList(Data, "SkillResCost", "设置技能消耗：");
            }
        }
    }
}