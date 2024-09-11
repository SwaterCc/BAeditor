using System.Linq;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class VariableTreeItem : AbilityLogicTreeItem
    {
        private ParameterMaker _variable;
        public VariableTreeItem(int id, int depth, string name) : base(id, depth, name) { }

        public VariableTreeItem(AbilityNodeData nodeData) : base(nodeData)
        {
            _variable = new ParameterMaker();
        }

        protected override Color getButtonColor()
        {
            return new Color(0.6f, 0.3f, 0.95f);
        }

        protected override string getButtonText()
        {
            if (string.IsNullOrEmpty(NodeData.VariableNodeData.Desc))
            {
	            ParameterMaker.Init(_variable, NodeData.VariableNodeData.VarParams);
                return $"调用并Set变量 {NodeData.VariableNodeData.Name} = {_variable}";
            }

            return NodeData.VariableNodeData.Desc;
        }

        protected override string getItemEffectInfo()
        {
            return "调用有返回值的Action";
        }

        protected override void OnBtnClicked()
        {
            SettingWindow = VariableNodeDataWindow.GetWindow(NodeData);
            SettingWindow.Show();
            SettingWindow.Focus();
        }
    }

    public class VariableNodeDataWindow : BaseNodeWindow<VariableNodeDataWindow>, IWindowInit
    {
        private ParameterMaker _variable;
        private EVariableRange _range;
        private string _name;
        private ParameterMaker _actorUid;
        private ParameterMaker _abilityUid;
        protected override void onInit() {
	        _range = NodeData.VariableNodeData.Range;
	        _name = NodeData.VariableNodeData.Name;
            _variable = new ParameterMaker();
            ParameterMaker.Init(_variable, NodeData.VariableNodeData.VarParams);
            _actorUid = new ParameterMaker();
            ParameterMaker.Init(_actorUid,  NodeData.VariableNodeData.ActorUid);
            _abilityUid = new ParameterMaker();
            ParameterMaker.Init(_abilityUid, NodeData.VariableNodeData.AbilityUid);
        }

        public override Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(740, 200);
        }

        private void Save() {
	        NodeData.VariableNodeData.Range = _range;
	        NodeData.VariableNodeData.Name = _name;
            NodeData.VariableNodeData.VarParams = _variable.ToArray();
	        NodeData.VariableNodeData.ActorUid = _actorUid.ToArray();
	        NodeData.VariableNodeData.AbilityUid = _abilityUid.ToArray();
            Close();
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox();

            EditorGUIUtility.labelWidth = 70;
            
            NodeData.VariableNodeData.OperationType =
                (EVariableOperationType)SirenixEditorFields.EnumDropdown("选择操作",
                    NodeData.VariableNodeData.OperationType);

            _range = (EVariableRange)SirenixEditorFields.EnumDropdown("选择范围", _range);

            if (_range != EVariableRange.Battleground) {
	            EditorGUILayout.BeginHorizontal();
	            EditorGUIUtility.labelWidth = 70;
	            EditorGUILayout.LabelField("ActorUid");
	            _actorUid.Draw("-> 设置ActorUid");
	            EditorGUILayout.EndHorizontal();
            }

            if (_range == EVariableRange.Ability) {
	            EditorGUILayout.BeginHorizontal();
	            EditorGUIUtility.labelWidth = 70;
	            EditorGUILayout.LabelField("AbilityUid");
	            _abilityUid.Draw("-> 设置AbilityUid");
	            EditorGUILayout.EndHorizontal();
            }
            EditorGUIUtility.labelWidth = 70;
            _name = SirenixEditorFields.TextField("变量名", _name);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.LabelField("变量值");
            _variable.Draw("-> 设置变量值");
            EditorGUILayout.EndHorizontal();
            
           
            if (SirenixEditorGUI.Button("保   存", ButtonSizes.Medium))
            {
                Save();
            }
            SirenixEditorGUI.EndBox();
        }
    }
}