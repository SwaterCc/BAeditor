using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.Utilities.Editor;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class BranchSettingWindow : ANodeSettingWindow<BranchNodeData>
    {
        private AParamsField _compareFunc;

        protected override void Init()
        {
            _compareFunc = new AParamsField<RefBoolean>(TempData.CompareFunc, "判定条件");
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("If节点", true);
            _compareFunc.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}