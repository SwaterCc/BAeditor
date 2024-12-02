using System;
using System.Collections.Generic;
using System.IO;
using Editor.AbilityEditor;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle.Editor.PowerEditor.SimpleWindow;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Hono.Scripts.Battle.Editor
{
    /// <summary>
    /// Ability菜单项
    /// 有两种实现，第一种是路径项，第二种是文件项
    /// 路径项右键实现添加文件，添加路径，删除文件夹
    /// 文件项实现右键复制，移动，删除
    /// </summary>
    public abstract class PowerEditorMenuItemBase : OdinMenuItem
    {
        public enum EMenuItemOperation
        {
            CreateFolder,
            CreateFileItem,
            CopyIn,
            MoveIn,
            Delete,
        }

        private readonly GenericMenu _rightMenu;
        private readonly Dictionary<EMenuItemOperation, bool> _rightMenuOperationFlags;
        private readonly Dictionary<EMenuItemOperation, GenericMenu.MenuFunction> _rightMenuOperationFunctions;

        public string Path { get; }

        public PMenuRootItem Root { get; set; }

        protected PowerEditorMenuItemBase(OdinMenuTree tree, string name, string path, object view) : base(
            tree, name, view)
        {
            Path = path;
            _rightMenu = new GenericMenu();
            OnRightClick += showMenu;
            _rightMenuOperationFlags = new Dictionary<EMenuItemOperation, bool>()
            {
                { EMenuItemOperation.CreateFolder, true },
                { EMenuItemOperation.CreateFileItem, true },
                { EMenuItemOperation.CopyIn, true },
                { EMenuItemOperation.MoveIn, true },
                { EMenuItemOperation.Delete, true },
            };

            _rightMenuOperationFunctions = new Dictionary<EMenuItemOperation, GenericMenu.MenuFunction>()
            {
                { EMenuItemOperation.CreateFolder, CreateFolder },
                { EMenuItemOperation.CreateFileItem, CreateFileItem },
                { EMenuItemOperation.CopyIn, SelectionCopyToHere },
                { EMenuItemOperation.MoveIn, SelectionMoveToHere },
                { EMenuItemOperation.Delete, DeleteItem },
            };
        }

        protected void SetOperationAllow(EMenuItemOperation operation, bool flag)
        {
            _rightMenuOperationFlags[operation] = flag;
        }

        /// <summary>
        /// 构建菜单
        /// </summary>
        private void showMenu(OdinMenuItem item)
        {
            addRightMenuItem(EMenuItemOperation.CreateFileItem, "新建Power");
            addRightMenuItem(EMenuItemOperation.CreateFolder,   "新建文件夹");
            addRightMenuItem(EMenuItemOperation.CopyIn,         "复制选中项到这里");
            addRightMenuItem(EMenuItemOperation.MoveIn,         "移动选中项到这里");
            _rightMenu.AddDisabledItem(new GUIContent("-------------------"));
            addRightMenuItem(EMenuItemOperation.Delete, "删除选中项");
            _rightMenu.ShowAsContext();
        }

        private void addRightMenuItem(EMenuItemOperation operation, string label)
        {
            var allowFlag = _rightMenuOperationFlags[operation];
            if (allowFlag)
                _rightMenu.AddItem(new GUIContent(label), false, _rightMenuOperationFunctions[operation]);
            else
                _rightMenu.AddDisabledItem(new GUIContent(label));
        }

        protected virtual void CreateFileItem()
        {
            CreateFileWindow.OpenWindow(this);
        }

        protected virtual void CreateFolder()
        {
            CreateFolderWindow.OpenWindow(this);
        }

        protected virtual void DeleteItem()
        {
            DeleteItemWindow.OpenWindow(this);
        }

        protected virtual void SelectionCopyToHere()
        {
            List<PowerEditorMenuItemBase> copyList = new List<PowerEditorMenuItemBase>(MenuTree.Selection.Count);
            foreach (var item in MenuTree.Selection)
            {
                if (item is not PowerEditorMenuItemBase powerMenuItemBase)
                {
                    continue;
                }

                powerMenuItemBase.CopyTo(this);
            }

            MenuTree.UpdateMenuTree();
        }

        public abstract void CopyTo(PowerEditorMenuItemBase parent);

        protected virtual void SelectionMoveToHere()
        {
            List<PowerEditorMenuItemBase> copyList = new List<PowerEditorMenuItemBase>(MenuTree.Selection.Count);
            foreach (var item in MenuTree.Selection)
            {
                if (item is not PowerEditorMenuItemBase powerMenuItemBase)
                {
                    continue;
                }

                powerMenuItemBase.MoveTo(this);
            }

            MenuTree.UpdateMenuTree();
        }

        public abstract void MoveTo(PowerEditorMenuItemBase parent);
    }

    public abstract class PMenuRootItem : PowerEditorMenuItemBase
    {
        private readonly List<string> _folders = new();
        private readonly List<string> _files = new();

        protected PMenuRootItem(OdinMenuTree tree, string name, string path) : base(tree, name, path, null)
        {
            Root = this;
            SdfIcon = SdfIconType.Hammer;
            Style = new OdinMenuStyle()
            {
                Height = 28,
                Offset = 20.00f,
                IndentAmount = 18.00f,
                IconSize = 16.00f,
                IconOffset = 0.00f,
                NotSelectedIconAlpha = 0.85f,
                IconPadding = 0.00f,
                TriangleSize = 16.00f,
                TrianglePadding = 0.00f,
                AlignTriangleLeft = true,
                Borders = true,
                BorderPadding = 4.80f,
                BorderAlpha = 0.62f,
                SelectedColorDarkSkin = new Color(0.689f,  0.210f, 0.172f, 1.000f),
                SelectedColorLightSkin = new Color(0.243f, 0.490f, 0.900f, 1.000f)
            };
            PowerEditorTools.GetPathAssetsAndFolders(path, ref _folders, ref _files);
            SetOperationAllow(EMenuItemOperation.Delete, false);
        }

        public abstract AView GetViewDrawer();

        public void BuildTree()
        {
            foreach (var path in _folders)
            {
                string menuName = System.IO.Path.GetFileName(path);
                var folderMenuItem = new PFolderMenuItem(MenuTree, menuName, path);
                folderMenuItem.Root = this;
                ChildMenuItems.Add(folderMenuItem);
                folderMenuItem.BuildTree();
            }

            foreach (var path in _files)
            {
                string menuName = System.IO.Path.GetFileName(path).Split(".")[0];
                var view = GetViewDrawer();
                view.Load(path);
                var abilityMenuItem = new PowerDataMenuItem(MenuTree, menuName, path, view);
                abilityMenuItem.Root = this;
                ChildMenuItems.Add(abilityMenuItem);
            }
        }

        /// <summary>
        /// 禁止拷贝
        /// </summary>
        /// <param name="parent"></param>
        public sealed override void CopyTo(PowerEditorMenuItemBase parent) { }

        /// <summary>
        /// 禁止移动
        /// </summary>
        /// <param name="parent"></param>
        public sealed override void MoveTo(PowerEditorMenuItemBase parent) { }
    }

    public class PFolderMenuItem : PowerEditorMenuItemBase
    {
        private readonly List<string> _folders = new();
        private readonly List<string> _files = new();

        public PFolderMenuItem(OdinMenuTree tree, string name, string path) : base(tree, name, path, null)
        {
            PowerEditorTools.GetPathAssetsAndFolders(path, ref _folders, ref _files);
            SdfIcon = SdfIconType.Folder;
            Style = new OdinMenuStyle()
            {
                Height = 28,
                Offset = 20.00f,
                IndentAmount = 18.00f,
                IconSize = 16.00f,
                IconOffset = 0.00f,
                NotSelectedIconAlpha = 0.85f,
                IconPadding = 0.00f,
                TriangleSize = 16.00f,
                TrianglePadding = 0.00f,
                AlignTriangleLeft = true,
                Borders = true,
                BorderPadding = 4.80f,
                BorderAlpha = 0.62f,
                SelectedColorDarkSkin = new Color(0.189f,  0.610f, 0.572f, 1.000f),
                SelectedColorLightSkin = new Color(0.243f, 0.490f, 0.900f, 1.000f)
            };
        }

        public void BuildTree()
        {
            foreach (var path in _folders)
            {
                string menuName = System.IO.Path.GetFileName(path);
                var folderMenuItem = new PFolderMenuItem(MenuTree, menuName, path);
                folderMenuItem.Root = Root;
                ChildMenuItems.Add(folderMenuItem);
                folderMenuItem.BuildTree();
            }

            foreach (var path in _files)
            {
                string menuName = System.IO.Path.GetFileName(path).Split(".")[0];
                var view = Root.GetViewDrawer();
                view.Load(path);
                var abilityMenuItem = new PowerDataMenuItem(MenuTree, menuName, path, view);
                abilityMenuItem.Root = Root;
                ChildMenuItems.Add(abilityMenuItem);
            }
        }

        public override void CopyTo(PowerEditorMenuItemBase parent)
        {
            //新建一个文件夹
            string newPath = parent.Path + "/" + Name;
            if (!Directory.Exists(newPath))
            {
                AssetDatabase.CreateFolder(parent.Path, Name);
            }

            foreach (var item in ChildMenuItems)
            {
                if (item is not PowerEditorMenuItemBase powerMenuItemBase)
                {
                    continue;
                }

                powerMenuItemBase.CopyTo(this);
            }
        }

        public override void MoveTo(PowerEditorMenuItemBase parent)
        {
            string newPath = parent.Path + "/" + Name;
            if (!Directory.Exists(newPath))
            {
                AssetDatabase.CreateFolder(parent.Path, Name);
            }

            foreach (var item in ChildMenuItems)
            {
                if (item is not PowerEditorMenuItemBase powerMenuItemBase)
                {
                    continue;
                }

                powerMenuItemBase.MoveTo(this);
            }

            AssetDatabase.DeleteAsset(Path);
        }
    }

    public class PowerDataMenuItem : PowerEditorMenuItemBase
    {
        public PowerDataMenuItem(OdinMenuTree tree, string name, string path, AView view) : base(tree, name, path, view)
        {
            SetOperationAllow(EMenuItemOperation.CreateFileItem, false);
            SetOperationAllow(EMenuItemOperation.CreateFolder,   false);
            SetOperationAllow(EMenuItemOperation.CopyIn,         false);
            SetOperationAllow(EMenuItemOperation.MoveIn,         false);
        }

        public override void CopyTo(PowerEditorMenuItemBase parent)
        {
            AssetDatabase.CopyAsset(Path, parent.Path + "/" + Name + ".asset");
        }

        public override void MoveTo(PowerEditorMenuItemBase parent)
        {
            AssetDatabase.MoveAsset(Path, parent.Path + "/" + Name + ".asset");
        }
    }
}