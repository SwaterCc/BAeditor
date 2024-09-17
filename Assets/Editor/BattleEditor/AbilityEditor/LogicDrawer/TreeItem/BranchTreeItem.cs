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
        private BranchNodeData _branchNodeData;

        public BranchTreeItem(AbilityLogicTree tree, AbilityNodeData nodeData) : base(tree, nodeData)
        {
            _branchNodeData = (BranchNodeData)_nodeData;
        }

        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("创建节点/添加Action"), false,
                AddChild, (EAbilityNodeType.EAction));
            _menu.AddItem(new GUIContent("创建节点/添加If"), false,
                AddChild, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("创建节点/添加ElseIf"), false,
                AddElseIfNode, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("创建节点/Set变量"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("创建节点/SetAttr"), false,
                AddChild, (EAbilityNodeType.EAttrSetter));
            _menu.AddItem(new GUIContent("创建节点/创建Event节点"), false,
                AddChild, (EAbilityNodeType.EEvent));
           
            _menu.AddItem(new GUIContent("删除"), false,
                Remove);
        }
        
        private void AddElseIfNode(object obj)
        {
            var node = (BranchNodeData)_tree.TreeData.GetNodeData(EAbilityNodeType.EBranchControl);
            node.ParentId = _branchNodeData.ParentId;
            node.Depth = _branchNodeData.Depth;
            node.BranchGroup = _branchNodeData.BranchGroup;
            node.Desc = "else if";
            var parentNode = _tree.TreeData.NodeDict[node.ParentId];
            var index = parentNode.ChildrenIds.IndexOf(_nodeData.NodeId);
            parentNode.ChildrenIds.Insert(index + 1, node.NodeId);
            _tree.TreeData.NodeDict.Add(node.NodeId, node);
            
            EditorUtility.SetDirty(_tree.TreeData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            _tree.Reload();
        }

        protected override Color getButtonColor()
        {
            return Color.cyan;
        }

        protected override string getButtonText()
        {
            string label = string.IsNullOrEmpty(_nodeData.Desc)
                ? "If"
                : _nodeData.Desc;

            return label;
        }

        protected override string getButtonTips()
        {
            return "执行if判定，如果判定成功则执行子节点内容，判定失败则走同级的下一节点";
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            SettingWindow = BaseNodeWindow<BranchNodeDataWindow, BranchNodeData>.GetSettingWindow(_tree.TreeData,
                _branchNodeData,
                (nodeData) => _tree.TreeData.NodeDict[nodeData.NodeId] = nodeData);
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class BranchNodeDataWindow : BaseNodeWindow<BranchNodeDataWindow, BranchNodeData>, IAbilityNodeWindow<BranchNodeData>
    {
        private ParameterField _compareFunc;

        protected override void onInit()
        {
            _compareFunc = new ParameterField(_nodeData.CompareFunc, "判定条件", typeof(bool));
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("If节点", true);
            EditorGUILayout.BeginVertical();
            _compareFunc.Draw();
            EditorGUILayout.EndVertical();
            SirenixEditorGUI.BeginBox();
            _nodeData.Desc = EditorGUILayout.TextField("节点描述:", _nodeData.Desc);
            if (SirenixEditorGUI.Button("保   存", ButtonSizes.Medium))
            {
                Save();
            }

            SirenixEditorGUI.EndBox();
            SirenixEditorGUI.EndBox();
        }
    }
}