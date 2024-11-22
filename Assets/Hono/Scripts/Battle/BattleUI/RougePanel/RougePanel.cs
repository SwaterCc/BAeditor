using Hono.Core;
using QTool;
using QTool.UI;
using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle.BattleUI.RougePanel {
	public class RougePanel : QUI<RougePanel> {
		public QObjectList CardList;
		[NonSerialized]
		public int PickUpUid;
		public override void OnFresh() {
			var cardSettings = BattleManager.CurBattle.LootController.RougeCardList;
			CardList.Clear();
			if (cardSettings.Count == 0 || PickUpUid <= 0) return;
			foreach (var setting in cardSettings) {
				CardList.Get().SetData((PickUpUid, setting));
			}
		}

		protected override void OnHide() {
			//ActorManager.Instance.TimeScale = 1;
			base.OnHide();
		}
	}
}