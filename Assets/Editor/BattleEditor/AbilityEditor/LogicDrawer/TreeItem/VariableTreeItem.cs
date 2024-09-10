
using System.Linq;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class VariableTreeItem : AbilityLogicTreeItem
    {
        public VariableTreeItem(int id, int depth, string name) : base(id, depth, name) { }
        public VariableTreeItem(AbilityNodeData nodeData) : base(nodeData) { }

        protected override Color getButtonColor()
        {
            return new Color(0.6f, 0.3f, 0.95f);
        }

        protected override string getButtonText()
        {
            if (string.IsNullOrEmpty(NodeData.VariableNodeData.Desc))
            {
                return $"调用并Set变量 {NodeData.VariableNodeData.Name} = {NodeData.VariableNodeData.VarParams.ToArray()}";
            }

            return NodeData.VariableNodeData.Desc;
        }

        protected override string getItemEffectInfo()
        {
            return "调用有返回值的Action";
        }

        protected override void OnBtnClicked()
        {
            VariableNodeDataWindow.Open(NodeData);
        }
    }

    public class VariableNodeDataWindow : BaseNodeWindow<VariableNodeDataWindow>, IWindowInit
    {
        private ParameterMaker _variable;

        protected override void onInit()
        {
            _variable = new ParameterMaker();
            ParameterMaker.Init(_variable,NodeData.VariableNodeData.VarParams);
        }

        public override Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(400, 140);
        }
        
        private void OnDestroy()
        {
            NodeData.VariableNodeData.VarParams = _variable.ToArray();
        }

        private void OnGUI()
        {
            string a = NodeData.VariableNodeData.OperationType == EVariableOperationType.Create ? "创建变量：" : "修改变量";
            SirenixEditorGUI.BeginBox(a);

            NodeData.VariableNodeData.OperationType =
                (EVariableOperationType)SirenixEditorFields.EnumDropdown("选择操作",
                    NodeData.VariableNodeData.OperationType);

            NodeData.VariableNodeData.Range =
                (EVariableRange)SirenixEditorFields.EnumDropdown("选择范围", NodeData.VariableNodeData.Range);

            
            NodeData.VariableNodeData.Name =
                SirenixEditorFields.TextField("变量名", NodeData.VariableNodeData.Name);

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("变量值");
            
            _variable.Draw();
            
            EditorGUILayout.EndHorizontal();
            
            SirenixEditorGUI.EndBox();
        }
    }
}