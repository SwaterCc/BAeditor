using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		/// <summary>
		/// 用ConfigId索引,每个buff只会有一个
		/// </summary>
		public class BuffComp : ALogicComponent {
			private readonly Dictionary<int, Buff> _buffs = new();
			public Dictionary<int, Buff> Buffs => _buffs;
			
			private Func<IntArray> _getBuffs;

			public BuffComp(ActorLogic logic, Func<IntArray> getBuffs) : base(logic) {
				_getBuffs = getBuffs;
			}
			
			public override void Init() {
				if(_getBuffs == null) return;
				foreach (var buff in _getBuffs.Invoke()) {
					AddBuff(Actor.Uid, buff);
				}
			}

			public void AddBuff(int sourceActorId, int buffConfigId, int buffLayer = 1) {
				var buffData = AssetManager.Instance.GetData<BuffData>(buffConfigId);

				if (buffData == null) {
					Debug.LogError($"Id {buffConfigId} BuffData is null");
					return;
				}

				if (!_buffs.TryGetValue(buffConfigId,out var buff)) {
					buff = new Buff(ActorLogic, sourceActorId, buffData);
					buff.OnAdd();
					_buffs.Add(buff.ConfigId, buff);
				}
				else {
					if (CheckReplace(buff,buffData,sourceActorId)) {
						buff.OnRemove();
						buff = new Buff(ActorLogic, sourceActorId, buffData);
						buff.OnAdd();
						_buffs[buffConfigId] = buff;
					}
					else {
						buff.AddLayer(buffLayer);
					}
				}
			}

			private bool CheckReplace(Buff oldBuff, BuffData newBuffData, int sourceId) {
				switch (newBuffData.ReplaceRule) {
					case EBuffReplaceRule.SameSourceReplace: {
						//同源替换
						return oldBuff.SourceId == sourceId;
					}
					case EBuffReplaceRule.SameSourceAdd: {
						//非同源替换
						return oldBuff.SourceId != sourceId;
					}
					case EBuffReplaceRule.Add: {
						//不替换
						return false;
					}
					case EBuffReplaceRule.OnlyOne: {
						//全替换
						return true;
					}
				}

				Debug.LogError("不应该走到这里");
				return false;
			}

			public void RemoveBuff(int buffConfigId) {
				if (_buffs.TryGetValue(buffConfigId,out var buff)) {
					buff.OnRemove();
					_buffs.Remove(buffConfigId);
				}
			}

			public int GetBuffLayer(int configId) {
				if (!_buffs.TryGetValue(configId, out var buff)) {
					return -1;
				}

				return buff.LayerCount;
			}

			public int GetBuffSource(int configId) {
				if (!_buffs.TryGetValue(configId, out var buff)) {
					return -1;
				}

				return buff.SourceId;
			}
		}
	}
}