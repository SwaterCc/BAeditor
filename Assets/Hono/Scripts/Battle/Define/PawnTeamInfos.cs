using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class PawnTeamData
    {
        [SerializeField] public List<int> Team = new() { -1, -1, -1, -1 };

        public int TeamMemberCount
        {
            get { return Team.Count(id => id > 0); }
        }
    }

    [Serializable]
    public class PawnTeamAllData : IUIPassData
    {
        public List<PawnTeamData> Teams = new();
    }
}