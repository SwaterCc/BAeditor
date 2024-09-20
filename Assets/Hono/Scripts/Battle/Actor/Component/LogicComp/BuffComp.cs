using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorLogic {
		public class Buff {
			public int SourceId => _sourceId;
			public int Uid => _uid;
			public int ConfigId => _data.id;

			private int _uid;
			private int _sourceId;
			private ActorLogic _logic;
			private BuffData _data;
			private int _abilityUid;

			private int _layer;
			public int LayerCount => _layer;

			public Buff(int uid, ActorLogic logic, int sourceId, BuffData data) {
				_uid = uid;
				_logic = logic;
				_sourceId = sourceId;
				_data = data;
				_layer = data.InitLayer;
			}

			public void OnAdd() {
				_abilityUid = _logic._abilityController.AwardAbility(_data.id, true);
			}

			public void AddLayer(int layerCount) {
				_layer += layerCount;
			}

			public void OnRemove() {
				_logic._abilityController.RemoveAbility(_abilityUid);
			}
		}

		public class BuffComp : ALogicComponent {
			private CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
			private Dictionary<int, List<Buff>> _buffControls = new();
			private Dictionary<int, Buff> _buffs = new();
			public Dictionary<int, Buff> BUffs => _buffs;
			public BuffComp(ActorLogic logic) : base(logic) { }

			public override void Init() {
				foreach (var buff in ActorLogic.LogicData.OwnerBuffs) {
					AddBuff(Actor.Uid, buff);
				}
			}

			public void AddBuff(int sourceActorId, int buffConfigId, int buffLayer = 1) {
				var buffData = AssetManager.Instance.GetData<BuffData>(buffConfigId);

				if (buffData == null) {
					Debug.LogError($"Id {buffConfigId} BuffData is null");
					return;
				}

				bool createNewBuff = true;

				if (!_buffControls.TryGetValue(buffConfigId, out var buffList)) {
					//如果没找到同类型的buff，直接创建新的，结束
					buffList = new List<Buff>();
					_buffControls.Add(buffConfigId, buffList);
				}
				else {
					if (buffList.Count != 0) {
						createNewBuff = AddRule(buffData, sourceActorId, buffList, buffLayer);
					}
				}

				if (createNewBuff) {
					var buff = new Buff(_idGenerator.GenerateId(), ActorLogic, sourceActorId, buffData);
					buffList.Add(buff);
					buff.OnAdd();
					_buffs.Add(buff.Uid, buff);
				}
			}

			private bool AddRule(BuffData buffData, int sourceId, List<Buff> buffList, int layerCount) {
				switch (buffData.ReplaceRule) {
					case EBuffReplaceRule.SameSourceReplace: {
						//同源替换
						List<int> removes = new List<int>();
						foreach (var buff in buffList) {
							if (buff.SourceId == sourceId) {
								removes.Add(buff.Uid);
								break;
							}
						}

						foreach (var uid in removes) {
							RemoveByUid(uid);
						}

						return true;
					}
					case EBuffReplaceRule.SameSourceAdd: {
						//同源叠加
						foreach (var buff in buffList) {
							if (buff.SourceId == sourceId) {
								buff.AddLayer(layerCount);
								return false;
							}
						}

						return true;
					}
					case EBuffReplaceRule.Add: {
						//全叠加
						buffList[0].AddLayer(layerCount);
						return true;
					}
					case EBuffReplaceRule.OnlyOne: {
						//非同源替换
						if (buffList[0].SourceId != sourceId) {
							RemoveByUid(buffList[0].Uid);
							return true;
						}

						return false;
					}
				}

				Debug.LogError("不应该走到这里");
				return false;
			}

			public void RemoveByConfigId(int buffConfigId) {
				if (!_buffControls.TryGetValue(buffConfigId, out var buffList)) {
					return;
				}

				foreach (var buff in buffList) {
					buff.OnRemove();
					_buffs.Remove(buff.Uid);
				}

				_buffControls.Remove(buffConfigId);
			}

			public void RemoveByUid(int buffUid) {
				if (!_buffs.TryGetValue(buffUid, out var buff)) {
					return;
				}

				buff.OnRemove();
				_buffControls[buff.ConfigId].Remove(buff);
				_buffs.Remove(buff.Uid);
			}

			public int GetBuffLayer(int configId) {
				if (!_buffControls.TryGetValue(configId, out var buffList)) {
					return -1;
				}

				if(buffList.Count == 0) {
					return -1;
				}

				return buffList[0].LayerCount;
			}
		}
	}
}