using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Battle;
using Battle.Tools;
using Battle.Tools.CustomAttribute;
using Editor.AbilityEditor.TreeItem;
using NUnit.Framework;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class FuncWindow : EditorWindow
    {
        public static void Open(object node, EFuncCacheFlag flag)
        {
            var window = CreateInstance<FuncWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.init((ParameterNode)node);
            window.Show();
        }

        public static void Open(ParameterNode node, EFuncCacheFlag flag)
        {
            var window = CreateInstance<FuncWindow>();
            window.init(node);
            window.Show();
        }

        private struct FuncInfo
        {
            public string FuncName;
            public int ParamCount;
            public List<string> ParamNames;
        }

        private ParameterNode _funcHead;

        private static Dictionary<EFuncCacheFlag, List<FuncInfo>> _flagMethodCache =
            new Dictionary<EFuncCacheFlag, List<FuncInfo>>()
            {
                { EFuncCacheFlag.Variable, new List<FuncInfo>() },
                { EFuncCacheFlag.Action, new List<FuncInfo>() },
                { EFuncCacheFlag.Branch, new List<FuncInfo>() },
            };

        private static Dictionary<string, List<MethodInfo>> _methodCache =
            new Dictionary<string, List<MethodInfo>>();

        public static Dictionary<string, List<MethodInfo>> MethodCache
        {
            get
            {
                if (_methodCache.Count == 0)
                {
                    initFuncCache();
                }

                return _methodCache;
            }
        }

        private FuncList _funcTree;

        private void init(ParameterNode node)
        {
            if (_flagMethodCache.Count == 0)
            {
                initFuncCache();
            }

            _funcHead = node;
            switch (_funcHead.Sel) { }

            _funcTree = new FuncList(new TreeViewState(), _funcHead, _flagMethodCache[])
        }

        private static void initFuncCache()
        {
            // 获取 MyStaticClass 类型信息
            Type type = typeof(AbilityCacheFuncDefine);

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                AbilityFuncCache attr = null;
                foreach (var obj in method.GetCustomAttributes(typeof(AbilityFuncCache), false))
                {
                    if (obj is AbilityFuncCache cache)
                    {
                        attr = cache;
                    }
                }

                if (attr == null) continue;

                if (!_methodCache.TryGetValue(method.Name, out var methodInfos))
                {
                    methodInfos = new List<MethodInfo>();
                    _methodCache.Add(method.Name, methodInfos);
                }

                methodInfos.Add(method);

                var info = new FuncInfo
                {
                    FuncName = method.Name,
                    ParamCount = method.GetParameters().Length
                };
                foreach (var parameter in method.GetParameters())
                {
                    info.ParamNames.Add(parameter.Name);
                }

                if ((attr.Flag & EFuncCacheFlag.Variable) > 0)
                {
                    _flagMethodCache[EFuncCacheFlag.Variable].Add(info);
                }

                if ((attr.Flag & EFuncCacheFlag.Action) > 0)
                {
                    _flagMethodCache[EFuncCacheFlag.Action].Add(info);
                }

                if ((attr.Flag & EFuncCacheFlag.Branch) > 0)
                {
                    _flagMethodCache[EFuncCacheFlag.Branch].Add(info);
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            //函数列表界面
            //_funcTree.OnGUI(new Rect(0,0,300,400));
            //函数预览界面
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginVerticalList();
            foreach (var method in _methodCache[_funcTree.CurSelect])
            {
                SirenixEditorGUI.BeginListItem();
                foreach (var param in method.GetParameters())
                {
                    EditorGUILayout.LabelField(param.Name);
                    EditorGUILayout.LabelField(param.ParameterType.ToString());
                }

                SirenixEditorGUI.EndListItem();
            }

            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

            EditorGUILayout.EndHorizontal();
            //配置界面
            SirenixEditorGUI.BeginBox();
            //_funcHead.Draw();
            SirenixEditorGUI.EndBox();
            EditorGUILayout.EndVertical();
        }


        public void OnDestroy() { }


        private class FuncList : TreeView
        {
            private List<FuncInfo> _infos;

            public string CurSelect;
            private ParameterNode _funcHead;

            public FuncList(TreeViewState state, ParameterNode funcHead, List<FuncInfo> infos) : base(state)
            {
                _infos = infos;
                _funcHead = funcHead;
                CurSelect = funcHead.Self.FuncName ?? "none";
            }

            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

                int idx = 1;

                foreach (var funcInfo in _infos)
                {
                    var func = new TreeViewItem() { id = ++idx, depth = 0, displayName = funcInfo.FuncName };
                    root.AddChild(func);
                }

                return root;
            }

            protected override void ContextClickedItem(int id)
            {
                CurSelect = FindItem(id, rootItem).displayName;
            }

            protected override void DoubleClickedItem(int id)
            {
                var item = FindItem(id, rootItem);
                _funcHead.Create(new Parameter() { IsFunc = true, FuncName = item.displayName });
            }
        }
    }
}