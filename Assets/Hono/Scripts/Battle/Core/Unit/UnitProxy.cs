using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public class UnitProxy : IGPoolObject
    {
        /// <summary>
        /// 非代理模式直接保存Unit对象
        /// </summary>
        private Unit _unit;

        /// <summary>
        /// 来源
        /// </summary>
        public int Uid;
        
        /// <summary>
        /// 属性快照
        /// </summary>
        private Dictionary<EAttrType, int> _attrSnapshot = new(256);

        /// <summary>
        /// Tag快照
        /// </summary>
        private HashSet<int> _tagsSnapshot = new(256);

        /// <summary>
        /// 位置快照
        /// </summary>
        private UnitTransform _unitTransformSnapShot;

        /// <summary>
        /// 状态位快照
        /// </summary>
        private EUnitFlag _state;

        /// <summary>
        /// 战斗组件快照
        /// </summary>
        private Dictionary<int, int> _buffLayer = new(10);

        /// <summary>
        /// 战斗组件快照
        /// </summary>
        private SkillModifier _skillRts = new();

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Pos => _unitTransformSnapShot.Pos;

        /// <summary>
        /// Y轴角度
        /// </summary>
        public float YAxisAngle => _unitTransformSnapShot.YAxisAngle;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="unit">被代理的Unit</param>
        /// <param name="isSnapshot">是否存储快照</param>
        public void Init(Unit unit, bool isSnapshot)
        {
            if (!isSnapshot)
            {
                _unit = unit;
                return;
            }

            Uid = unit.Uid;
            unit.Attrs.GetAttrSnapShots(ref _attrSnapshot);
            unit.Tags.GetSnapshot(ref _tagsSnapshot);
            _unitTransformSnapShot = unit.UnitTransform;
            _state = unit.State;
        }

        public int GetAttr(EAttrType attrType)
        {
            if (_unit != null)
            {
                return _unit.GetAttr(attrType) ;
            }

            return _attrSnapshot.GetValueOrDefault(attrType, 0);
        }

        public bool HasTag(int tag)
        {
            if (_unit != null)
            {
                return _unit.Tags.HasTag(tag);
            }

            if (_tagsSnapshot.Contains(tag))
            {
                return true;
            }

            foreach (var item in _tagsSnapshot)
            {
                if (TagTreeHelper.HasParent(tag, item))
                {
                    return true;
                }
            }

            return false;
        }
        
        public void OnRecycle()
        {
            _unit = null;
            _attrSnapshot.Clear();
            _tagsSnapshot.Clear();
            _unitTransformSnapShot = default;
        }
    }
}