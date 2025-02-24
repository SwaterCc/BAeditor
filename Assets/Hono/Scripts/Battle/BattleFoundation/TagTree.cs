using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.BattleFoundation {
	public sealed class Tag {
		private TagTree _tree;
		public int Id { get; set; }
		public string TagName { get; set; }
		public string TagString { get; set; }
		public int TagType { get; set; }
		public int ParentTagId { get; set; }

		public List<Tag> Children { get; set; } = new();

		public Tag(TagTree tree) {
			_tree = tree;
		}

		public void AddChild(Tag child) {
			Children.Add(child);
		}

		public void RemoveChild(Tag child) {
			Children.Remove(child);
		}

		public void Display(int depth = 0) {
			Debug.Log(new string('-', depth * 2) + $"ID: {Id}, Name: {TagName}, Type: {TagType}");
			foreach (var child in Children) {
				child.Display(depth + 1);
			}
		}

		/// <summary>
		/// 检查当前节点或其子节点是否包含指定的 TagId。
		/// </summary>
		/// <param name="tagId">要查找的 TagId。</param>
		/// <param name="strict">如果为 true，则仅检查当前节点；如果为 false，则检查当前节点及其所有子节点。</param>
		/// <returns>如果找到指定的 TagId，则返回 true；否则返回 false。</returns>
		public bool ContainsTag(int tagId, bool strict = false) {
			if (strict) {
				return Id == tagId;
			}

			if (Id == tagId) {
				return true;
			}

			foreach (var child in Children) {
				if (child.ContainsTag(tagId)) {
					return true;
				}
			}

			return false;
		}
	}

	public class TagTree {
		/// <summary>
		/// 所有Tag快速索引
		/// </summary>
		private Dictionary<int, Tag> _tagsLookup = new();
		/// <summary>
		/// 隐藏根节点
		/// </summary>
		private Tag _root;

		public TagTree() {
			_root = new Tag(this);
		}

		public void BuildTree() {
			var tags = new List<Tag>();
			foreach (var tagRow in ConfigDataBase.Table<TagTable>().GetAllRows()) {
				tags.Add(new Tag(this) {
					Id = tagRow.Id,
					ParentTagId = tagRow.ParentTag,
					TagName = tagRow.TagName,
					TagString = tagRow.TagString,
					TagType = tagRow.TagType
				});
			}

			// 创建一个字典以便快速查找
			foreach (var tag in tags) {
				_tagsLookup[tag.Id] = tag;
			}

			// 遍历所有标签，找到它们的父节点并添加为子节点
			foreach (var tag in tags) {
				if (tag.ParentTagId == 0) {
					// 如果没有父节点，则它是根节点
					_root.AddChild(tag);
				}
				else {
					// 否则找到父节点并添加为子节点
					if (_tagsLookup.ContainsKey(tag.ParentTagId)) {
						_tagsLookup[tag.ParentTagId].AddChild(tag);
					}
				}
			}
		}

		/// <summary>
		/// 查找某个子类 Tag 是否具有指定的父类 Tag。
		/// </summary>
		/// <param name="childTagId">子类 Tag 的 Id。</param>
		/// <param name="parentTagId">要查找的父类 Tag 的 Id。</param>
		/// <returns>如果找到指定的父类 Tag，则返回 true；否则返回 false。</returns>
		public bool HasParentTag(int childTagId, int parentTagId) {
			if (!_tagsLookup.TryGetValue(childTagId,out var child)) {
				return false;
			}

			if (!_tagsLookup.TryGetValue(child.ParentTagId, out var parent)) {
				return false;
			}

			while (parent != null) {
				
				if (parent.Id == parentTagId) {
					return true;
				}
				if (!_tagsLookup.TryGetValue(parent.ParentTagId, out parent)) {
					return false;
				}
			}

			return false;
		}

		/// <summary>
		/// 检查当前节点或其子节点是否包含指定的 TagId。
		/// </summary>
		/// <param name="tagId">要查找的 TagId。</param>
		/// <param name="strict">如果为 true，则仅检查当前节点；如果为 false，则检查当前节点及其所有子节点。</param>
		/// <returns>如果找到指定的 TagId，则返回 true；否则返回 false。</returns>
		public bool ContainsTag(int tagId, bool strict = false) {
			if (!_tagsLookup.ContainsKey(tagId)) {
				return false;
			}

			var tag = _tagsLookup[tagId];
			return tag.ContainsTag(tagId, strict);
		}
	}
}