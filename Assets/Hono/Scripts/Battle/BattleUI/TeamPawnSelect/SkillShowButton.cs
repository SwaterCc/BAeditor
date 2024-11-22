#region

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class SkillShowButton : MonoBehaviour {
		public Image Image;

		public TextMeshProUGUI SkillName;
		public TextMeshProUGUI SkillDesc;


		private string _skillName;
		private string _skillDesc;
		private int _idx;

		public void Init(int idx,int skillId) {
			var skillData = AssetManager.Instance.GetData<SkillData>(skillId);
			if (skillData == null) {
				return;
			}

			_idx = idx;
			_skillName = skillData.SkillName;
			_skillDesc = skillData.SkillDesc;

			if (!string.IsNullOrEmpty(skillData.SkillIconPath)) {
				var sprite = Resources.Load<Sprite>(skillData.SkillIconPath);
				if (sprite != null) {
					Image.sprite = sprite;
				}
			}
		}

		public void OnValueChange(bool flag) {
			if (!flag) return;
			TeamMemberSelectPanel.Instance.SkillSelect = _idx;
			UpdateDesc();
		}

		public void UpdateDesc() {
			SkillName.SetText(_skillName);
			SkillDesc.SetText(_skillDesc);
		}
	}
}