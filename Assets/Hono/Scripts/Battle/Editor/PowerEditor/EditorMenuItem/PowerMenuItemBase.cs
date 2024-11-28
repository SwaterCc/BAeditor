using System;
using System.Collections.Generic;
using System.IO;
using Editor.AbilityEditor;
using Sirenix.OdinInspector.Editor;
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
    public abstract class PowerMenuItemBase : OdinMenuItem
    {
        public enum EMenuItemOperation
        {
            Create,
            CopyIn,
            MoveIn,
            Delete,
        }

        private readonly GenericMenu _rightMenu;
        private readonly Dictionary<EMenuItemOperation, bool> _rightMenuOperationFlags;
        private readonly Dictionary<EMenuItemOperation, GenericMenu.MenuFunction> _rightMenuOperationFunctions;
        private bool _allowMove;
        private bool _allowCopy;
        
        public string Path { get; set; }

        protected PowerMenuItemBase(OdinMenuTree tree, string name, object view) : base(tree, name, view)
        {
            _allowMove = true;
            _allowCopy = true;
            _rightMenu = new GenericMenu();
            OnRightClick += showMenu;
            _rightMenuOperationFlags = new Dictionary<EMenuItemOperation, bool>()
            {
                { EMenuItemOperation.Create, true },
                { EMenuItemOperation.CopyIn, true },
                { EMenuItemOperation.MoveIn, true },
                { EMenuItemOperation.Delete, true },
            };

            _rightMenuOperationFunctions = new Dictionary<EMenuItemOperation, GenericMenu.MenuFunction>()
            {
                { EMenuItemOperation.Create, CreateItem },
                { EMenuItemOperation.CopyIn, SelectionCopyToHere },
                { EMenuItemOperation.MoveIn, SelectionMoveToHere },
                { EMenuItemOperation.Delete, DeleteItem },
            };
        }

        protected void SetAllowMove(bool allow)
        {
            _allowMove = allow;
        }

        protected void SetAllowCopy(bool allow)
        {
            _allowCopy = allow;
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
            addRightMenuItem(EMenuItemOperation.Create, "新建");
            addRightMenuItem(EMenuItemOperation.CopyIn, "复制选中项到这里");
            addRightMenuItem(EMenuItemOperation.MoveIn, "移动选中项到这里");
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

        protected virtual void CreateItem() { }
        protected virtual void DeleteItem() { }

        protected virtual void SelectionCopyToHere()
        {
            /*List<AbilityEditorMenuItem> copyList = new List<AbilityEditorMenuItem>(MenuTree.Selection.Count);
            foreach (var item in MenuTree.Selection)
            {
                if (item is not AbilityEditorMenuItem aItem)
                {
                    continue;    
                }

                if (!aItem._allowCopy)
                {
                    continue;
                }
                
                copyList.Add(aItem);
            }*/
        }
        protected virtual void SelectionMoveToHere() { }
        

        public virtual void BuildTree() { }
    }

    public class AAbilityMenuItemBase : PowerMenuItemBase
    {
        public AAbilityMenuItemBase(OdinMenuTree tree, string name, AView view) : base(tree, name, view)
        {
            SetOperationAllow(EMenuItemOperation.Create, false);
            SetOperationAllow(EMenuItemOperation.CopyIn, false);
            SetOperationAllow(EMenuItemOperation.MoveIn, false);
        }
    }

    public class AFolderMenuItemBase : PowerMenuItemBase
    {
        private readonly List<string> _folders = new();
        private readonly List<string> _files = new();
        
        public AFolderMenuItemBase(OdinMenuTree tree, string name, string path) : base(tree, name, null)
        {
            Path = path;
            PowerEditorTools.GetPathAssetsAndFolders(path, ref _folders, ref _files);
        }

        protected virtual AView getViewDrawer()
        {
            return new AbilityView();;
        }

        public override void BuildTree()
        {
            foreach (var path in _folders)
            {
                string menuName = System.IO.Path.GetFileName(path);
                var folderMenuItem = new AFolderMenuItemBase(MenuTree, menuName, path);
                ChildMenuItems.Add(folderMenuItem);
                folderMenuItem.BuildTree();
            }

            foreach (var path in _files)
            {
                string menuName = System.IO.Path.GetFileName(path).Split(".")[0];
                var view = getViewDrawer();
                view.Load(path);
                var abilityMenuItem = new AAbilityMenuItemBase(MenuTree, menuName, view)
                {
                    Path = path
                };
                ChildMenuItems.Add(abilityMenuItem);
                abilityMenuItem.BuildTree();
            }
        }
    }

    public abstract class ARootMenuItemBase : AFolderMenuItemBase
    {
        protected ARootMenuItemBase(OdinMenuTree tree, string name, string path) : base(tree, name, path)
        {
            SetOperationAllow(EMenuItemOperation.Delete, false);
        }
    }
}