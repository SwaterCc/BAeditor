#region

using TMPro;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.Tools.DebugTools
{
    public class RoundInfoShow : MonoBehaviour
    {
        public TextMeshProUGUI RoundCount;
        public TextMeshProUGUI LastTime;
        public TextMeshProUGUI RoundState;

        public TextMeshProUGUI CurPawnCount;
        public TextMeshProUGUI CurMonsterCount;
        public TextMeshProUGUI PawnKillCount;
        public TextMeshProUGUI DeadMonster;

        public void Update()
        {
            var curBattleGround = BattleManager.CurBattle;
            if (curBattleGround == null) return;

            var roundCount = curBattleGround.RtInfo.CurRoundCount;
            var roundState = curBattleGround.RtInfo.CurRoundState;
            var lastTime = curBattleGround.RtInfo.CurRoundDurationTime;
            var curPawnCount = curBattleGround.RtInfo.GetRoundSurvivalFaction(1);
            var curMonsterCount = curBattleGround.RtInfo.GetRoundSurvivalFaction(3);
            var pawnKilledCount = curBattleGround.RtInfo.GetRoundPawnKill(0);
            var deadMonster = curBattleGround.RtInfo.GetBattleDeadFaction(3);

            RoundCount.SetText("RoundCount:" + (roundCount + 1));
            LastTime.SetText("LastTime:" + lastTime);
            RoundState.SetText(roundState.ToString());
            CurPawnCount.SetText("curPawnCount:" + curPawnCount);
            CurMonsterCount.SetText("curMonsterCount:" + curMonsterCount);
            PawnKillCount.SetText("pawnKilledCount:" + pawnKilledCount);
            DeadMonster.SetText("deadMonster:" + deadMonster);
        }

        public void ExitBattleClick()
        {
            BattleManager.Instance.ExitBattle();
        }
    }
}