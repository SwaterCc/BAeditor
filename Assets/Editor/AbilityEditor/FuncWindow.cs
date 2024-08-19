using System;
using Editor.AbilityEditor.TreeItem;
using UnityEditor;

namespace Editor.AbilityEditor
{
    public class FuncWindow : EditorWindow
    {
        public static void Open(object node)
        {
            var window = CreateInstance<FuncWindow>();
            window.init((ParameterNode)node);
            window.Show();
        }

        public static void Open(ParameterNode node)
        {
            var window = CreateInstance<FuncWindow>();
            window.init(node);
            window.Show();
        }

        private ParameterNode _funcHead;

        private void init(ParameterNode node)
        {
            _funcHead = node;
        }


        public void OnDestroy()
        {
            _funcHead.ChangeToFunc();
        }
    }
}