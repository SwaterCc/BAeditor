using System.Collections.Generic;
using Editor.AbilityEditor;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class BuffRootMenu : ARootMenuItem
    {
        public BuffRootMenu(OdinMenuTree tree, string itemName) : base(tree, itemName,
            BattleEditorPath.BuffRootPath) { }

        
        public override AView GetViewDrawer()
        {
            return new BuffView();
        }
    }
}