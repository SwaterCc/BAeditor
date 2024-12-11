using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class BranchTreeItem : ATreeItem<BranchNodeData>
    {
        public BranchTreeItem(AbilityCycleTree tree, AEditorTreeNode data) : base(tree, data)
        {
            ButtonBackGroundColor = Color.cyan;
            addCustomMenu("合并为分支组", ERightClickOperationType.JoinBranchGroup, null, null);
        }
        
        protected override ERightMenuState checkRightMenuState(MenuInfo info)
        {
            return ERightMenuState.Enable;
        }

        protected override string getButtonText()
        {
            string label = "If:";
            if (parent is BranchGroupTreeItem)
            {
                label = "Case:";
            }

            return label + " : " + Data.CompareFunc;
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            ANodeSettingWindow.Open<BranchSettingWindow>(this);
        }
    }

    public class BranchSettingWindow : ANodeSettingWindow<BranchNodeData>
    {
        private ParameterField _compareFunc;

        protected override void Init()
        {
            _compareFunc = new ParameterField(TempData.CompareFunc, "判定条件", typeof(bool));
        }

        protected override void Draw()
        {
            SirenixEditorGUI.BeginBox("If节点", true);
            _compareFunc.Draw();
            SirenixEditorGUI.EndBox();
        }
    }
}