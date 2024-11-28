using System;
using System.Collections.Generic;
using System.IO;
using Editor.AbilityEditor;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class SkillRootMenu : ARootMenuItemBase
    {
        public SkillRootMenu(OdinMenuTree tree, string itemName) : base(tree, itemName,
            BattleEditorPath.SkillRootPath) { }

        protected override AView getViewDrawer()
        {
            return new SkillView();
        }

        protected override void CreateItem()
        {
            
        }
    }
}