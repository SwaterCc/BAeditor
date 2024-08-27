using System.Collections.Generic;

namespace Battle
{
    public class AttrCollection
    {
        private Dictionary<EAttributeType, Attribute> _simpleAttrs = new(64);
        private Dictionary<EAttributeType, Attribute> _compositeAttrs = new(64);

        public Attribute GetAttr(EAttributeType attrType)
        {
            if (_simpleAttrs.TryGetValue(attrType, out var attr))
            {
                return attr;
            }
            else if (_compositeAttrs.TryGetValue(attrType, out attr))
            {
                return attr;
            }
            else
            {
                return null;
            }
        }

        public void AddAttr(EAttributeType attributeType, Attribute attr)
        {
            if (attr is CompositeAttribute)
            {
                _compositeAttrs.Add(attributeType, attr);
            }
            else
            {
                _simpleAttrs.Add(attributeType, attr);
            }
        }
    }
}