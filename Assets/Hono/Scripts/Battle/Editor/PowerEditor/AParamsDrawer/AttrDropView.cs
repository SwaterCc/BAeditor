using System;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.Base;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AttrDropdownItem : AdvancedDropdownItem
    {
        public EAttrType AttrType;
        public AttrDropdownItem(EAttrType attrType) : base(attrType.ToString())
        {
            AttrType = attrType;
        }
    }
    
    public class AttrDropdown : AdvancedDropdown
    {
        private readonly AParams _aParams;

        public AttrDropdown(AParams aParams) : base(new AdvancedDropdownState())
        {
            _aParams = aParams;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("属性列表");

            foreach (var attrType in Enum.GetValues(typeof(EAttrType)))
            {
                root.AddChild(new AttrDropdownItem((EAttrType)attrType));
            }

            return root;
        }
        
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            _aParams.attrType = ((AttrDropdownItem)item).AttrType;
        }
    }
}