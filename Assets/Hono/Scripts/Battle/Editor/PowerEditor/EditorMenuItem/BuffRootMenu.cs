using System.Collections.Generic;
using Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class BuffMenuRoot : PMenuRootItem
    {
        public BuffMenuRoot(OdinMenuTree tree, string itemName) : base(tree, itemName,
                                                                       BattleEditorPath.BuffRootPath)
        {
            SdfIcon = SdfIconType.Snapchat;
        }

        
        public override AView GetViewDrawer()
        {
            return new BuffView();
        }
    }
}