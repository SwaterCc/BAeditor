using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class LogicTreeNodeSelectionWindow : OdinEditorWindow
    {
        public static void OpenWindow(LogicTreeDrawer treeDrawer, LogicTreeNodeDrawer selectNodeDrawer)
        {
            var window = GetWindow<LogicTreeNodeSelectionWindow>(false, "对节点进行操作", true);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 300);
            window.init(treeDrawer, selectNodeDrawer);
        }

        private LogicTreeDrawer _treeDrawer;
        private LogicTreeNodeDrawer _selectNodeDrawer;

        private void init(LogicTreeDrawer treeDrawer, LogicTreeNodeDrawer selectNodeDrawer)
        {
            _treeDrawer = treeDrawer;
            _selectNodeDrawer = selectNodeDrawer;
        }

        public bool SelectNodeDrawerIsNull => _selectNodeDrawer == null;

        [LabelWidth(250)] [LabelText("创建节点后是否直接打开配置界面")]
        public bool isOpenSettingWindow = true;


        [ShowIf("SelectNodeDrawerIsNull")]
        [BoxGroup("初始化节点")]
        [Button("创建事件节点")]
        public void CreateEventNode()
        {
            var drawer = LogicTreeNodeDrawer.CreateNodeDrawerAndAddToTree(_treeDrawer, null, ENodeType.Event);
            _treeDrawer.TreeData.rootKey = drawer.GetNodeKey();
            _treeDrawer.SetRootDrawer(drawer);
            this.Close();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("添加同级节点")]
        [Button("添加If节点")]
        public void CreateConditionNode()
        {
            var drawer = LogicTreeNodeDrawer.CreateNodeDrawerAndAddToTree(_treeDrawer, _selectNodeDrawer.GetParent(), ENodeType.Condition);
            this.Close();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("添加子节点")]
        [Button("添加If节点")]
        public void CreateConditionNodeIntoParent()
        {
            var drawer = LogicTreeNodeDrawer.CreateNodeDrawerAndAddToTree(_treeDrawer, _selectNodeDrawer, ENodeType.Condition);
            this.Close();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("添加同级节点")]
        [Button("创建Action节点")]
        public void CreateActionNode()
        {
            var drawer = LogicTreeNodeDrawer.CreateNodeDrawerAndAddToTree(_treeDrawer, _selectNodeDrawer.GetParent(), ENodeType.Action);
            this.Close();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("添加子节点")]
        [Button("创建Action节点")]
        public void CreateActionNodeIntoParent()
        {
            var drawer = LogicTreeNodeDrawer.CreateNodeDrawerAndAddToTree(_treeDrawer, _selectNodeDrawer, ENodeType.Action);
            this.Close();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("添加同级节点")]
        [Button("创建变量")]
        public void CreateVariableNode()
        {
            var drawer = LogicTreeNodeDrawer.CreateNodeDrawerAndAddToTree(_treeDrawer, _selectNodeDrawer.GetParent(), ENodeType.Variable);
            this.Close();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("添加子节点")]
        [Button("创建变量")]
        public void CreateVariableNodeIntoParent()
        {
            var drawer = LogicTreeNodeDrawer.CreateNodeDrawerAndAddToTree(_treeDrawer, _selectNodeDrawer, ENodeType.Variable);
            this.Close();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("删除节点")]
        [Button("删除选中节点及其子节点")]
        public void DeleteSelectNode()
        {
            _selectNodeDrawer.RemoveSelf();
        }

        [HideIf("SelectNodeDrawerIsNull")]
        [TabGroup("删除节点")]
        [Button("清除选中节点的所有子节点")]
        public void DleteSelectNodeAllChildren()
        {
            _selectNodeDrawer.RemoveAllChildren();
        }
    }
}