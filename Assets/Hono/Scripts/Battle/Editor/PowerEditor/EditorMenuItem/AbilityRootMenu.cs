using System.Collections.Generic;
using Editor.AbilityEditor;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class AbilityRoot : PMenuRootItem
    {
        public AbilityRoot(OdinMenuTree tree) : base(tree, "Ability", BattleEditorPath.AbilityRootPath) { }

        public override AView GetViewDrawer()
        {
            return new AbilityView();
        }
    }
}