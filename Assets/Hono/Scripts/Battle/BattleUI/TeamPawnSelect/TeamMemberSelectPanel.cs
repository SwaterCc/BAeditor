#region

using Hono.Core;
using QTool;
using QTool.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class TeamMemberSelectPanel : QUI<TeamMemberSelectPanel>, IView<(int, Action<int, int>)> {
		private int _curPawnConfigId;
		private int _curTeamMemberIndex;
		private Action<int, int> _callback;

		public TextMeshProUGUI PawnName;
		public TextMeshProUGUI PawnDesc;

		public CanvasGroup PawnInfo;
		public CanvasGroup SkillInfo;

		public QObjectList PawnQObjetList;
		public List<SkillShowButton> SkillShowButtons;

		[NonSerialized]
		public int SkillSelect;
		private void Start() {
			PawnQObjetList.Clear();
			foreach (var pair in ConfigManager.Table<PawnLogicTable>().GetTable()) {
				PawnQObjetList.Get().SetData<(int, Action<int>)>((pair.Key, onPawnBtnClick));
			}
		}

		public void OnFresh((int, Action<int, int>) data) {
			_curTeamMemberIndex = data.Item1;
			_callback = data.Item2;
		}

		private void refreshDetailView() {
			if (_curPawnConfigId == 0) {
				PawnInfo.alpha = 0;
				SkillInfo.alpha = 0;
				return;
			}

			var pawnLogicRow = ConfigManager.Table<PawnLogicTable>().Get(_curPawnConfigId);
			refreshPawnInfo(pawnLogicRow);
			refreshSkillInfo(pawnLogicRow);
		}

		private void refreshPawnInfo(PawnLogicTable.PawnLogicRow pawnLogicRow) {
			PawnInfo.alpha = 1;
			PawnName.SetText(pawnLogicRow.Name);
			PawnDesc.SetText(pawnLogicRow.Desc);
		}

		private void refreshSkillInfo(PawnLogicTable.PawnLogicRow pawnLogicRow) {
			SkillInfo.alpha = 1;
			if (pawnLogicRow.ActorClassId > 0) {
				var mainClass = ConfigManager.Table<ActorClassTable>().Get(pawnLogicRow.ActorClassId);
				for (int i = 0; i < 4; i++) {
					if (mainClass.Skills.Count <= i) {
						continue;
					}

					var skillInfo = mainClass.Skills[i];
					if (skillInfo.Count <= 1) {
						continue;
					}

					SkillShowButtons[i].Init(i, skillInfo[0]);
					if (SkillSelect == i) {
						SkillShowButtons[i].UpdateDesc();
					}
				}
			}

			if (pawnLogicRow.ActorSubClassId > 0) {
				var subClass = ConfigManager.Table<ActorClassTable>().Get(pawnLogicRow.ActorSubClassId);
				for (int i = 4; i < 5; i++) {
					if (subClass.SubClassSkill.Count <= i - 4) {
						continue;
					}

					var skillInfo = subClass.SubClassSkill[i - 4];
					if (skillInfo.Count <= 1) {
						continue;
					}

					SkillShowButtons[i].Init(i, skillInfo[0]);
					if (SkillSelect == i) {
						SkillShowButtons[i].UpdateDesc();
					}
				}
			}
		}

		private void onPawnBtnClick(int configId) {
			if (configId == _curPawnConfigId) {
				return;
			}

			_curPawnConfigId = configId;
			refreshDetailView();
		}

		public void OnConfirm() {
			_callback.Invoke(_curTeamMemberIndex, _curPawnConfigId);
			Hide();
		}
	}
}