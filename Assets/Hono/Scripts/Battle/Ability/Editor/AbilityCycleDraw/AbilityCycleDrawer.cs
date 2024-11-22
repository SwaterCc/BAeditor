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
        public OnPreAwardCheckDrawer(EAbilityCycle cycle, AbilityData data) : base(cycle,
            data) { }

        protected override bool getDefaultFoldout()
        {
            return Data.Type == EAbilityType.Buff;
        }

        protected override void drawEx() { }
    }

    public class OnPreExecuteCheckDrawer : AbilityCycleDrawBase
    {
        private ResItem _removeItem;
        private List<ResItem> _removeList;


        public OnPreExecuteCheckDrawer(EAbilityCycle cycle, AbilityData data) : base(cycle,
            data) { }

        protected override bool getDefaultFoldout()
        {
            return Data.Type == EAbilityType.Skill;
        }

        protected override void drawEx() { }
    }

    public class OnInitDrawer : AbilityCycleDrawBase
    {
        public OnInitDrawer(EAbilityCycle cycle, AbilityData data) : base(cycle, data) { }

        protected override void drawEx() { }
    }

    public class OnPreExecuteDrawer : AbilityCycleDrawBase
    {
        private EResCostType _resCostType;
        private bool _hasType;

        public OnPreExecuteDrawer(EAbilityCycle cycle, AbilityData data) :
            base(cycle, data) { }

        protected override bool getDefaultFoldout() => false;

        protected override void drawEx()
        {
            if (Data.Type == EAbilityType.Skill && _hasType && _resCostType == EResCostType.BeforeExecute)
            {
                Foldout = true;
            }
        }
    }

    public class OnExecutingDrawer : AbilityCycleDrawBase
    {
        public OnExecutingDrawer(EAbilityCycle cycle, AbilityData data) :
            base(cycle, data) { }

        protected override bool getDefaultFoldout() => true;
    }

    public class OnEndExecuteDrawer : AbilityCycleDrawBase
    {
        private EResCostType _resCostType;
        private bool _hasType;

        public OnEndExecuteDrawer(EAbilityCycle cycle, AbilityData data) :
            base(cycle, data) { }

        protected override bool getDefaultFoldout() => false;

        protected override void drawEx() { }
    }
}