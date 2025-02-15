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
            /// 当前值()
            /// </summary>
            public int CurrentValue { get; private set; }

            /// <summary>
            /// 最大值上限
            /// </summary>
            public int MaxValue => _energyFields[ECombatEnergyField.MaxValueAdd] * (int)(1 + _energyFields[ECombatEnergyField.MaxValuePCT] / 10000f);
            
            /// <summary>
            /// 攻击回复值最终值
            /// </summary>
            public int EnergyGetWhenSkillHit =>
                _energyFields[ECombatEnergyField.EnergyGetWhenSkillHitAdd] * (int)(1 + _energyFields[ECombatEnergyField.EnergyGetWhenSkillHitPCT] / 10000f);

            /// <summary>
            /// 击杀回复
            /// </summary>
            public int EnergyGetWhenKillEnemy =>
                _energyFields[ECombatEnergyField.EnergyGetWhenKillEnemyAdd] * (int)(1 + _energyFields[ECombatEnergyField.EnergyGetWhenKillEnemyPCT] / 10000f);
            
            /// <summary>
            /// 受击回复值最终值
            /// </summary>
            public int EnergyGetWhenBeHit => _energyFields[ECombatEnergyField.EnergyGetWhenBeHitAdd] * (int)(1 + _energyFields[ECombatEnergyField.EnergyGetWhenBeHitPCT] / 10000f);

            /// <summary>
            /// 自然回复值最终值
            /// </summary>
            public int EnergyGetWhenIdle => _energyFields[ECombatEnergyField.EnergyGetWhenIdleAdd] * (int)(1 + _energyFields[ECombatEnergyField.EnergyGetWhenIdlePCT] / 10000f);

            /// <summary>
            /// 属性字段
            /// </summary>
            private readonly Dictionary<ECombatEnergyField, int> _energyFields = new()
            {
                { ECombatEnergyField.MaxValueAdd, 0 },
                { ECombatEnergyField.MaxValuePCT, 0 },
                { ECombatEnergyField.EnergyGetWhenKillEnemyAdd, 0 },
                { ECombatEnergyField.EnergyGetWhenKillEnemyPCT, 0 },
                { ECombatEnergyField.EnergyGetWhenSkillHitAdd, 0 },
                { ECombatEnergyField.EnergyGetWhenSkillHitPCT, 0 },
                { ECombatEnergyField.EnergyGetWhenBeHitAdd, 0 },
                { ECombatEnergyField.EnergyGetWhenBeHitPCT, 0 },
                { ECombatEnergyField.EnergyGetWhenIdleAdd, 0 },
                { ECombatEnergyField.EnergyGetWhenIdlePCT, 0 },
            };

            public void AddCurrentValue(int value)
            {
                CurrentValue = Mathf.Clamp(CurrentValue + value, 0, MaxValue);
            }


            public void SetField(ECombatEnergyField field, int value)
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