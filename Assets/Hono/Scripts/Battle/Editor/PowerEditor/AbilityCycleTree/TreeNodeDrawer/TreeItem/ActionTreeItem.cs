using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class ActionTreeItem : ATreeItem<ActionNodeData>
    {
        private readonly MenuInfo _menuInfo;
        public ActionTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = new Color(2f, 0.5f, 0.5f);
            _menuInfo = addCustomMenu("获取返回值", ERightClickOperationType.GetResult, null, null);
        }

        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            if (info.OperationType < ERightClickOperationType.AddChildOperation)
            {
                return ERightMenuState.NoShow;
            }

            if (info.OperationType == ERightClickOperationType.GetResult)
            {
                if (string.IsNullOrEmpty(Data.Function.funcName))
                {
                    _menuInfo.Label = "函数未初始化，无法获取返回值";
                    return ERightMenuState.Disable;
                }

                if (!AbilityFunctionHelper.TryGetFuncInfo(Data.Function.funcName, out var funcInfo))
                    return ERightMenuState.Disable;
                
                if (funcInfo.ReturnType == typeof(void))
                {
                    _menuInfo.Label = "函数无返回值";
                    return ERightMenuState.Disable;
                }

                _menuInfo.Label = "获取返回值";
                _menuInfo.Function += data => Node.AddChild(new AEditorTreeNode((AbilityNodeData)data));
                _menuInfo.Param = new VariableNodeData() { IsGetReturnValue = true };
                return ERightMenuState.Enable;
            }

            return ERightMenuState.Enable;
        }

        protected override bool checkIsAllowMove(ATreeItem newParent)
        {
            if (newParent is AttrTreeItem or VariableTreeItem or ActionTreeItem or BranchGroupTreeItem)
            {
                return false;
            }

            return true;
        }

        protected override string getButtonText()
        {
            if (Data.Function.paramType != EParamType.Function)
            {
                return "未指定函数！";
            }

            return Data.Function.ToString();
        }
        
        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<ActionSettingWindow>(this);
        }
    }
    
    public class ActionSettingWindow : ANodeSettingWindow<ActionNodeData>
    {
        private AParamsField _function;

        protected override void Init()
        {
            //下拉框，选择函数类型，
           // _function = new AParamsField<RefBoolean>(TempData.CompareFunc, "判定条件");
            
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("If节点", true);
           // _compareFunc.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}