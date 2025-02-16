using System.Collections.Generic;
using Hono.Scripts.Battle.Core;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Event;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public class HpCompCtorParams : UnitCompCtorParams
    {
        /// <summary>
        /// 能否被选中
        /// </summary>
        public bool CanBeSelect;
        /// <summary>
        /// 允许受伤
        /// </summary>
        public bool AllowBeHurt;
    }

    /// <summary>
    /// 生命值管理组件，负责处理当前生命值变化
    /// </summary>
    public class HpComp : UnitComponent,IGPoolObject
    {
        private struct HpLock
        {
            public int Id;
            public float HpPCT;
            public bool TriggerEvent;
        }

        private int _currentHp;
        private int _lockCount = 1;
        private readonly List<HpLock> _hpLocks = new(5);
        private readonly DamagePipLine _damagePipLine = new();

        public bool IsAlive => _currentHp > 0;

        public override void Init()
        {
            //初始化血量等于最大HP
            _currentHp = Unit.GetAttr(EAttrType.AttrMaxHp);
            Unit.SetAttr(EAttrType.AttrHp, _currentHp);
        }

        protected override void onTick(float dt)
        {
            if (_currentHp <= 0)
            {
                //单位死亡,从父节点删除
                World.Current.RemoveUnit(Unit);
                //死亡特效播放
            }
        }

        public override void Recycle()
        {
            GPool<HpComp>.Pool.Recycle(this);
        }

        /// <summary>
        /// 修改血量
        /// </summary>
        private void modifyHp(int value)
        {
            var maxHp = Unit.GetAttr(EAttrType.AttrMaxHp);
            _currentHp = Mathf.Clamp(_currentHp + value, 0, maxHp);

            foreach (var hpLock in _hpLocks)
            {
                var curPercent = _currentHp / (float)maxHp;

                if (!(curPercent < hpLock.HpPCT))
                    continue;
                //触发锁血
                _currentHp = (int)(maxHp * hpLock.HpPCT);
                if (!hpLock.TriggerEvent)
                    continue;
                var board = GPool<VariableBoard>.Pool.Rent();
                board.Set(HpLockEventInfo.HpLockId, hpLock.Id);
                Unit.FireEvent(EEventType.OnAttrChanged, board);
                GPool<VariableBoard>.Pool.Recycle(board);
                break;
            }

            Unit.SetAttr(EAttrType.AttrHp, _currentHp);
        }

        /// <summary>
        ///  添加血量锁
        /// </summary>
        /// <param name="hpPCTPer">血量锁万分比</param>
        /// <param name="triggerEvent">到达血量锁的时候是否触发事件</param>
        /// <returns>返回锁的识别id</returns>>
        public int AddHpLock(int hpPCTPer, bool triggerEvent = false)
        {
            HpLock hpLock = new HpLock
            {
                Id = _lockCount++,
                HpPCT = hpPCTPer / 10000f,
                TriggerEvent = triggerEvent
            };
            _hpLocks.Add(hpLock);
            return hpLock.Id;
        }

        public void RemoveHpLock(int id)
        {
            for (int i = 0; i < _hpLocks.Count; i++)
            {
                if (_hpLocks[i].Id != id)
                    continue;
                _hpLocks.RemoveAt(i);
                break;
            }
        }

        /// <summary>
        /// 执行伤害流程
        /// </summary>
        public void MakeDamage(HitInfo hitInfo)
        {
            _damagePipLine.Init(hitInfo);
            LuaBridge.Instance.CalcDamageResults(hitInfo.Attacker, Unit, _damagePipLine);
            beHurt(_damagePipLine.GetDamageResult().DamageValue, (EDamageType)hitInfo.DamageRow.DamageType);
        }

        /// <summary>
        /// 造成固定伤害
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageType"></param>
        public void MakeFixDamageValue(int damageValue, EDamageType damageType)
        {
            beHurt(damageValue, damageType);
        }

        /// <summary>
        /// 受伤扣血流程
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageType"></param>
        private void beHurt(int damageValue, EDamageType damageType)
        {
            //伤害是负值
            //回血是正值

            //FIX:目前还是反的
            var curShield = Unit.GetAttr(EAttrType.AttrShield);
            switch (damageType)
            {
                case EDamageType.Normal:
                case EDamageType.Percent:
                case EDamageType.Dot:
                    if (Unit.GetAttr(EAttrType.AttrInvincible) > 0)
                        return;

                    var lastShield = curShield + damageValue;
                    if (lastShield > 0)
                    {
                        //护盾抗住了伤害
                        curShield = lastShield;
                        //播放护盾抵挡特效
                        playHurtVFX("beHurt");
                    }
                    else
                    {
                        var lastDamageValue = -lastShield;
                        curShield = 0;
                        modifyHp(lastDamageValue);
                        //播放受击特效
                        playHurtVFX("beHurt");
                    }

                    break;
                case EDamageType.Health:
                    modifyHp(damageValue);
                    break;
            }

            Unit.SetAttr(EAttrType.AttrShield, curShield);

            Debug.Log($"当前血量{Unit.GetAttr(EAttrType.AttrHp)}");
        }

        private void playHurtVFX(string key)
        {
            //通过组件内部事件通知特效组件
            if (Unit.TryGetComponent(out VFXComp vfxComp))
            {
                vfxComp.AddVFXByKey(key,new VFXSetting()
                {
                    isWorldVFX = true,
                    duration = 3
                });
            }
        }
        
        public void OnRecycle()
        {
            _currentHp = 0;
            _hpLocks.Clear();
        }
    }
}