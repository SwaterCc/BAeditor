#region

using Hono.Scripts.Battle.BattleUI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	public class PawnTeamController {
		private readonly List<PawnTeamState> _teamStates;
		private PawnTeamState _curControlTeam;
		private readonly BattleGround _battleGround;

		public int ControlUid => _curControlTeam?.LeaderUid ?? -1;

		public bool IsReady => _teamStates.Count > 0;

		public List<PawnTeamState> TeamStates => _teamStates;

		public int TeamCount { get; private set; }

		public PawnTeamState CurControlTeam => _curControlTeam;

		public PawnTeamController(BattleGround battleGround) {
			_battleGround = battleGround;
			_teamStates = new List<PawnTeamState>(BattleConstValue.TeamMaxCount);
		}

		public void BuildTeam(PawnTeamDataList teamDataList) {
			_teamStates.Clear();
			TeamCount = teamDataList.TeamCount;
			for (var index = 0; index < teamDataList.Teams.Count; index++) {
				var teamData = teamDataList.Teams[index];
				var teamState = new PawnTeamState(this, index, teamData.TeamMemberCount, teamData.Team);
				_teamStates.Add(teamState);
			}

			_curControlTeam = _teamStates[0];
		}

		public void CreatePawnTeams() {
			foreach (var teamState in _teamStates) {
				if (_battleGround.TryGetTeamPoint(teamState.TeamIndex, out var centerPos)) {
					teamState.CreateTeam(centerPos, Quaternion.identity);
				}
			}

			ChangeControlTeam(0);
		}

		public void RemoveTeam() {
			foreach (var teamState in _teamStates) {
				teamState.RemoveTeam();
			}
		}

		public void ChangeControlTeam(int index) {
			_curControlTeam.OnPlayerControlChange(false);
			_curControlTeam = _teamStates[index];
			_curControlTeam.OnPlayerControlChange(true);
			PlayerControlPanel.Instance.OnSwitchTeam();
		}

		public void Tick(float dt) {
			foreach (var teamState in _teamStates) {
				teamState.OnTick(dt);
			}

			if (_teamStates.Count <= 0) {
				return;
			}


			if (_curControlTeam != null && !_curControlTeam.HasAlive()) {
				foreach (var teamState in _teamStates) {
					if (teamState.HasAlive()) {
						ChangeControlTeam(teamState.TeamIndex);
						break;
					}
				}
			}
		}

		public bool CheckHasTeamAlive() {
			return _teamStates.Any(state => state.HasAlive());
		}
	}
}