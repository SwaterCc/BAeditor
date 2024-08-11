using Battle.Auto;

namespace Battle
{
    public static class AttributeHelper
    {
        public static Attribute GetAttr(this Actor actor,EAttributeType type)
        {
            return actor.GetAttrs().GetAttr(type);;
        }
    }
}