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
        public static int WindowCount = 0;
        public static float xOffset = 30;
        public static float yOffset = -20;
        
        public static void OpenVariableToFunc(object node)
        {
            var window = CreateInstance<FuncWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            (ParameterMaker maker,string str) pair = ((ParameterMaker maker, string str))node;
            pair.maker.ChangeToDefaultFunc(EFuncCacheFlag.Variable);
            window.Init(pair.maker, EFuncCacheFlag.Variable);
            window.Show();
            window.FromString += pair.str;
        }

        public static FuncWindow Open(ParameterMaker maker, EFuncCacheFlag flag)
        {
            var window = CreateInstance<FuncWindow>();
            window.Init(maker, flag);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.Show();
            return window;
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
        public string FromString;

        public void Init(ParameterMaker maker, EFuncCacheFlag flag)
        {
            if (_methodCache.Count == 0)
            {
                initFuncCache();
            }

            _origin = maker;
            _funcHead = new ParameterMaker();
            ParameterMaker.Init(_funcHead, maker.ToArray());

            _flag = flag;
            _funcTree = new FuncList(new TreeViewState(), _funcHead, this, _flagMethodCache[_flag]);
        }

        private void OnDoubleClick(string funcName)
        {
            _funcHead.CreateFuncParam(funcName);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            SirenixEditorGUI.Title(FromString,"",TextAlignment.Center,true);
            EditorGUILayout.BeginHorizontal();
            //函数列表界面
            GUILayout.Box("", GUILayout.Width(300), GUILayout.Height(280));
            var rect = GUIHelper.GetCurrentLayoutRect();
            _funcTree.OnGUI(new Rect(rect.x, rect.y, 300, 280));
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
                   // param.Draw(FromString +" -> 设置变量：" +param.Self.ParamName);
                }
            }

            if (GUILayout.Button("确认修改"))
            {
                ParameterMaker.Init(_origin, _funcHead.ToArray());
                _origin.Save();
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

            private FuncWindow _window;

            public FuncList(TreeViewState state, ParameterMaker funcHead, FuncWindow window,
                List<FuncInfo> infos) : base(state)
            {
                _infos = infos;
                _funcHead = funcHead;
                _window = window;
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
               _window.OnDoubleClick(item.displayName);
            }
        }
    }
}