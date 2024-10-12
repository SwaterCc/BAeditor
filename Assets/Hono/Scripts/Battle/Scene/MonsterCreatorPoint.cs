using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Map;
using Hono.Scripts.Battle.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle.Scene {
	[Serializable]
	public struct MonsterInfo {
		public int MonsterConfigId;
		public int MonsterCount;
	}

	[Serializable]
	public struct RoundMonsterInfos {
		public List<MonsterInfo> RoundInfos;
	}
	
	/// <summary>
	/// 刷怪点、矩形会被分割成1*1大小的格子，然后放入配置的怪物
	/// </summary>
	public class MonsterCreatorPoint : MonoBehaviour {
		[LabelText("关联触发器")] public List<MonsterCreatorTriggerBox> TriggerPointLink = new();
		
		[LabelText("所有波次怪物配置")] 
		public List<RoundMonsterInfos> AllRoundMonsterInfos = new();

		/// <summary>
		/// 当前点刷怪波次
		/// </summary>
		public int CurrentRound => _curRound;
		
		private int _curRound = -1;
		private int _width;
		private int _length;
		private int _split;

		private List<int> _currentSurvivalMonstersUid = new();

		private void onTriggerPointFired(in VarCollection variables) {
			
		}

		public void RefreshMonsterRound() {
			
		}
		
		private void createMonster(int configId, Vector3 pos, Quaternion rot) {
			
		}
	}
}