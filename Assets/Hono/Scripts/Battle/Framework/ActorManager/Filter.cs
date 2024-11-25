#region

using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.Profiling;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorManager
    {
        private Filter _filter;

        public void UseFilter(Actor filterUser, in FilterSetting setting, ref List<int> result)
        {
            if (setting == null)
            {
                Debug.LogError("Setting is Empty!");
                return;
            }

            if (result == null)
            {
                Debug.LogError("result list is Empty!");
                return;
            }

            result.Clear();
            _filter.Reset();
            _filter.SettingChange(filterUser, setting);
            _filter.GetResults(ref result);
        }

        public bool CheckActorPassFilter(Actor filterUser, int checkActorUid, in FilterSetting setting)
        {
            _filter.SettingChange(filterUser, setting);
            return _filter.CheckPass(checkActorUid);
        }

        private class Filter
        {
            private FilterSetting _filterSetting;
            private Actor _filterUser;
            private readonly ActorManager _actorManager;
            private readonly List<Actor> _filterActors = new(32);
            private List<int> _checkBoxResult = new(32);

            internal Filter(ActorManager actorManager)
            {
                _actorManager = actorManager;
            }

            public void Reset()
            {
                _filterActors.Clear();
                _checkBoxResult.Clear();
            }

            public void SettingChange(in Actor filterUser, in FilterSetting setting)
            {
                _filterSetting = setting;
                _filterUser = filterUser;
            }

            private bool rangeCheck(in Actor actor, in FilterRange range)
            {
                bool checkResult = false;
                switch (range.RangeType)
                {
                    case EFilterRangeType.ActorType:
                        checkResult = (int)actor.ActorType == range.Value;
                        break;
                    case EFilterRangeType.Tag:
                        checkResult = actor.TagCollection.HasTag(range.Value, ETagSearchRange.Actor);
                        break;
                    case EFilterRangeType.ActorState:
                        checkResult = actor.Logic.CurState() == (EActorStateType)range.Value;
                        break;
                    case EFilterRangeType.AbilityID:
                        checkResult = actor.Abilities.HasAbility(range.Value);
                        break;
                    case EFilterRangeType.Faction:
                        var f1 = _filterUser.GetAttr(EAttrType.AttrFaction);
                        var f2 = actor.GetAttr(EAttrType.AttrFaction);
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
                return _actorManager._uidActorDict.TryGetValue(uid, out var actor) && checkActorPass(actor);
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
                    var left = actor.GetAttr(compare.attrTypeType);
                    int res = left.CompareTo((int)compare.CompareValue);
                    if (!getCompareRes(compare.CompareResType, res))
                    {
                        return false;
                    }
                }

                return true;
            }

            public void GetResults(ref List<int> result)
            {
                Profiler.BeginSample("UseFilter");
                if (_filterSetting == null)
                {
                    Debug.LogError("筛选器设置为空");
                    return;
                }

                if (_filterSetting.OpenBoxCheck)
                {
                    var pos = _filterUser.Pos;
                    var rot = _filterUser.Rot;

                    //bool show = _filterUser.GetAttr(ELogicAttr.AttrFaction) == 11;

                    if (CommonUtility.HitRayCast(_filterSetting.BoxData, pos, rot, ref _checkBoxResult))
                    {
                        foreach (var uid in _checkBoxResult)
                        {
                            var unSelectable =
                                _actorManager._uidActorDict[uid].GetAttr(EAttrType.AttrUnselectable) != 0;
                            if (unSelectable) continue;
                            _filterActors.Add(_actorManager._uidActorDict[uid]);
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
                        result.Add(actor.Uid);
                }

                if (_filterSetting.MaxTargetCount <= 0)
                {
                    return;
                }

                if (_filterSetting.MaxTargetCount >= result.Count)
                {
                    return;
                }

                //TODO:实现比较器，减少GC
                switch (_filterSetting.FilterFunctionType)
                {
                    case EFilterFunctionType.LeastHp:
                        result.Sort((aUid, bUid) =>
                        {
                            int aHp = _actorManager._uidActorDict[aUid].GetAttr(EAttrType.AttrHp);
                            int bHp = _actorManager._uidActorDict[bUid].GetAttr(EAttrType.AttrHp);
                            return aHp.CompareTo(bHp);
                        });
                        break;
                    case EFilterFunctionType.HighestHp:
                        result.Sort((aUid, bUid) =>
                        {
                            int aHp = _actorManager._uidActorDict[aUid].GetAttr(EAttrType.AttrHp);
                            int bHp = _actorManager._uidActorDict[bUid].GetAttr(EAttrType.AttrHp);
                            return aHp.CompareTo(bHp) * -1;
                        });
                        break;
                    case EFilterFunctionType.LeastMp:
                        result.Sort((aUid, bUid) =>
                        {
                            int aMp = _actorManager._uidActorDict[aUid].GetAttr(EAttrType.AttrMp);
                            int bMp = _actorManager._uidActorDict[bUid].GetAttr(EAttrType.AttrMp);
                            return aMp.CompareTo(bMp);
                        });
                        break;
                    case EFilterFunctionType.HighestMp:
                        result.Sort((aUid, bUid) =>
                        {
                            int aMp = _actorManager._uidActorDict[aUid].GetAttr(EAttrType.AttrMp);
                            int bMp = _actorManager._uidActorDict[bUid].GetAttr(EAttrType.AttrMp);
                            return aMp.CompareTo(bMp) * -1;
                        });
                        break;
                    case EFilterFunctionType.Far:
                        result.Sort((aUid, bUid) =>
                        {
                            var aPos = _actorManager._uidActorDict[aUid].Pos;
                            var bPos = _actorManager._uidActorDict[bUid].Pos;
                            var selfPos = _filterUser.Pos;

                            var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                            var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                            return aDis.CompareTo(bDis) * -1;
                        });
                        break;
                    case EFilterFunctionType.Near:
                        result.Sort((aUid, bUid) =>
                        {
                            var aPos = _actorManager._uidActorDict[aUid].Pos;
                            var bPos = _actorManager._uidActorDict[bUid].Pos;
                            var selfPos = _filterUser.Pos;

                            var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                            var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                            return aDis.CompareTo(bDis);
                        });
                        break;
                    case EFilterFunctionType.Random:
                        CommonUtility.Shuffle(ref result);
                        break;
                    case EFilterFunctionType.ControlPlayer:
                        result.Clear();
                        result.Add(BattleManager.CurBattle.RtInfo.LeaderUid);
                        break;
                }

                for (int index = result.Count - 1; index > -1; index--)
                {
                    if (index > _filterSetting.MaxTargetCount - 1)
                    {
                        result.RemoveAt(index);
                    }
                }

                Profiler.EndSample();
            }
        }
    }
}