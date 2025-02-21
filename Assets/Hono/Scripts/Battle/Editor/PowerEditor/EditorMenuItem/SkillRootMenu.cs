using Editor.AbilityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class SkillMenuRoot : PMenuRootItem
    {
        public SkillMenuRoot(OdinMenuTree tree, string itemName) : base(tree, itemName,
                                                                        BattleEditorPath.SkillRootPath)
        {
            SdfIcon = SdfIconType.LightningChargeFill;
        }

        public override AView GetViewDrawer()
        {
            return new SkillEditorView();
        }
    }
}