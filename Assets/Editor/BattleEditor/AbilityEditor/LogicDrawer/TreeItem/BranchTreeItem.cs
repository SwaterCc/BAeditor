
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class BranchTreeItem : AbilityLogicTreeItem
    {
        public BranchTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public BranchTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        private ParameterMaker _left;
        private ParameterMaker _right;
        
        protected override Color getButtonColor()
        {
            return Color.cyan;
        }
        
        protected override string getButtonText()
        {
            string label = string.IsNullOrEmpty(NodeData.BranchNodeData.Desc)
                ? "分支（无描述）"
                : NodeData.BranchNodeData.Desc;
            
            return label;
        }

        protected override string getItemEffectInfo()
        {
            return "执行if判定，如果判定成功则执行子节点内容，判定失败则走同级的下一节点";
        }

        protected override void OnBtnClicked()
        {
            SettingWindow = BranchNodeDataWindow.GetWindow(NodeData);
            SettingWindow.Show();
            SettingWindow.Focus();
        }
    }

    public class BranchNodeDataWindow : BaseNodeWindow<BranchNodeDataWindow>, IWindowInit
    {
        private ParameterMaker _left;
        private ParameterMaker _right;
        private ECompareResType _type;
        protected override void onInit()
        {
            _left = new ParameterMaker();
            ParameterMaker.Init(_left, NodeData.BranchNodeData.Left);

            _type = NodeData.BranchNodeData.ResType;
            
            _right = new ParameterMaker();
            ParameterMaker.Init(_right, NodeData.BranchNodeData.Right);
        }

        public override Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(700, 170);
        }

        private void changeCompareType(object type)
        {
            _type = (ECompareResType)type;
        }

        private void saveData()
        {
            NodeData.BranchNodeData.Left = _left.ToArray();
            NodeData.BranchNodeData.ResType = _type;
            NodeData.BranchNodeData.Right = _right.ToArray();
            Close();
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
            EditorGUILayout.BeginVertical();
            _left.Draw();
            if (GUILayout.Button(getFlag(_type)))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("<"), false, changeCompareType, ECompareResType.Less);
                menu.AddItem(new GUIContent("<="), false, changeCompareType, ECompareResType.LessAndEqual);
                menu.AddItem(new GUIContent("=="), false, changeCompareType, ECompareResType.Equal);
                menu.AddItem(new GUIContent(">"), false, changeCompareType, ECompareResType.More);
                menu.AddItem(new GUIContent(">="), false, changeCompareType, ECompareResType.MoreAndEqual);
                menu.ShowAsContext();
            }

            _right.Draw();
            EditorGUILayout.EndVertical();
            SirenixEditorGUI.BeginBox();
            NodeData.BranchNodeData.Desc = EditorGUILayout.TextField("节点描述:", NodeData.BranchNodeData.Desc);
            if (SirenixEditorGUI.Button("保   存", ButtonSizes.Medium))
            {
                saveData();
            }
            SirenixEditorGUI.EndBox();
            SirenixEditorGUI.EndBox();
        }
        
        
    }
}