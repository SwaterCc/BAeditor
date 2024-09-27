using System;
using System.Collections;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using System.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class ActorManager {
		private Filter _filter;

		public List<int> UseFilter(Actor target, FilterSetting setting) {
			if (setting == null) {
				Debug.LogError("Setting is Empty!");
				return null;
			}

			_filter.SettingChange(target, setting);
			return _filter.GetResults();
		}

		private class Filter {
			private FilterSetting _filterSetting;
			private Actor _filterUser;
			private readonly ActorManager _actorManager;

			internal Filter(ActorManager actorManager) {
				_actorManager = actorManager;
			}

			public void SettingChange(Actor filterUser, FilterSetting setting) {
				_filterSetting = setting;
				_filterUser = filterUser;
			}

			private bool rangeCheck(Actor actor, FilterRange range) {
				bool checkResult = false;
				switch (range.RangeType) {
					case EFilterRangeType.ActorType:
						checkResult = (int)actor.ActorType == range.Value;
						break;
					case EFilterRangeType.Tag:
						checkResult = actor.HasTag(range.Value);
						break;
					case EFilterRangeType.ActorState:
						checkResult = actor.Logic.CurState() == (EActorState)range.Value;
						break;
					case EFilterRangeType.AbilityID:
						checkResult = actor.HasAbility(range.Value);
						break;
					case EFilterRangeType.Faction:
						var f1 = _filterUser.GetAttr<int>(ELogicAttr.AttrFaction);
						var f2 = actor.GetAttr<int>(ELogicAttr.AttrFaction);
						checkResult = LuaInterface.GetFaction(f1, f2) == range.Value;
						break;
					default:
						Debug.LogError($"使用了未实现的范围筛选 settingId {_filterUser.Uid} type {range.RangeType}");
						return false;
				}

				if (range.IsReverse)
					checkResult = !checkResult;

				return checkResult;
			}

			private bool getCompareRes(ECompareResType compareResType, int flag) {
				switch (compareResType) {
					case ECompareResType.Less:
						return flag < 0;
					case ECompareResType.LessAndEqual:
						return flag <= 0;
					case ECompareResType.Equal:
						return flag == 0;
					case ECompareResType.More:
						return flag > 0;
					case ECompareResType.MoreAndEqual:
						return flag >= 0;
				}

				return true;
			}

			public List<int> GetResults() {
				if (_filterSetting == null) {
					Debug.LogError("筛选器设置为空");
					return null;
				}

				List<int> actorUids = new();
				List<Actor> actors = new();
				if (_filterSetting.OpenBoxCheck) {
					var pos = _filterUser.GetAttr<Vector3>(ELogicAttr.AttrPosition);
					var rot = _filterUser.GetAttr<Quaternion>(ELogicAttr.AttrRot);
					if (CommonUtility.HitRayCast(_filterSetting.BoxData, pos, rot, out var hitActorIds)) {
						foreach (var uid in hitActorIds) {
							var unSelectable =
								_actorManager._uidActorDict[uid].GetAttr<int>(ELogicAttr.AttrUnselectable) != 0;
							if (unSelectable) continue;
							actors.Add(_actorManager._uidActorDict[uid]);
						}
					}
				}
				else {
					actors.AddRange(_actorManager._runningActorList);
				}

				if (!_filterSetting.HasSelf && actors.Contains(_filterUser)) {
					actors.Remove(_filterUser);
				}

				foreach (var actor in actors) {
					bool pass = true;
					foreach (var range in _filterSetting.Ranges) {
						if (!rangeCheck(actor, range)) {
							pass = false;
							break;
						}
					}

					if (!pass) {
						continue;
					}

					foreach (var compare in _filterSetting.Compares) {
						var left = (IComparable)actor.Logic.GetAttrBox(compare.AttrType);
						int res = left.CompareTo(compare.CompareValue);
						if (!getCompareRes(compare.CompareResType, res)) {
							pass = false;
							break;
						}
					}

					if (!pass) {
						continue;
					}

					actorUids.Add(actor.Uid);
				}

				if (_filterSetting.MaxTargetCount <= 0) {
					return actorUids;
				}

				if (_filterSetting.MaxTargetCount >= actorUids.Count) {
					return actorUids;
				}

				switch (_filterSetting.FilterFunctionType) {
					case EFilterFunctionType.HighestHp:
						actorUids.Sort((aUid, bUid) => {
							int aHp = _actorManager._uidActorDict[aUid].GetAttr<int>(ELogicAttr.AttrHp);
							int bHp = _actorManager._uidActorDict[bUid].GetAttr<int>(ELogicAttr.AttrHp);
							return aHp.CompareTo(bHp);
						});
						break;
					case EFilterFunctionType.LeastHp:
						actorUids.Sort((aUid, bUid) => {
							int aHp = _actorManager._uidActorDict[aUid].GetAttr<int>(ELogicAttr.AttrHp);
							int bHp = _actorManager._uidActorDict[bUid].GetAttr<int>(ELogicAttr.AttrHp);
							return aHp.CompareTo(bHp) * -1;
						});
						break;
					case EFilterFunctionType.HighestMp:
						actorUids.Sort((aUid, bUid) => {
							int aMp = _actorManager._uidActorDict[aUid].GetAttr<int>(ELogicAttr.AttrMp);
							int bMp = _actorManager._uidActorDict[bUid].GetAttr<int>(ELogicAttr.AttrMp);
							return aMp.CompareTo(bMp);
						});
						break;
					case EFilterFunctionType.LeastMp:
						actorUids.Sort((aUid, bUid) => {
							int aMp = _actorManager._uidActorDict[aUid].GetAttr<int>(ELogicAttr.AttrMp);
							int bMp = _actorManager._uidActorDict[bUid].GetAttr<int>(ELogicAttr.AttrMp);
							return aMp.CompareTo(bMp) * -1;
						});
						break;
					case EFilterFunctionType.Far:
						actorUids.Sort((aUid, bUid) => {
							var aPos = _actorManager._uidActorDict[aUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
							var bPos = _actorManager._uidActorDict[bUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
							var selfPos = _filterUser.GetAttr<Vector3>(ELogicAttr.AttrPosition);

							var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
							var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
							return aDis.CompareTo(bDis);
						});
						break;
					case EFilterFunctionType.Near:
						actorUids.Sort((aUid, bUid) => {
							var aPos = _actorManager._uidActorDict[aUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
							var bPos = _actorManager._uidActorDict[bUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
							var selfPos = _filterUser.GetAttr<Vector3>(ELogicAttr.AttrPosition);

							var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
							var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
							return aDis.CompareTo(bDis) * -1;
						});
						break;
					case EFilterFunctionType.Random:
						actorUids = CommonUtility.SelectRandomElements(actorUids, _filterSetting.MaxTargetCount);
						return actorUids;
				}

				actorUids = actorUids.GetRange(0, _filterSetting.MaxTargetCount);

				return actorUids;
			}
		}
	}
}