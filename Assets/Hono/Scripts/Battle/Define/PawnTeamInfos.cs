#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class PawnTeamData
    {
        [SerializeField] public List<int> Team = new() { -1, -1, -1, -1 };

        public int TeamMemberCount => Team.Count(id => id > 0);
    }

    [Serializable]
    public class PawnTeamDataList : IUIPassToLogicData
    {
        public List<PawnTeamData> Teams = new();

        public int TeamCount => Teams.Count(team => team.TeamMemberCount > 0);
    }
}