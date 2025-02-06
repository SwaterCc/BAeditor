using System;
using Hono.Scripts.Battle;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Core;
using UnityEditor.IMGUI.Controls;

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
        private Action<EAttrType> _callback;

        public AttrDropdown(Action<EAttrType> onSelect) : base(new AdvancedDropdownState())
        {
            _callback = onSelect;
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
            var attrType = ((AttrDropdownItem)item).AttrType;
            _callback?.Invoke(attrType);
        }
    }

    public class AParamAttrDropdown : AdvancedDropdown
    {
        private readonly AParams _aParams;

        public AParamAttrDropdown(AParams aParams) : base(new AdvancedDropdownState())
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