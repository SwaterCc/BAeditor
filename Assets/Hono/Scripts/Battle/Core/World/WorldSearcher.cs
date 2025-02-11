using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// Unit检索器，用于快速搜索符合条件的Unit
    /// </summary>
    public class WorldSearcher
    {
        /// <summary>
        /// Unit索引字典
        /// </summary>
        private readonly Dictionary<int, Unit> _searchDict = new(2000);
        private readonly Dictionary<int, Actor> _actorSearchDict = new(2000);
        private readonly List<Unit> _filterActors = new(32);
        private List<int> _checkBoxResult = new(32);
        private RangeFilterSetting _rangeFilterSetting;
        private Unit _filterUser;
        private Vector3 _searchCenterPos;

        public void RemoveUnitLookup(Unit unit)
        {
            _searchDict.Remove(unit.Uid);
        }
        
        public Unit GetUnit(int uid)
        {
            return null;
        }

        public bool TryGetUnit(int uid, out Unit unit)
        {
            return _searchDict.TryGetValue(uid, out unit);
        }

        public Actor GetActor(int uid)
        {
            return null;
        }

        public bool TryGetActor(int uid, out Actor unit)
        {
            return _actorSearchDict.TryGetValue(uid, out unit);
        }
        
        public void SearchUnits(Unit user, Vector3 centerPos, RangeFilterSetting setting, ref List<int> result)
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

            _filterUser = user;
            _searchCenterPos = centerPos;
            _rangeFilterSetting = setting;
            result.Clear();

            getResults(ref result);
        }

        public bool CheckActorPassFilter(Unit filterUser, int checkActorUid, ConditionFilterSetting setting)
        {
            if (!_searchDict.TryGetValue(checkActorUid, out var unit))
            {
                return false;
            }

            _filterUser = filterUser;
            bool result = checkActorPass(unit, setting);
            _filterUser = null;
            return result;
        }

        private bool rangeCheck(Unit unit, in FilterCondition condition)
        {
            bool checkResult = false;
            switch (condition.conditionType)
            {
                case EFilterConditionType.ActorType:
                    if (unit is not Actor actor)
                        return false;
                    checkResult = (int)actor.ActorType == condition.value;
                    break;
                case EFilterConditionType.Tag:
                    checkResult = unit.Tags.HasTag(condition.value);
                    break;
                case EFilterConditionType.Faction:
                    var f1 = _filterUser.GetAttr(EAttrType.AttrFaction);
                    var f2 = unit.GetAttr(EAttrType.AttrFaction);
                    checkResult = LuaInterface.GetFaction(f1, f2) == condition.value;
                    break;
                default:
                    Debug.LogError($"使用了未实现的范围筛选 settingId {_filterUser.Uid} type {condition.conditionType}");
                    return false;
            }

            if (condition.isReverse)
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

        private bool checkActorPass(Unit unit, ConditionFilterSetting conditionFilterSetting)
        {
            foreach (var condition in conditionFilterSetting.conditions)
            {
                if (!rangeCheck(unit, condition))
                {
                    return false;
                }
            }

            foreach (var compare in conditionFilterSetting.attrCompares)
            {
                var left = unit.GetAttr(compare.attrType);
                int res = left.CompareTo((int)compare.compareValue);
                if (!getCompareRes(compare.compareResType, res))
                {
                    return false;
                }
            }

            return true;
        }

        private void getResults(ref List<int> result)
        {
            Profiler.BeginSample("UseFilter");
            if (_rangeFilterSetting == null)
            {
                Debug.LogError("筛选器设置为空");
                return;
            }

            if (_rangeFilterSetting.OpenBoxCheck)
            {
                var pos = _filterUser.UnitTransform.Pos;
                var rot = Quaternion.AngleAxis(_filterUser.UnitTransform.YAxisAngle, Vector3.up);

                if (CommonUtility.HitRayCast(_rangeFilterSetting.BoxData, pos, rot, ref _checkBoxResult))
                {
                    foreach (var uid in _checkBoxResult)
                    {
                        var unSelectable =
                            _searchDict[uid].GetAttr(EAttrType.AttrUnselectable) != 0;
                        if (unSelectable) continue;
                        _filterActors.Add(_searchDict[uid]);
                    }

                    _checkBoxResult.Clear();
                }
            }
            else
            {
                _filterActors.AddRange(_searchDict.Values);
            }

            if (!_rangeFilterSetting.includeSelf && _filterActors.Contains(_filterUser))
            {
                _filterActors.Remove(_filterUser);
            }

            foreach (var actor in _filterActors)
            {
                if (checkActorPass(actor, _rangeFilterSetting.conditionFilterSetting))
                    result.Add(actor.Uid);
            }

            if (_rangeFilterSetting.maxResultCount <= 0)
            {
                return;
            }

            if (_rangeFilterSetting.maxResultCount >= result.Count)
            {
                return;
            }

            //TODO:实现比较器，减少GC
            switch (_rangeFilterSetting.filterFunctionType)
            {
                case EFilterFunctionType.LeastHp:
                    result.Sort((aUid, bUid) =>
                    {
                        int aHp = _searchDict[aUid].GetAttr(EAttrType.AttrHp);
                        int bHp = _searchDict[bUid].GetAttr(EAttrType.AttrHp);
                        return aHp.CompareTo(bHp);
                    });
                    break;
                case EFilterFunctionType.HighestHp:
                    result.Sort((aUid, bUid) =>
                    {
                        int aHp = _searchDict[aUid].GetAttr(EAttrType.AttrHp);
                        int bHp = _searchDict[bUid].GetAttr(EAttrType.AttrHp);
                        return aHp.CompareTo(bHp) * -1;
                    });
                    break;
                case EFilterFunctionType.LeastMp:
                    result.Sort((aUid, bUid) =>
                    {
                        int aMp = _searchDict[aUid].GetAttr(EAttrType.AttrMp);
                        int bMp = _searchDict[bUid].GetAttr(EAttrType.AttrMp);
                        return aMp.CompareTo(bMp);
                    });
                    break;
                case EFilterFunctionType.HighestMp:
                    result.Sort((aUid, bUid) =>
                    {
                        int aMp = _searchDict[aUid].GetAttr(EAttrType.AttrMp);
                        int bMp = _searchDict[bUid].GetAttr(EAttrType.AttrMp);
                        return aMp.CompareTo(bMp) * -1;
                    });
                    break;
                case EFilterFunctionType.Far:
                    result.Sort((aUid, bUid) =>
                    {
                        var aPos = _searchDict[aUid].UnitTransform.Pos;
                        var bPos = _searchDict[bUid].UnitTransform.Pos;
                        var selfPos = _filterUser.UnitTransform.Pos;

                        var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                        var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                        return aDis.CompareTo(bDis) * -1;
                    });
                    break;
                case EFilterFunctionType.Near:
                    result.Sort((aUid, bUid) =>
                    {
                        var aPos = _searchDict[aUid].UnitTransform.Pos;
                        var bPos = _searchDict[bUid].UnitTransform.Pos;
                        var selfPos = _filterUser.UnitTransform.Pos;

                        var aDis = Math.Abs(Vector3.Distance(aPos, selfPos));
                        var bDis = Math.Abs(Vector3.Distance(bPos, selfPos));
                        return aDis.CompareTo(bDis);
                    });
                    break;
                case EFilterFunctionType.Random:
                    CommonUtility.Shuffle(ref result);
                    break;
                case EFilterFunctionType.ControlPlayer:
                    break;
            }

            for (int index = result.Count - 1; index > -1; index--)
            {
                if (index > _rangeFilterSetting.maxResultCount - 1)
                {
                    result.RemoveAt(index);
                }
            }

            _filterActors.Clear();
            _checkBoxResult.Clear();

            Profiler.EndSample();
        }
    }
}