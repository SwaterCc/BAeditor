using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace Editor.AbilityEditor
{

    public class VariableDropViewItem : AdvancedDropdownItem
    {
        
    }
    
    public class VariableDropView : AdvancedDropdown
    {
        private AParamFiledFilter _filer;
        
        public VariableDropView(string name, List<Type> filters = null, bool reelection = false) : base(new AdvancedDropdownState())
        {
            _filer = new AParamFiledFilter();
        }
        
        protected override AdvancedDropdownItem BuildRoot()
        {
            
        }
    }
}