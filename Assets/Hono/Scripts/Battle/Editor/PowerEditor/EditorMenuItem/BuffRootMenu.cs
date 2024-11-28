using System.Collections.Generic;
using Editor.AbilityEditor;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class BuffRootMenu : ARootMenuItemBase
    {
        public BuffRootMenu(OdinMenuTree tree, string itemName) : base(tree, itemName,
            BattleEditorPath.BuffRootPath) { }

        
        protected override AView getViewDrawer()
        {
            return new BuffView();
        }
    }
}