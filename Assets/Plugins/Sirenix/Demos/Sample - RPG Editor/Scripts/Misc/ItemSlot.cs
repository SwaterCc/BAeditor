using System;

#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    [Serializable]
    public struct ItemSlot
    {
        public int ItemCount;
        public Item Item;
    }
}
#endif