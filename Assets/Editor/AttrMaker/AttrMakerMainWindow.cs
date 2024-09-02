using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.AbilityEditor;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.AttrMaker
{
    [Serializable]
    public class AttrTemplateItem
    {
        [LabelText("属性变量名")] [HorizontalGroup("a")]
        public string AttrName;

        [LabelText("属性描述")] [HorizontalGroup("a")]
        public string AttrDesc;

        [LabelText("属性值类型")]
        [HorizontalGroup("a")]
        [ValueDropdown("attrTypeList")]
        [OnValueChanged("OnDropdownValueChanged")]
        public string AttrType;

        [LabelText("属性默认值")] [HorizontalGroup("a")] [OdinSerialize]
        public object AttrDefaultValue;

        [NonSerialized] private IEnumerable attrTypeList = new ValueDropdownList<string>()
        {
            { "int", typeof(Int32).ToString() },
            { "long", typeof(Int64).ToString() },
            { "float", typeof(Single).ToString() },
            { "bool", typeof(Boolean).ToString() },
        };

        private void OnDropdownValueChanged()
        {
            var valueType = Type.GetType(AttrType);
            if (valueType != null)
            {
                AttrDefaultValue = valueType.InstantiateDefault(true);
            }
        }
    }

    public class AttrMakerMainWindow : OdinMenuEditorWindow
    {
        [MenuItem("战斗编辑器/打开AttrMaker")]
        private static void OpenWindow()
        {
            var window = GetWindow<AttrMakerMainWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.titleContent = new GUIContent("AttrMaker");
        }

        public static string AttrTemplatesResourcePath = "Assets/Resources/Attr/AttrTemplates";

        public static string PawnAttrResourcePath = "Assets/Resources/Attr/ActorAttrConfig/Pawn";
        public static string MonsterAttrResourcePath = "Assets/Resources/Attr/ActorAttrConfig/Monster";
        public static string BulletAttrResourcePath = "Assets/Resources/Attr/ActorAttrConfig/Bullet";
        public static string HitBoxAttrResourcePath = "Assets/Resources/Attr/ActorAttrConfig/HitBox";

        protected override void Initialize()
        {
            base.Initialize();
        }

        public static string[] LoadFolder(string folderPath)
        {
            // 获取指定文件夹路径下所有 .asset 文件的 GUID 数组
            string[] assetGUIDs = AssetDatabase.FindAssets("t:object", new[] { folderPath });

            // 创建一个字符串数组来存储文件名
            string[] assetFileNames = new string[assetGUIDs.Length];

            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                // 使用 GUID 获取每个资产的路径
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);

                // 检查文件是否为 .asset 文件
                if (assetPath.EndsWith(".asset"))
                {
                    // 获取文件名（带扩展名）
                    assetFileNames[i] = System.IO.Path.GetFileName(assetPath);
                }
            }

            return assetFileNames;
        }

        private static string _actorAttrTreeName = "Actor属性定义/";
        private static string _pawnAttrTreeName = $"{_actorAttrTreeName}/Pawn/";
        private static string _monsterAttrTreeName = $"{_actorAttrTreeName}/Monster/";
        private static string _bulletAttrTreeName = $"{_actorAttrTreeName}/Bullet/";
        private static string _hitBoxAttrTreeName = $"{_actorAttrTreeName}/HitBox/";

        public List<AttrTemplate> attrTemplates = new List<AttrTemplate>();

        //创建模板
        public void AddItems(OdinMenuTree tree, string rootPath, string folderPath)
        {
            var files = LoadFolder(folderPath);
            foreach (var fileFullName in files)
            {
                var attrDefine = AssetDatabase.LoadAssetAtPath<AttrDefine>(folderPath + "/" + fileFullName);
                if (attrDefine != null)
                {
                    tree.Add(rootPath + attrDefine.name, attrDefine);
                }
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var treeInstance = new OdinMenuTree(false)
            {
                { "属性模板", new AttrTemplateCreator(ref attrTemplates) },
                { _actorAttrTreeName, null },
                { _pawnAttrTreeName, new ActorAttrRootView() },
                { _monsterAttrTreeName, new ActorAttrRootView() },
                { _bulletAttrTreeName, new ActorAttrRootView() },
                { _hitBoxAttrTreeName, new ActorAttrRootView() },
            };

            foreach (var attrTemplate in attrTemplates)
            {
                treeInstance.Add("属性模板/" + attrTemplate.name, attrTemplate);
            }

            AddItems(treeInstance, _pawnAttrTreeName, PawnAttrResourcePath);
            AddItems(treeInstance, _monsterAttrTreeName, MonsterAttrResourcePath);
            AddItems(treeInstance, _hitBoxAttrTreeName, HitBoxAttrResourcePath);
            AddItems(treeInstance, _bulletAttrTreeName, BulletAttrResourcePath);

            return treeInstance;
        }

        private void Save(Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        protected override void OnBeginDrawEditors()
        {
            //绘制顶部创建按钮
            var selected = this.MenuTree.Selection.FirstOrDefault();
            if (selected == null)
            {
                return;
            }

            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (selected.Value is AttrTemplateCreator)
                {
                    if (SirenixEditorGUI.ToolbarButton("新建模板"))
                    {
                        CreateTemplateWindow.Open(attrTemplates);
                    }
                }

                if (selected.Value is AttrTemplate template)
                {
                    if (SirenixEditorGUI.ToolbarButton("新建模板"))
                    {
                        CreateTemplateWindow.Open(attrTemplates);
                    }

                    if (SirenixEditorGUI.ToolbarButton("保存"))
                    {
                        Save(template);
                    }
                }


                if (selected.Value is ActorAttrRootView rootView)
                {
                    if (SirenixEditorGUI.ToolbarButton("新建属性"))
                    {
                        CreateActorAttrWindow.Open(attrTemplates);
                    }
                }

                if (selected.Value is AttrDefine attrDefine)
                {
                    if (SirenixEditorGUI.ToolbarButton("新建属性"))
                    {
                        CreateActorAttrWindow.Open(attrTemplates);
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}