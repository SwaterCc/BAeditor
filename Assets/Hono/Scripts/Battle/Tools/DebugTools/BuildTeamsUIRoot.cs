using System;
using UnityEngine;

namespace Hono.Scripts.Battle.Tools
{
    public class BuildTeamsUIRoot : BattleUIHandle
    {
        public PawnTeamAllData AllData { get; set; }

        protected override IUIPassData returnPassData()
        {
            return AllData;
        }

        protected override void onActiveFalse()
        {
            
        }
    }
}