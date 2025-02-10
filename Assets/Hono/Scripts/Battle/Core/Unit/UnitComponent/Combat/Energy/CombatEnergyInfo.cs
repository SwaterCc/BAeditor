using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public partial class CombatComp
    {
        /// <summary>
        /// 战斗资源字段
        /// </summary>
        public class CombatEnergyInfo : IGPoolObject
        {
            /// <summary>
            /// 当前值
            /// </summary>
            public float CurrentValue { get; private set; }

            /// <summary>
            /// 最大值上限
            /// </summary>
            public float MaxValue => _energyFields[ECombatEnergyField.MaxValueAdd] *
                                     _energyFields[ECombatEnergyField.MaxValuePCTPer];

            /// <summary>
            /// 攻击回复值最终值
            /// </summary>
            public float EnergyGetWhenSkillHit => _energyFields[ECombatEnergyField.EnergyGetWhenSkillHitAdd] *
                                                  _energyFields[ECombatEnergyField.EnergyGetWhenSkillHitPCTPer];

            /// <summary>
            /// 受击回复值最终值
            /// </summary>
            public float EnergyGetWhenBeHit => _energyFields[ECombatEnergyField.EnergyGetWhenBeHitAdd] *
                                               _energyFields[ECombatEnergyField.EnergyGetWhenBeHitPCTPer];

            /// <summary>
            /// 自然回复值最终值
            /// </summary>
            public float EnergyGetWhenIdle => _energyFields[ECombatEnergyField.EnergyGetWhenIdleAdd] *
                                              _energyFields[ECombatEnergyField.EnergyGetWhenIdlePCTPer];

            /// <summary>
            /// 属性字段
            /// </summary>
            private readonly Dictionary<ECombatEnergyField, float> _energyFields = new()
            {
                { ECombatEnergyField.MaxValueAdd, 0 },
                { ECombatEnergyField.MaxValuePCTPer, 1 },
                { ECombatEnergyField.EnergyGetWhenSkillHitAdd, 0 },
                { ECombatEnergyField.EnergyGetWhenSkillHitPCTPer, 1 },
                { ECombatEnergyField.EnergyGetWhenBeHitAdd, 0 },
                { ECombatEnergyField.EnergyGetWhenBeHitPCTPer, 1 },
                { ECombatEnergyField.EnergyGetWhenIdleAdd, 0 },
                { ECombatEnergyField.EnergyGetWhenIdlePCTPer, 1 },
            };

            public void AddCurrentValue(float value)
            {
                CurrentValue = Mathf.Clamp(CurrentValue + value, 0, MaxValue);
            }


            public void SetField(ECombatEnergyField field, float value)
            {
                _energyFields[field] += value;
            }

            public void OnRecycle()
            {
                CurrentValue = 0;
                foreach (var pField in _energyFields)
                {
                    _energyFields[pField.Key] = 0;
                }
            }
        }
    }
}