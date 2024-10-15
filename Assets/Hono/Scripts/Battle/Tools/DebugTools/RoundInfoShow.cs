using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            var curBattleGround = BattleManager.CurrentBattleGround;
            if (curBattleGround == null) return;

            var roundCount = curBattleGround.RuntimeInfo.CurRoundCount;
            var roundState = curBattleGround.RuntimeInfo.CurRoundState;
            var lastTime = curBattleGround.RuntimeInfo.CurRoundLastTime;
            var curPawnCount = curBattleGround.RuntimeInfo.GetRoundSurvivalFaction(1);
            var curMonsterCount = curBattleGround.RuntimeInfo.GetRoundSurvivalFaction(3);
            var pawnKilledCount =  curBattleGround.RuntimeInfo.GetRoundPawnKill(0);
            var deadMonster = curBattleGround.RuntimeInfo.GetBattleDeadFaction(3);
            
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