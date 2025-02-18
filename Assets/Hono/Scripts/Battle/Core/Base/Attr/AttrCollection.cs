using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public class AttrCollection
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

        public AttrCollection(Unit unit)
        {
            _unit = unit;
        }

        /// <summary>
        /// 通过表来设置属性
        /// </summary>
        public void Init(int attrTableConfigId)
        {
            if (!ConfigDataBase.Table<EntityAttrBaseTable>().TryGet(attrTableConfigId, out var attrRow))
            {
                Debug.LogError($"AttrTable找不到指定Id:{attrTableConfigId}");
                return;
            }

            _allowDirty = false;
            AttrHelper.Instance.InitByTableRow(this, attrRow);
            _allowDirty = true;
        }

        /// <summary>
        /// 执行召唤物相关属性流程初始化
        /// </summary>
        /// <param name="summoner"></param>
        /// <param name="fromTopSummer">基础来源是否来自</param>
        public void SetSummoned(Unit summoner)
        {
            _allowDirty = false;
            SetAttr(EAttrType.AttrIsSummoned, 1);
            var sourceUid = summoner.GetAttr(EAttrType.AttrSourceActorUid);
            SetAttr(EAttrType.AttrSourceActorUid,    sourceUid);
            SetAttr(EAttrType.AttrTopSourceActorUid, summoner.GetAttr(EAttrType.AttrTopSourceActorUid));
            SetAttr(EAttrType.AttrFaction,           summoner.GetAttr(EAttrType.AttrFaction));
            _allowDirty = true;
        }

        /// <summary>
        /// 继承属性
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="inheritSetting"></param>
        public void InheritAttrs(Unit parent, WorldInstance.InheritSetting inheritSetting) { }

        /// <summary>
        /// 获取属性快照
        /// </summary>
        public void GetAttrSnapShots(ref Dictionary<EAttrType, int> snapshot)
        {
            snapshot ??= new Dictionary<EAttrType, int>(_attrs.Count);
            snapshot.Clear();
            foreach (var pair in _attrs)
            {
                snapshot.Add(pair.Key, pair.Value);
            }
        }

        public int GetAttr(EAttrType attrType)
        {
            if (!_attrs.TryGetValue(attrType, out Attr attr))
            {
                attr = GPool<Attr>.Pool.Rent();
                _attrs.Add(attrType, attr);
            }

            return attr;
        }

        /// <summary>
        /// 初始化属性，不会触发dirty
        /// </summary>
        /// <param name="attrType"></param>
        /// <param name="value"></param>
        public void InitAttr(EAttrType attrType, int value)
        {
            _allowDirty = true;
            SetAttr(attrType, value);
            _allowDirty = false;
        }

        public void SetAttr(EAttrType attrType, int value, bool forceDirty = false)
        {
            var attrTypeInt = attrType;

            if (!_attrs.TryGetValue(attrTypeInt, out var attr))
            {
                attr = GPool<Attr>.Pool.Rent();
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
                var board = GPool<VariableBoard>.Pool.Rent();
                board.SetEvtField(AttrChangedEventInfo.AttrType, attrType);
                board.SetEvtField(AttrChangedEventInfo.Value,    value);
                EventManager.Instance.FireWorldEvent(EEventType.OnAttrChanged, board);
                GPool<VariableBoard>.Pool.Recycle(board);
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