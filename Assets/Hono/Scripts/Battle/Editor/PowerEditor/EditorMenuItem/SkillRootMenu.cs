using System;
using System.Collections.Generic;
using System.IO;
using Editor.AbilityEditor;
using Editor.AbilityEditor.SimpleWindow;
using Hono.Scripts.Battle.Editor.PowerEditor.SimpleWindow;
using Sirenix.OdinInspector.Editor;

namespace Hono.Scripts.Battle.Editor
{
    public class SkillRootMenu : ARootMenuItem
    {
        public SkillRootMenu(OdinMenuTree tree, string itemName) : base(tree, itemName,
                                                                        BattleEditorPath.SkillRootPath) { }

        public override AView GetViewDrawer()
        {
            return new SkillView();
        }
    }
}