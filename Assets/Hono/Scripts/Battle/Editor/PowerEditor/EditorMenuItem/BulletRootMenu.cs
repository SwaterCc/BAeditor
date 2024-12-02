using System.Collections.Generic;
using Editor.AbilityEditor;
using Editor.BattleEditor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class BulletMenuRoot : PMenuRootItem
    {
        public BulletMenuRoot(OdinMenuTree tree, string itemName) : base(tree, itemName,
                                                                         BattleEditorPath.BulletRootPath)
        {
            SdfIcon = SdfIconType.Tornado;
        }

        public override AView GetViewDrawer()
        {
            return new BulletView();
        }
    }
}