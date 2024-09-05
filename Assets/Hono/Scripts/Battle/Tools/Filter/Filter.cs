using System;
using System.Collections;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class ActorManager
    {
        private Filter _filter;

        public List<int> UseFilter(ActorLogic target, FilterSetting setting)
        {
            if (setting == null)
            {
                Debug.LogError("Setting is Empty!");
                return null;
            }

            _filter.SettingChange(target, setting);
            return _filter.GetResults();
        }

        private class Filter
        {
            private FilterSetting _filterSetting;
            private ActorLogic _filterUser;
            private readonly ActorManager _actorManager;

            internal Filter(ActorManager actorManager)
            {
                _actorManager = actorManager;
            }

            public void SettingChange(ActorLogic filterUser, FilterSetting setting)
            {
                _filterSetting = setting;
                _filterUser = filterUser;
            }

            private bool rangeCheck(ActorLogic logic, FilterRange range)
            {
                switch (range.RangeType)
                {
                    case EFilterRangeType.Tag:
                        return logic.HasTag(range.Value);
                    case EFilterRangeType.ActorState:
                        return logic.CurState() == (EActorState)range.Value;
                    case EFilterRangeType.AbilityID:
                        return logic.HasAbility(range.Value);
                    case EFilterRangeType.Faction :
                        var f1 = _filterUser.GetAttr<int>(ELogicAttr.AttrFaction);
                        var f2 = _filterUser.GetAttr<int>(ELogicAttr.AttrFaction);
                        return LuaInterface.GetFaction(f1, f2) == (EFactionType)range.Value;
                }

                Debug.LogError($"使用了未实现的范围筛选 settingId {_filterSetting.id} type {range.RangeType}");
                return false;
            }

            private bool getCompareRes(ECompareResType compareResType, int flag)
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

            public List<int> GetResults()
            {
                if (_filterSetting == null)
                {
                    Debug.LogError("筛选器设置为空");
                    return null;
                }

                List<int> actorIds = new List<int>();

                List<Actor> actors;
                if (_filterSetting.OpenBoxCheck && _filterSetting.BoxData != null)
                {
                    actors = new List<Actor>();
                    var pos = _filterUser.GetAttr<Vector3>(ELogicAttr.AttrPosition);
                    var rot = _filterUser.GetAttr<Quaternion>(ELogicAttr.AttrRot);
                    if (CommonUtility.HitRayCast(_filterSetting.BoxData, pos, rot, out var hitActorIds))
                    {
                        foreach (var uid in hitActorIds)
                        {
                            if (uid == _filterUser.Uid) continue;
                            actors.Add(_actorManager._uidActorDict[uid]);
                        }
                    }
                }
                else
                {
                    actors = _actorManager._runningActorList;
                }

                foreach (var actor in actors)
                {
                    bool pass = true;
                    foreach (var range in _filterSetting.Ranges)
                    {
                        if (!rangeCheck(actor.Logic, range))
                        {
                            pass = false;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        continue;
                    }

                    foreach (var compare in _filterSetting.Compares)
                    {
                        var left = (IComparable)actor.Logic.GetAttrBox(compare.AttrType);
                        int res = left.CompareTo(compare.CompareValue);
                        if (!getCompareRes(compare.CompareResType, res))
                        {
                            pass = false;
                            break;
                        }
                    }

                    if (!pass)
                    {
                        continue;
                    }

                    actorIds.Add(actor.Uid);
                }

                return actorIds;
            }
        }
    }
}