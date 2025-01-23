using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle.Base
{
    public partial class AttrCollection
    {
        private readonly Unit _unit;
        /// <summary>
        /// 允许记录脏标记
        /// </summary>
        private bool _allowDirty = true;

        /// <summary>
        /// 属性列表
        /// </summary>
        private readonly Dictionary<EAttrType, Attr> _attrs = new(128);

        /// <summary>
        /// 脏属性列表
        /// </summary>
        private readonly List<(EAttrType, int)> _dirtyList = new(128);

        public AttrCollection(Unit unit)
        {
            _unit = unit;
        }

        /// <summary>
        /// 通过表来设置属性
        /// </summary>
        /// <param name="attrTableConfigId"></param>
        /// <param name="isInit">如果是初始化流程，则不会触发属性修改事件</param>
        public void SetAttrsByTable(int attrTableConfigId, bool isInit)
        {
            if (!ConfigManager.Table<EntityAttrBaseTable>().TryGet(attrTableConfigId, out var attrRow))
            {
                Debug.LogError($"AttrTable找不到指定Id:{attrTableConfigId}");
                return;
            }

            _allowDirty = !isInit;
            AttrHelper.Instance.InitByTableRow(this, attrRow);
            _allowDirty = true;
        }

        /// <summary>
        /// 通过快照来修改属性
        /// </summary>
        /// <param name="snapshots"></param>
        /// <param name="isInit"></param>
        public void SetAttrsBySnapshot(AttrSnapshots snapshots, bool isInit)
        {
            if (snapshots == null)
                return;

            _allowDirty = !isInit;
            foreach (var snapshot in snapshots)
            {
                SetAttr(snapshot.Key, snapshot.Value);
            }

            _allowDirty = true;

            APool<AttrSnapshots>.Pool.Recycle(snapshots);
        }

        /// <summary>
        /// 执行召唤物相关属性流程初始化
        /// </summary>
        /// <param name="summoner"></param>
        /// <param name="fromTopSummer"></param>
        public void InitSummonedAttrs(AttrCollection summoner, bool fromTopSummer)
        {
            SetAttr(EAttrType.AttrIsSummoned, 1);
            var sourceUid = fromTopSummer
                ? summoner.GetAttr(EAttrType.AttrTopSourceActorUid)
                : summoner.GetAttr(EAttrType.AttrSourceActorUid);
            SetAttr(EAttrType.AttrSourceActorUid,    sourceUid);
            SetAttr(EAttrType.AttrTopSourceActorUid, summoner.GetAttr(EAttrType.AttrTopSourceActorUid));
            SetAttr(EAttrType.AttrFaction,           summoner.GetAttr(EAttrType.AttrFaction));
        }

        /// <summary>
        /// 获取属性快照
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <returns></returns>
        public AttrSnapshots GetAttrSnapShots(string rule, int param1 = 0, int param2 = 0, int param3 = 0, int param4 = 0)
        {
            var snapshot = APool<AttrSnapshots>.Pool.Rent();
            snapshot.Init(this);
            snapshot.AddParam(param1);
            snapshot.AddParam(param2);
            snapshot.AddParam(param3);
            snapshot.AddParam(param4);
            snapshot.Process(rule);
            return snapshot;
        }

        public Attr GetAttr(EAttrType attrType)
        {
            if (!_attrs.TryGetValue(attrType, out Attr attr))
            {
                attr = APool<Attr>.Pool.Rent();
                _attrs.Add(attrType, attr);
            }

            return attr;
        }

        public void SetAttr(EAttrType attrType, int value, bool forceDirty = false)
        {
            var attrTypeInt = attrType;

            if (!_attrs.TryGetValue(attrTypeInt, out var attr))
            {
                attr = APool<Attr>.Pool.Rent();
                _attrs.Add(attrTypeInt, attr);
            }

            if (attr.Set(value, forceDirty))
            {
                setDirty(attrType, attr);
            }
        }

        private void setDirty(EAttrType attrType, int value)
        {
            if (_allowDirty)
            {
                _dirtyList.Add((attrType, value));
                var board = APool<VariableBoard>.Pool.Rent();
                board.Set(AttrChangedEventInfo.AttrType, attrType);
                board.Set(AttrChangedEventInfo.Value,    value);
                EventManager.Instance.FireEvent(EEventType.AttrChanged, _unit.Uid, board);
                APool<VariableBoard>.Pool.Recycle(board);
            }

            //查找该属性关联的其他属性
            if (AttrHelper.Instance.TryGetLink(attrType, out var link))
            {
                link.UpdateLink(this);
            }
        }

        public void Clear()
        {
            _allowDirty = true;
        }
    }
}