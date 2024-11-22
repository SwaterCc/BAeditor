#region

using Hono.Core;
using QTool;
using QTool.UI;
using TMPro;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.BattleUI {
	public class WarDeployPanel : QUI<WarDeployPanel> {
		private bool _isReadyViewShow;
		private int _beforeRp;
		public RoundInfoPanel RoundInfo;
		public TextMeshProUGUI RPCount;
		public CanvasGroup BuildTeamBtn;
		public CanvasGroup RoundBeginBtn;
		public QObjectList BuildingList;
		public QObjectList PawnRefreshPointList;

		private void Start() {
			//初始化建筑列表
			var buildingTable = ConfigManager.Table<BuildingLogicTable>().GetTable();
			foreach (var pBuiding in buildingTable) {
				BuildingList.Get().SetData(pBuiding.Value);
			}

			//初始化队伍刷新点列表
			var battleSceneRow = ConfigManager.Table<BattleSceneTable>()
				.Get(BattleManager.CurBattle.BattleGroundConfigId);
			for (int i = 0; i < battleSceneRow.TeamCount; i++) {
				PawnRefreshPointList.Get().SetData(i);
			}
		}

		public override void OnFresh() {
			_isReadyViewShow = true;
			BuildTeamBtn.alpha = 1;
			RoundBeginBtn.alpha = 1;
			RoundInfo.ShowPanel();
			int curRoundCount = BattleManager.CurBattle.RtInfo.CurRoundCount;
			int maxRoundCount = BattleManager.CurBattle.LevelData.RoundDatas.Count;
			RoundInfo.OnFresh((curRoundCount, maxRoundCount));
			_beforeRp = BattleManager.CurBattle.RtInfo.RPCount;
			RPCount.SetText(_beforeRp.ToString());
		}

		private void Update() {
			if (BattleManager.CurBattle?.RtInfo == null)
				return;
			if (_beforeRp != BattleManager.CurBattle.RtInfo.RPCount) {
				_beforeRp = BattleManager.CurBattle.RtInfo.RPCount;
				RPCount.SetText(_beforeRp.ToString());
			}

			_beforeRp = BattleManager.CurBattle.RtInfo.RPCount;
			RPCount.SetText(_beforeRp.ToString());
		}

		protected override void OnHide() {
			BuildTeamBtn.alpha = 0;
			RoundBeginBtn.alpha = 0;
			RoundInfo.HidePanel();
			_isReadyViewShow = false;
			base.OnHide();
		}

		public void OnBuildingTeamBtnClick() {
			TeamSelectPanel.Instance.Show();
		}

		public void OnRoundBeginBtnClick() {
			BattleManager.CurBattle.RoundBegin();
		}
	}
}