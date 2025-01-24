#region

using System;

#endregion

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AbilityFunction : Attribute
    {
        public bool ShowInEditorView;
        public string GroupTitle;

        public AbilityFunction(bool showInEditorView = true, string groupTitle = "Other")
        {
            ShowInEditorView = showInEditorView;
            GroupTitle = groupTitle;
        }

        public AbilityFunction(string groupTitle)
        {
            GroupTitle = groupTitle;
            ShowInEditorView = true;
        }
    }
}