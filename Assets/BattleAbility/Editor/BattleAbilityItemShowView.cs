using Sirenix.OdinInspector;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 能力配置界面 （基础配置 + 逻辑配置）
    /// </summary>
    public class BattleAbilityItemShowView
    {
        public BattleAbilityBaseConfig BaseConfig;

        public BattleAbilityLogicTreeView LogicTreeView;
        
        public BattleAbilityItemShowView(BattleAbilityBaseConfig baseConfig, BattleAbilitySerializableTree serializableTree)
        {
            BaseConfig = baseConfig;
            LogicTreeView = new BattleAbilityLogicTreeView(serializableTree);
        }

        public string GetOdinMenuTreeItemLabel()
        {
            return $"{BaseConfig.ConfigId}->{BaseConfig.Name}";
        }
    }
}