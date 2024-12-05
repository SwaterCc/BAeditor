using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Editor.BattleEditor.TagEditor {
	[Serializable]
	public class TagInfo {
		public string name;
		public string desc;
	}

	public class TagEditorInfos : SerializedScriptableObject {
		public Dictionary<int, TagInfo> Infos = new();
	}
}