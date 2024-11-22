#region

using Hono.Core;
using QTool;
using QTool.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class TeamSelectPanel : QUI<TeamSelectPanel> {
		public static List<List<int>> teamDataListCache = null;
		public QObjectList TeamButtonList;
		public List<MemberSelectMainButton> SelectMainButtons = new();
		private PawnTeamDataList _teamDataList = new();
		private int _curSelectTeam = 0;
		private int _sceneId = -1;

		public void FirstOpen() {
			Show();
			_curSelectTeam = 0;

			var currentSceneId = BattleManager.CurBattle.BattleGroundConfigId;

			if (_sceneId != currentSceneId) {
				_sceneId = currentSceneId;
				_teamDataList.Teams.Clear();
				var sceneRow = ConfigManager.Table<BattleSceneTable>().Get(currentSceneId);
				for (int i = 0; i < sceneRow.TeamCount; i++) {
					TeamButtonList.Get().SetData<(int, Action<int>)>((i, OnTeamButton));
					_teamDataList.Teams.Add(new PawnTeamData());
					if (teamDataListCache != null && teamDataListCache.Count > i) {
						if (teamDataListCache[i] != null) {
							_teamDataList.Teams[i].Team = teamDataListCache[i];
						}
					}
				}
			}

			refreshMemberBtnData();
			LoadingPanel.HidePanel();
		}

		public void OnMemberBtnClick(int idx) {
			TeamMemberSelectPanel.Instance.SetData((idx, updateTeamMember));
			TeamMemberSelectPanel.Instance.Show();
		}

		private void updateTeamMember(int teamIndex, int configId) {
			_teamDataList.Teams[_curSelectTeam].Team[teamIndex] = configId;
			refreshMemberBtnData();
		}

		public void OnTeamButton(int idx) {
			if (idx == _curSelectTeam) return;
			_curSelectTeam = idx;
			refreshMemberBtnData();
		}

		private void refreshMemberBtnData() {
			for (int i = 0; i < BattleConstValue.TeamMemberMaxCount; i++) {
				int configId = _teamDataList.Teams[_curSelectTeam].Team[i];
				var mainButton = SelectMainButtons[i];
				mainButton.SetPawnConfigId(configId);
			}
		}

		public void OnConfirm() {
			if (_teamDataList.TeamCount <= 0) {
				Debug.Log("队伍为空");
				return;
			}

			BattleManager.CurBattle.BuildTeam(_teamDataList);
			if (BattleManager.CurBattle.LevelData.ReadyRoundStateSpik) {
				BattleManager.CurBattle.RoundBegin();
			}

			Hide();
		}

		public void Close() {
			Hide();
		}
	}
}