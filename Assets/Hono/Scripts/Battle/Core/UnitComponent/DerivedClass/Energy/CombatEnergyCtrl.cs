using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public partial class CombatComp
    {
        /// <summary> 
        /// 战斗资源控制
        /// </summary>
        public class CombatEnergyCtrl
        {
            /// <summary>
            /// 快速索引
            /// </summary>
            private readonly Dictionary<int, CombatEnergyInfo> _combatEnergy = new(10);

            /// <summary>
            /// 
            /// </summary>
            private readonly Unit _unit;
            private readonly UnitEventListener _attackListener = new(EEventType.OnSkillUseSuccess);
            private readonly UnitEventListener _beHitListener = new(EEventType.OnBeHit);
            private readonly UnitEventListener _killEnemyListener = new(EEventType.OnHit, true);

            public CombatEnergyCtrl(Unit unit)
            {
                _unit = unit;
                _attackListener.SetCallback(onAttack);
                _beHitListener.SetCallback(onBeHit);
            }

            public void Init()
            {
                _unit.RegisterEvtListener(_attackListener);
                _unit.RegisterEvtListener(_beHitListener);
            }

            public void Tick(float dt)
            {
                foreach (var energyInfo in _combatEnergy.Values)
                {
                    energyInfo.AddCurrentValue(energyInfo.EnergyGetWhenIdle);
                }
            }

            public void Clear()
            {
                _unit.UnregisterEvtListener(_attackListener);
                _unit.UnregisterEvtListener(_beHitListener);
            }

            /// <summary>
            /// 添加新的能量类型
            /// </summary>
            /// <param name="energyTypeId"></param>
            public void AddEnergyType(int energyTypeId)
            {
                //读表设置初始值
                if (!ConfigDataBase.Table<CombatEnergyTable>().TryGet(energyTypeId, out var energyRow))
                {
                    Debug.LogError($"找不到对应的资源类型{energyTypeId}");
                    return;
                }

                var info = GPool<CombatEnergyInfo>.Pool.Rent();

                info.SetField(ECombatEnergyField.CurrentValue,             energyRow.InitValue);
                info.SetField(ECombatEnergyField.EnergyGetWhenSkillHitAdd, energyRow.AttackGetValue);
                info.SetField(ECombatEnergyField.EnergyGetWhenBeHitAdd,    energyRow.BehitGetValue);
                info.SetField(ECombatEnergyField.EnergyGetWhenIdleAdd,     energyRow.IdleGetValue);
                info.SetField(ECombatEnergyField.MaxValueAdd,              energyRow.EnergyMax);

                _combatEnergy.Add(energyTypeId, info);
            }

            /// <summary>
            /// 删除指定类型的能量类型
            /// </summary>
            /// <param name="energyTypeId"></param>
            public void RemoveEnergyType(int energyTypeId)
            {
                if (_combatEnergy.TryGetValue(energyTypeId, out var energyInfo))
                {
                    GPool<CombatEnergyInfo>.Pool.Recycle(energyInfo);
                    _combatEnergy.Remove(energyTypeId);
                }
            }

            /// <summary>
            ///  修改指定能量值
            /// </summary>
            /// <param name="energyTypeId"></param>
            /// <param name="fieldType"></param>
            /// <param name="value"></param>
            public void SetEnergyValue(int energyTypeId, ECombatEnergyField fieldType, int value)
            {
                if (!_combatEnergy.TryGetValue(energyTypeId, out var energyInfo))
                {
                    return;
                }

                if (fieldType == ECombatEnergyField.CurrentValue)
                {
                    energyInfo.AddCurrentValue(value);
                }
                else
                {
                    energyInfo.SetField(fieldType, value);
                }
            }

            /// <summary>
            /// 检测某个能量是否足够
            /// </summary>
            /// <param name="id"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool CheckEnergyEnough(int id, float value)
            {
                if (!_combatEnergy.TryGetValue(id, out var energyInfo))
                {
                    return false;
                }

                return energyInfo.CurrentValue >= value;
            }

            /// <summary>
            /// 消耗指定能量
            /// </summary>
            /// <param name="id"></param>
            /// <param name="value"></param>
            public void CostEnergy(int id, int value)
            {
                if (!_combatEnergy.TryGetValue(id, out var energyInfo))
                {
                    Debug.LogError("该能量不存在，扣除失败");
                    return;
                }

                energyInfo.AddCurrentValue(-value);
            }

            private void onAttack(VariableBoard board)
            {
                foreach (var energyInfo in _combatEnergy.Values)
                {
                    energyInfo.AddCurrentValue(energyInfo.EnergyGetWhenSkillHit);
                }
            }

            private void onBeHit(VariableBoard board)
            {
                foreach (var energyInfo in _combatEnergy.Values)
                {
                    energyInfo.AddCurrentValue(energyInfo.EnergyGetWhenBeHit);
                }
            }
        }
    }
}