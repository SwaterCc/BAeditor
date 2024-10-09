using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Scene {
	/// <summary>
	/// 玩家出生点
	/// </summary>
	[Serializable]
	public class PawnStartPoints {
		
		[LabelText("队伍1出生点")]
		public Transform GroupStart0;
		[LabelText("队伍2出生点")]
		public Transform GroupStart1;
		[LabelText("队伍3出生点")]
		public Transform GroupStart2;
		[LabelText("队伍4出生点")]
		public Transform GroupStart3;

		public Transform this[int idx]
		{
			get
			{
				idx = Mathf.Clamp(idx, 0, BattleConstValue.PawnGroupMaxCount-1);
				switch (idx)
				{
					case 0:
						return GroupStart0;
					case 1:
						return GroupStart1;
					case 2:
						return GroupStart2;
					case 3:
						return GroupStart3;
				}

				return null;
			}
		}
	}
}