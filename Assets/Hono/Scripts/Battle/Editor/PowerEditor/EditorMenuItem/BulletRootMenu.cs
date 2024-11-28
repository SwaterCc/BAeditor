using System.Collections.Generic;
using Editor.AbilityEditor;
using Editor.BattleEditor.AbilityEditor;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class BulletRootMenu : ARootMenuItemBase
    {
        public BulletRootMenu(OdinMenuTree tree, string itemName) : base(tree, itemName,
            BattleEditorPath.PowerEditorRootPath + "/Bullet") { }
    }
}