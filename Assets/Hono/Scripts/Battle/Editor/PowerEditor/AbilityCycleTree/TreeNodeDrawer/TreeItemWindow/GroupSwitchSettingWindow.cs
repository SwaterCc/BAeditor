using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Editor.AbilityEditor;
using Sirenix.Utilities.Editor;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class GroupSwitchSettingWindow : ANodeSettingWindow<GroupSwitchNodeData>
    {
        private AParamsField _nextGroupId;
        protected override void Init()
        {
            _nextGroupId = new AParamsField<RefInt>(TempData.nextGroupId, "Next Group Id");
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置Group切换：");
            TempData.switchGroupNow = PowerEditorUIHelper.BoolDropField("是否立刻切换Group：", TempData.switchGroupNow);
            _nextGroupId.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}