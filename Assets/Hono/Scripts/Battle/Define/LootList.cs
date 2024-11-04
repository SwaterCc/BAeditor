#region

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

#endregion

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class LootSetting
    {
        public ELootFunctionType LootFunctionType;

        [ShowIf("LootFunctionType", ELootFunctionType.SkillLevelUp)]
        public int SkillLevelNum;

        [ShowIf("LootFunctionType", ELootFunctionType.BuffAdd)]
        public int BuffId;

        [ShowIf("LootFunctionType", ELootFunctionType.BuffAdd)]
        public int BuffLayer;

        [ShowIf("LootFunctionType", ELootFunctionType.AttrChange)]
        public ELogicAttr AttrType;

        [ShowIf("LootFunctionType", ELootFunctionType.AttrChange)]
        public int ChangeAttrValue;
    }

    [Serializable]
    public class LootList
    {
        public List<LootSetting> LootSettings = new();
    }
}