#region

using System;

#endregion

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityFunction : Attribute
    {
        public bool ShowInEditorView;

        public AbilityFunction(bool showInEditorView = true)
        {
            ShowInEditorView = showInEditorView;
        }
    }
}