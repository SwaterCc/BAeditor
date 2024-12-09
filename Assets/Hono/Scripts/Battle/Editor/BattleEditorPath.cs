namespace Hono.Scripts.Battle
{
    public static class BattleEditorPath
    {
        public static readonly string DataRootPath = "Assets/BattleData";
        
        public static readonly string PowerEditorRootPath = $"{DataRootPath}/PowerEditor";
        public static readonly string PowerGameData = $"{PowerEditorRootPath}/GameData";
        public static readonly string PowerEditorCache = $"{PowerEditorRootPath}/EditorCache";
        
        public static readonly string AbilityRootPath = $"{PowerGameData}/Ability";
        public static readonly string SkillRootPath = $"{PowerGameData}/Skill";
        public static readonly string BuffRootPath = $"{PowerGameData}/Buff";
        public static readonly string BulletRootPath = $"{PowerGameData}/Bullet";
        
        public static readonly string AbilityCacheRootPath = $"{PowerEditorCache}/Ability";
        public static readonly string SkillCacheRootPath = $"{PowerEditorCache}/Skill";
        public static readonly string BuffCacheRootPath = $"{PowerEditorCache}/Buff";
        public static readonly string BulletCacheRootPath = $"{PowerEditorCache}/Bullet";

        public static readonly string TagEditorInfoPath = $"{DataRootPath}/TagEditorInfo.asset";
        public static readonly string TagTreePath = $"{DataRootPath}/TagTree.asset";
    }
}