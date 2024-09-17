using System;

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AbilityMethod : Attribute
    {
        public bool ShowInEditorView; 
        public AbilityMethod(bool showInEditorView = true)
        {
            ShowInEditorView = showInEditorView;
        }
    }
}