using Hono.Scripts.Battle;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class GroupSettingWindow : ANodeSettingWindow<GroupNodeData>
    {
        protected override void Init() { }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("设置Group");

            TempData.groupId = SirenixEditorFields.IntField("阶段Id", TempData.groupId);
            TempData.autoNext = EditorGUILayout.Toggle("阶段Id", TempData.autoNext);
            if (TempData.autoNext)
            {
                TempData.defaultNextGroupId = SirenixEditorFields.IntField("默认下一阶段Id", TempData.defaultNextGroupId);
            }

            SirenixEditorGUI.EndBox();
        }
    }
}