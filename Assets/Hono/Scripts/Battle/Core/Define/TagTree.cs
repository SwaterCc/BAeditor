using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class TagTreeItem
    {
        public TagTreeItem parent;
        public int tag;
        public List<TagTreeItem> children = new();
    }

    public class TagTreeRoot : ScriptableObject
    {
        [ReadOnly]
        public TagTreeItem root = new();
        [LabelText("快速索引")]
        public Dictionary<int, TagTreeItem> searchDict = new();
    }

    public static class TagTreeHelper
    {
        public static bool HasParent(int parentTag, int tag)
        {
            var treeItem = BattleManager.TagTree.root.FindTag(tag);

            if (treeItem == null)
            {
                return false;
            }

            var parent = treeItem.parent;
            while (parent != null)
            {
                if (parent.tag == parentTag)
                {
                    return true;
                }

                parent = parent.parent;
            }

            return false;
        }

        public static TagTreeItem FindTag(this TagTreeItem item, int tag)
        {
            if (item.tag == tag)
            {
                return item;
            }

            foreach (var child in item.children)
            {
                var result = FindTag(child, tag);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static bool TagIsUnique(this TagTreeItem item, int tag)
        {
            if (item.tag == tag)
            {
                return false;
            }
            else
            {
                foreach (var itemChild in item.children)
                {
                    if (!TagIsUnique(itemChild, tag))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}