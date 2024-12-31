using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.Utilities.Editor;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class RepeatSettingWindow : ANodeSettingWindow<RepeatNodeData>
    {
        private AParamsField _maxCount;

        protected override void Init()
        {
            //_maxCount = new AParamsField(TreeItem, TempData.MaxRepeatCount, "循环次数", typeof(RefInt));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置循环次数");
            _maxCount.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}