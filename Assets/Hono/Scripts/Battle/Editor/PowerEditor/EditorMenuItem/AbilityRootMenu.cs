using System.Collections.Generic;
using Editor.AbilityEditor;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class AbilityRootMenu : ARootMenuItemBase
    { 
        public AbilityRootMenu(OdinMenuTree tree) : base(tree, "其他", BattleEditorPath.PowerEditorRootPath + "/Other") { }
    }
}