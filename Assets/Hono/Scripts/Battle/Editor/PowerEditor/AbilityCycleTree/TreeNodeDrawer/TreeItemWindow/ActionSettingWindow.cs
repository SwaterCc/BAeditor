using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Editor.AbilityEditor.TreeItemWindow
{
    public class ActionSettingWindow : ANodeSettingWindow<ActionNodeData>
    {
        protected override void Init() { }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("Action节点配置");

            var buttonText = string.IsNullOrEmpty(TempData.action.funcName) ? "未选择函数" : TempData.action.funcName;

            if (SirenixEditorGUI.Button(buttonText, ButtonSizes.Medium))
            {
                FuncWindow.Open(TempData.action, null, true);
            }

            if (AbilityFuncInfoCache.TryGetFuncInfo(TempData.action.funcName, out var funcInfo))
            {
                TempData.returnType = funcInfo.ReturnType.ToString();

                if (funcInfo.ReturnType != typeof(void))
                {
                    TempData.isCreateVariable = EditorGUILayout.Toggle("是将返回值作为变量存储", TempData.isCreateVariable);
                }
                else
                {
                    TempData.isCreateVariable = false;
                }

                if (TempData.isCreateVariable)
                {
                    TempData.returnValueKey = SirenixEditorFields.TextField("变量key值：", TempData.returnValueKey);
                }
            }


            //调用函数 且捕获返回值
            SirenixEditorGUI.EndBox();
        }
    }
}