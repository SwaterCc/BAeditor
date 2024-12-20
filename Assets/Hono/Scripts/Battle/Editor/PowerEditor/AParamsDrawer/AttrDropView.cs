using System;
using Hono.Scripts.Battle;
using UnityEditor.IMGUI.Controls;

namespace Editor.AbilityEditor
{
    public class AttrDropdown : AdvancedDropdown
    {
        public AttrDropdown(AdvancedDropdownState state) : base(state) { }
        
        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("root");

            foreach (var name in Enum.GetNames(typeof(EAttrType)))
            {
                root.AddChild(new AdvancedDropdownItem(name));
            }
            
            return root;
        }
    }
}