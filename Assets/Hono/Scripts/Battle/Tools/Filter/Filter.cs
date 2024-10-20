using System;
using System.Collections;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle
{
    public partial class ActorManager
    {
        private Filter _filter;

        public List<int> UseFilter(Actor filterUser,in FilterSetting setting)
        {
            if (setting == null)
            {
                Debug.LogError("Setting is Empty!");
                return null;
            }

            _filter.Reset();
            _filter.SettingChange(filterUser, setting);
            return _filter.GetResults();
        }

        public bool CheckActorPassFilter(Actor filterUser,int checkActorUid,in FilterSetting setting)
        {
            _filter.SettingChange(filterUser, setting);
            return _filter.CheckPass(checkActorUid);
        }

        private class Filter
        {
            private FilterSetting _filterSetting;
            private Actor _filterUser;
            private readonly ActorManager _actorManager;
            private List<int> _filterActorUids = new(32);
            private readonly List<Actor> _filterActors = new(32);
            private List<int> _checkBoxResult = new(32);
            internal Filter(ActorManager actorManager)
            {
                _actorManager = actorManager;
            }

            public void Reset() {
	            _filterActorUids.Clear();
	            _filterActors.Clear();
	            _checkBoxResult.Clear();
            }
            
            public void SettingChange(in Actor filterUser,in FilterSetting setting)
            {
                _filterSetting = setting;
                _filterUser = filterUser;
            }

            private bool rangeCheck(in Actor actor,in FilterRange range)
            {
                bool checkResult = false;
                switch (range.RangeType)
                {
                    case EFilterRangeType.ActorType:
                        checkResult = (int)actor.ActorType == range.Value;
                        break;
                    case EFilterRangeType.Tag:
                        checkResult = actor.HasTag(range.Value);
                        break;
                    case EFilterRangeType.ActorState:
                        checkResult = actor.Logic.CurState() == (EActorLogicStateType)range.Value;
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

            private bool getCompareRes(in ECompareResType compareResType, int flag)
            {
                switch (compareResType)
                {
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

            public bool CheckPass(int uid)
            {
                return _actorManager._uidActorSearchDict.TryGetValue(uid, out var actor) && checkActorPass(actor);
            }

            private bool checkActorPass(in Actor actor)
            {
                foreach (var range in _filterSetting.Ranges)
                {
                    if (!rangeCheck(actor, range))
                    {
                        return false;
                    }
                }

                foreach (var compare in _filterSetting.Compares)
                {
                    var left = (IComparable)actor.Logic.GetAttrBox(compare.AttrType);
                    int res = left.CompareTo(compare.CompareValue);
                    if (!getCompareRes(compare.CompareResType, res))
                    {
                        return false;
                    }
                }

                return true;
            }

            public List<int> GetResults()
            {
	            Profiler.BeginSample("UseFilter");
                if (_filterSetting == null)
                {
                    Debug.LogError("筛选器设置为空");
                    return null;
                }
                
                if (_filterSetting.OpenBoxCheck)
                {
                    var pos = _filterUser.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                    var rot = _filterUser.GetAttr<Quaternion>(ELogicAttr.AttrRot);
                    if (CommonUtility.HitRayCast(_filterSetting.BoxData, pos, rot, ref _checkBoxResult))
                    {
                        foreach (var uid in _checkBoxResult)
                        {
							if (_actorManager.GetActorRtState(uid) != EActorRunningState.Active)
								continue;
                            var unSelectable =
                                _actorManager._uidActorSearchDict[uid].GetAttr<int>(ELogicAttr.AttrUnselectable) != 0;
                            if (unSelectable) continue;
                            if (_actorManager._uidActorSearchDict[uid].IsExpired)
	                            continue;
                            _filterActors.Add(_actorManager._uidActorSearchDict[uid]);
                        }
                        _checkBoxResult.Clear();
                    }
                }
                else
                {
                    _filterActors.AddRange(_actorManager._runningActorList);
                }

                if (!_filterSetting.HasSelf && _filterActors.Contains(_filterUser))
                {
                    _filterActors.Remove(_filterUser);
                }

                foreach (var actor in _filterActors)
                {
                    if (checkActorPass(actor))
                        _filterActorUids.Add(actor.Uid);
                }

                if (_filterSetting.MaxTargetCount <= 0)
                {
                    return _filterActorUids;
                }

                if (_filterSetting.MaxTargetCount >= _filterActorUids.Count)
                {
                    return _filterActorUids;
                }

                switch (_filterSetting.FilterFunctionType)
                {
                    case EFilterFunctionType.HighestHp:
                        _filterActorUids.Sort((aUid, bUid) =>
                        {
                            int aHp = _actorManager._uidActorSearchDict[aUid].GetAttr<int>(ELogicAttr.AttrHp);
                            int bHp = _actorManager._uidActorSearchDict[bUid].GetAttr<int>(ELogicAttr.AttrHp);
                            return aHp.CompareTo(bHp);
                        });
                        break;
                    case EFilterFunctionType.LeastHp:
                        _filterActorUids.Sort((aUid, bUid) =>
                        {
                            int aHp = _actorManager._uidActorSearchDict[aUid].GetAttr<int>(ELogicAttr.AttrHp);
                            int bHp = _actorManager._uidActorSearchDict[bUid].GetAttr<int>(ELogicAttr.AttrHp);
                            return aHp.CompareTo(bHp) * -1;
                        });
                        break;
                    case EFilterFunctionType.HighestMp:
                        _filterActorUids.Sort((aUid, bUid) =>
                        {
                            int aMp = _actorManager._uidActorSearchDict[aUid].GetAttr<int>(ELogicAttr.AttrMp);
                            int bMp = _actorManager._uidActorSearchDict[bUid].GetAttr<int>(ELogicAttr.AttrMp);
                            return aMp.CompareTo(bMp);
                        });
                        break;
                    case EFilterFunctionType.LeastMp:
                        _filterActorUids.Sort((aUid, bUid) =>
                        {
                            int aMp = _actorManager._uidActorSearchDict[aUid].GetAttr<int>(ELogicAttr.AttrMp);
                            int bMp = _actorManager._uidActorSearchDict[bUid].GetAttr<int>(ELogicAttr.AttrMp);
                            return aMp.CompareTo(bMp) * -1;
                        });
                        break;
                    case EFilterFunctionType.Far:
                        _filterActorUids.Sort((aUid, bUid) =>
                        {
                            var aPos = _actorManager._uidActorSearchDict[aUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
                            var bPos = _actorManager._uidActorSearchDict[bUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
                            var selfPos = _filterUser.GetAttr<Vector3>(ELogicAttr.AttrPosition);

                            var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                            var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                            return aDis.CompareTo(bDis) * -1;
                        });
                        break;
                    case EFilterFunctionType.Near:
                        _filterActorUids.Sort((aUid, bUid) =>
                        {
                            var aPos = _actorManager._uidActorSearchDict[aUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
                            var bPos = _actorManager._uidActorSearchDict[bUid].GetAttr<Vector3>(ELogicAttr.AttrPosition);
                            var selfPos = _filterUser.GetAttr<Vector3>(ELogicAttr.AttrPosition);

                            var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                            var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                            return aDis.CompareTo(bDis);
                        });
                        break;
                    case EFilterFunctionType.Random:
                        CommonUtility.Shuffle(ref _filterActorUids);
                        return _filterActorUids;
                }

                for (int index = _filterActorUids.Count - 1; index > -1; index--) {
	                if (index > _filterSetting.MaxTargetCount -1) {
		                _filterActorUids.RemoveAt(index);
	                }
                }
                Profiler.EndSample();
                return _filterActorUids;
            }
        }
    }
}