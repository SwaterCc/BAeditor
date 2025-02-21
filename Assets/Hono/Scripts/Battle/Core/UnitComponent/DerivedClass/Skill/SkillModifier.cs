using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public class SkillModifier : IGPoolObject
    {
        private int _level;
        /// <summary>
        /// 技能等级
        /// </summary>
        public int Level
        {
            get => _level;

            set
            {
                _level = value;
                if (_level <= 0)
                {
                    _level = 1;
                }

                if (_level > 10)
                {
                    _level = 10;
                }
            }
        }

        /// <summary>
        /// 技能额外属性修改
        /// </summary>
        private readonly Dictionary<EAttrType, int> _attrModify = new(16);

        /// <summary>
        /// 技能能量消耗加值
        /// </summary>
        private readonly Dictionary<int, int> _energyCostAdd = new(5);
        
        /// <summary>
        /// 技能能量消耗乘值
        /// </summary>
        private readonly Dictionary<int, int> _energyCostPCT = new(5);
        
        /// <summary>
        /// 运行时添加的Tag
        /// </summary>
        private readonly TagCollection _tags = new();

        /// <summary>
        /// 修改固定值
        /// </summary>
        /// <param name="modifyType">修改类型</param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public void Modify(ESkillModifyType modifyType, int param1, int param2 = 0)
        {
            switch (modifyType)
            {
                case ESkillModifyType.SkillLevel:
                    Level += param1;
                    break;
                case ESkillModifyType.SkillAttr:
                    _attrModify[(EAttrType)param1] += param2;
                    break;
                case ESkillModifyType.SkillTag:
                    _tags.Add(param1);
                    break;
                case ESkillModifyType.SkillEnergyCostAdd:
                    _energyCostAdd[param1] += param2;
                    break;
                case ESkillModifyType.SkillEnergyCostPCT:
                    _energyCostPCT[param1] += param2;
                    break;
            }
        }

        public int GetAttrModify(EAttrType attrType)
        {
            return _attrModify.GetValueOrDefault(attrType, 0);
        }

        public int GetEnergyAdd(int energyId)
        {
            return _energyCostAdd.GetValueOrDefault(energyId);
        }

        public float GetEnergyPCT(int energyId)
        {
            return _energyCostPCT.GetValueOrDefault(energyId);
        }

        public void OnRecycle()
        {
            _level = 1;
            _tags.Clear();
            _attrModify.Clear();
        }
    }
}