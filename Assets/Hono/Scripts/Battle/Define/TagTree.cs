using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	[Serializable]
	public class TagTreeItem {
		public int parent;
		public int tag;
		public List<TagTreeItem> children = new();
	}

	public class TagTreeRoot : ScriptableObject {
		[ReadOnly]
		public TagTreeItem root = new();
	}

	public static class TagTreeHelper {
		public static TagTreeItem FindTag(this TagTreeItem item, int tag) {
			if (item.tag == tag) {
				return item;
			}

			foreach (var child in item.children) {
				var result = FindTag(child, tag);
				if (result != null) {
					return result;
				}
			}

			return null;
		}

		public static bool TagIsUnique(this TagTreeItem item, int tag) {
			if (item.tag == tag) {
				return false;
			}
			else {
				foreach (var itemChild in item.children) {
					if (!TagIsUnique(itemChild, tag)) {
						return false;
					}
				}
			}

			return true;
		}
	}
}