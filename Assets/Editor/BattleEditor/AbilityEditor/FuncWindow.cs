using System;
using System.Collections.Generic;
using System.Reflection;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class FuncWindow : EditorWindow
    {
        public static void OpenVariableToFunc(object node)
        {
            var window = CreateInstance<FuncWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            var maker = new ParameterMaker();
            maker.ChangeToDefaultFunc(EFuncCacheFlag.Variable);
            window.init((ParameterMaker)node, EFuncCacheFlag.Variable);
            window.Show();
        }

        public static void Open(ParameterMaker maker, EFuncCacheFlag flag)
        {
            var window = CreateInstance<FuncWindow>();
            window.init(maker, flag);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.Show();
        }

        #region 函数缓存

        public struct FuncInfo
        {
            public string FuncName;
            public int ParamCount;
            public List<string> ParamNames;
        }

        private static Dictionary<EFuncCacheFlag, List<FuncInfo>> _flagMethodCache =
            new Dictionary<EFuncCacheFlag, List<FuncInfo>>()
            {
                { EFuncCacheFlag.Variable, new List<FuncInfo>() },
                { EFuncCacheFlag.Action, new List<FuncInfo>() },
                { EFuncCacheFlag.Branch, new List<FuncInfo>() },
            };

        private static bool _flagMethodCacheInit = false;

        public static Dictionary<EFuncCacheFlag, List<FuncInfo>> FlagMethodCache
        {
            get
            {
                if (!_flagMethodCacheInit)
                {
                    initFuncCache();
                }

                return _flagMethodCache;
            }
        }

        private static Dictionary<string, MethodInfo> _methodCache =
            new Dictionary<string, MethodInfo>();

        public static Dictionary<string, MethodInfo> MethodCache
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

        private static void initFuncCache()
        {
            // 获取 MyStaticClass 类型信息
            Type type = typeof(AbilityFunction);

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

                if (!_methodCache.TryGetValue(method.Name, out var methodInfo))
                {
                    methodInfo = method;
                    _methodCache.Add(method.Name, method);
                }

                var info = new FuncInfo
                {
                    FuncName = method.Name,
                    ParamCount = method.GetParameters().Length
                };
                foreach (var parameter in method.GetParameters())
                {
                    if (info.ParamNames == null)
                    {
                        info.ParamNames = new List<string>();
                    }

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

            _flagMethodCacheInit = true;
        }
        #endregion

        private ParameterMaker _origin;
        private ParameterMaker _funcHead;
        private EFuncCacheFlag _flag;
        
        private FuncList _funcTree;

        private void init(ParameterMaker maker, EFuncCacheFlag flag)
        {
            if (_methodCache.Count == 0)
            {
                initFuncCache();
            }

            _origin = maker;
            _funcHead = new ParameterMaker();
            ParameterMaker.Init(_funcHead,_funcHead.ToArray());

            _flag = flag;
            _funcTree = new FuncList(new TreeViewState(), _funcHead, _flagMethodCache[_flag]);
        }
        
        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            //函数列表界面
            GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(280));
            _funcTree.OnGUI(new Rect(5, 5, 300, 280));
            //函数预览界面
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginVerticalList();
            if (_methodCache.TryGetValue(_funcTree.CurSelect, out var method))
            {
                foreach (var param in method.GetParameters())
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.LabelField("参数名：" + param.Name);
                    EditorGUILayout.LabelField("参数类型：" + param.ParameterType);
                    SirenixEditorGUI.EndListItem();
                }

                if (method.ReturnType != typeof(void))
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.LabelField("返回类型：" + method.ReturnType);
                    SirenixEditorGUI.EndListItem();
                }
                else
                {
                    SirenixEditorGUI.BeginListItem();
                    EditorGUILayout.LabelField("无返回值");
                    SirenixEditorGUI.EndListItem();
                }
            }

            SirenixEditorGUI.EndVerticalList();
            SirenixEditorGUI.EndBox();

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(30);
            //配置界面
            SirenixEditorGUI.BeginBox($"当前函数:{_funcHead.Self.FuncName}", true);
            if (_funcHead.FuncParams.Count == 0)
            {
                EditorGUILayout.LabelField("无参函数");
            }
            else
            {
                foreach (var param in _funcHead.FuncParams)
                {
                    param.Draw();
                }
            }

            if (GUILayout.Button("确认修改"))
            {
                _origin = _funcHead;
                AbilityEditorHelper.SearchText = "";
                Close();
            }

            SirenixEditorGUI.EndBox();
            EditorGUILayout.EndVertical();
        }


        private class FuncList : TreeView
        {
            private List<FuncInfo> _infos;

            public string CurSelect;
            private ParameterMaker _funcHead;

            public FuncList(TreeViewState state, ParameterMaker funcHead, List<FuncInfo> infos) : base(state)
            {
                _infos = infos;
                _funcHead = funcHead;
                if (funcHead.Self.IsFunc && string.IsNullOrEmpty(funcHead.Self.FuncName))
                {
                    funcHead.Self.FuncName = infos[0].FuncName;
                }

                CurSelect = funcHead.Self.FuncName;
                showAlternatingRowBackgrounds = true;
                showBorder = true;
                Reload();
                foreach (var item in rootItem.children)
                {
                    if (item.displayName == CurSelect)
                    {
                        SelectionClick(item, false);
                    }
                }
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                base.RowGUI(args);
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    Rect rowRect = args.rowRect;
                    if (rowRect.Contains(Event.current.mousePosition))
                    {
                        CurSelect = FindItem(args.item.id, rootItem).displayName;
                        //Event.current.Use(); // 使用事件以防止其他控件处理它
                    }
                }
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

            protected override void DoubleClickedItem(int id)
            {
                var item = FindItem(id, rootItem);
                _funcHead.CreateFuncParam(item.displayName);
            }
        }
    }
}