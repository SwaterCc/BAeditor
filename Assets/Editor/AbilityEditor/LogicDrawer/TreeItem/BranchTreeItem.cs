using System;
using Battle;
using Battle.Def;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class BranchTreeItem: AbilityLogicTreeItem
    {
        public BranchTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public BranchTreeItem(AbilityNodeData nodeData) : base(nodeData) { }
        protected override Color getButtonColor()
        {
           return Color.cyan;
        }

        protected override string getButtonText()
        {
            return "分支";
        }

        protected override void OnBtnClicked()
        {
            BranchNodeDataWindow.Open(NodeData);
        }
    }

    public class BranchNodeDataWindow : BaseNodeWindow<BranchNodeDataWindow>, IWindowInit
    {
        private ParameterNode _left;
        private ParameterNode _right;
        protected override void onInit()
        {
            _left = new ParameterNode();
            _left.Parse(NodeData.BranchNodeData.Left);
            _right = new ParameterNode();
            _right.Parse(NodeData.BranchNodeData.Right);
        }

        private void changeCompareType(object type)
        {
            NodeData.BranchNodeData.ResType = (ECompareResType)type;
        }

        private string getFlag(ECompareResType compareResType)
        {
            switch (compareResType)
            {
                case ECompareResType.Less:
                    return "<";
                case ECompareResType.LessAndEqual:
                    return "<=";
                case ECompareResType.Equal:
                    return "==";
                case ECompareResType.More:
                    return ">";
                case ECompareResType.MoreAndEqual:
                    return ">=";
            }

            return "?";
        }
        
        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox();
            EditorGUILayout.BeginHorizontal();
            _left.Draw();
            if (GUILayout.Button(getFlag(NodeData.BranchNodeData.ResType), GUILayout.Width(10)))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("<"), true, changeCompareType, ECompareResType.Less);
                menu.AddItem(new GUIContent("<="), true, changeCompareType, ECompareResType.LessAndEqual);
                menu.AddItem(new GUIContent("=="), true, changeCompareType, ECompareResType.Equal);
                menu.AddItem(new GUIContent(">"), true, changeCompareType, ECompareResType.More);
                menu.AddItem(new GUIContent(">="), true,changeCompareType, ECompareResType.MoreAndEqual);
                menu.ShowAsContext();
            }
            _right.Draw();
            EditorGUILayout.EndHorizontal();
            SirenixEditorGUI.EndBox();
        }
    }
}