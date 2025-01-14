using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using Sirenix.Utilities.Editor;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class TimerNodeDataWindow : ANodeSettingWindow<TimerNodeData>
    {
        private AParamsField _first;
        private AParamsField _interval;
        private AParamsField _maxCount;

        protected override void Init()
        {
            _first = new AParamsField(TreeItem,    TempData.firstInterval, "第一次触发间隔",      typeof(RefFloat));
            _interval = new AParamsField(TreeItem, TempData.interval,      "触发间隔",         typeof(RefFloat));
            _maxCount = new AParamsField(TreeItem, TempData.maxCount,      "触发次数(-1为无限次)", typeof(RefInt));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("配置计时器");
            _first.Draw();
            _interval.Draw();
            _maxCount.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}