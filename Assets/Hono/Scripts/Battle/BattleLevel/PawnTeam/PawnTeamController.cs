using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class PawnTeamController
    {
        private readonly List<PawnTeamState> _teamStates;
        private PawnTeamState _curControlTeam;
        private readonly BattleGround _battleGround;
        public bool PawnLoadFinish { get; private set; }
        private bool _pawnLoadFinishFirst = true;

        public PawnTeamController(BattleGround battleGround)
        {
            _battleGround = battleGround;
            _teamStates = new List<PawnTeamState>(BattleConstValue.TeamMaxCount);
        }

        public void BuildTeam(PawnTeamAllData teamAllData)
        {
            _teamStates.Clear();
            
            for (var index = 0; index < teamAllData.Teams.Count; index++)
            {
                var teamData = teamAllData.Teams[index];
                var teamState = new PawnTeamState(index, teamData.TeamMemberCount, teamData.Team);
                _teamStates.Add(teamState);
            }

            _curControlTeam = _teamStates[0];
        }
        
        public void CreatePawnTeams()
        {
            foreach (var teamState in _teamStates)
            {
                if (_battleGround.TryGetTeamPoint(teamState.TeamIndex, out var centerPos))
                {
                    teamState.CreateTeam(centerPos,Quaternion.identity);
                }
            }

            ChangeControlTeam(0);
        }

        public void ChangeControlTeam(int index)
        {
            _curControlTeam.OnPlayerControlChange(false);
            _curControlTeam = _teamStates[index];
            _curControlTeam.OnPlayerControlChange(true);
        }

        public void Tick(float dt)
        {
            foreach (var teamState in _teamStates)
            {
                teamState.OnTick(dt);
            }
        }
    }
}