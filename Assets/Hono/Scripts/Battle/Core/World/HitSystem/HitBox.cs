using System.Collections.Generic;
using Hono.Scripts.Battle.Event;
using Unity.VisualScripting;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    
    //仅Actor对象存在父子关系
    
    public class HitBox : WorldNode
    {
        //什么是hitbox？
        //本质是一次检测 + 伤害计算的流程
        //Hit的配置和类型决定了如何检测
        //damage则是有单独的计算
        
        //HitBox 规定了检测方式
        //检测多久，检测几次，比如我有个火焰会对范围内的敌人造成多次伤害，多次来自与配置
        //我发起了一次攻击，这个攻击是否命中，命中后是否产生伤害，HitBox是一个独立的对象，决定产生几次命中检测
        //看起来hitBox更像是一个单独的机制
        //但是，碰撞伤害如何实现？
        //如果我车辆运行时创建了一个绑定打击盒，我车辆停止时 -死亡对象不允许被命中
        //CreateHitBox -> Tick -> Hit -> 命中存活目标 -> damage 传入 攻击者属性， 来源数据（技能，buff,子弹），命中对象
        
        //攻击盒子变大，本质是下次检测的缩放值发生了变化
        //HitBox是否应该是一个Unit？应该不是
        
        //子弹呢，子弹是一个Unit吗，是，HitBox不是是因为他更像一个流程，子弹则是一个单独的对象
        //邻域同理
        
        //是否需要状态机？不是很需要
        //
        //skill 执行时限制输入？蓄力技能的实现?技能分阶段 ability依然需要知道开始结束,把事件单独开一个周期，理解起来更清晰一些
        
        //在拥有了地图数据后并不需要使用射线检测来处理命中，直接使用地图块检索就好
        //速度方向移动是一种行为委托，地图每帧获取速度向量，更新单位的坐标
        
        //攻击 ：直接对目标产生伤害   -> single
        //      对目标周围面积产生伤害 -> aoe
       
        private HitBoxData _hitBoxData;

        private Actor _attacker;
        private Actor _target;

        private float _intervalDuration;
        private int _curCount;

        private bool _isExpire;
        private readonly Damage _damage;
        private RangeFilterSetting _rangeFilterSetting;
        private List<int> _aoeTargetIds = new(32);
        private readonly Dictionary<BeHurtComp, int> _hitCountDict = new(16);
        private readonly List<BeHurtComp> _hurtComps = new(32);

        public HitBox()
        {
            _damage = new Damage(this);
        }

        public void Init(Actor attacker, Vector3 target, HitBoxData hitBoxData)
        {
            _hitBoxData = hitBoxData;
            //因为是同帧，所以攻击者必然存在
            _attacker = Battle.UnitManager.Instance.GetActor(GetAttr(EAttrType.AttrSourceActorUid));
            _attacker.ExitSceneCallBack += setHitBoxExpire;
            //打击目标
            var targetUid = Variables.Get<int>("targetUid");
            if (targetUid != 0)
            {
                _target = Battle.UnitManager.Instance.GetActor(targetUid);
            }

            if (_target != null)
            {
                _target.ExitSceneCallBack += setHitBoxExpire;
            }

            _isExpire = _target == null;

            if (!_isExpire)
            {
                _damage.Init(_attacker, _target, _hitBoxData.DamageConfigId);
            }

            _curCount = 0;
            _intervalDuration = _hitBoxData.Interval;
            _rangeFilterSetting = _hitBoxData.rangeFilterSetting;
            Self.Pos = _target.Pos;
            Self.Rot = _attacker.Rot;
        }

        protected override void onTick(float dt)
        {
            if (_isExpire) return;

            var interval = _curCount == 0 ? _hitBoxData.FirstInterval : _hitBoxData.Interval;

            if (_intervalDuration > interval)
            {
                ++_curCount;
                _intervalDuration = 0;
                onHit();
            }

            _intervalDuration += dt;

            if (_curCount >= _hitBoxData.MaxCount)
            {
                Battle.UnitManager.Instance.RemoveActor(Uid);
            }
        }

        protected override void onRemove()
        {
            _hitBoxData = null;

            _attacker = null;
            _target = null;

            _intervalDuration = 0;
            _curCount = 0;

            _isExpire = false;

            _rangeFilterSetting = null;
            _aoeTargetIds.Clear();
            _hitCountDict.Clear();
            _hurtComps.Clear();
            _damage.Clear();
        }

        private void onHit()
        {
            for (int i = 0; i < _hitBoxData.OnceHitDamageCount; i++)
            {
                switch (_hitBoxData.HitType)
                {
                    case EHitType.Aoe:
                        aoeHit();
                        break;
                    case EHitType.Single:
                        singleHit();
                        break;
                }
            }
        }

        private void setHitBoxExpire(Actor actor)
        {
            _isExpire = true;
            Battle.UnitManager.Instance.RemoveActor(Uid);
        }

        private void singleHit()
        {
            if (!_target.Logic.TryGetComponent(out BeHurtComp beHurtComp)) return;
            hitCounter(beHurtComp);
            var hitDamageInfo = _damage.MakeDamage(1, _hitCountDict[beHurtComp], _hitBoxData.CriticalFlag);
            var vb = APool<VariableBoard>.Pool.Rent();
            vb.InitByHitDamageInfo(hitDamageInfo);
            EventManager.Instance.FireEvent(EEventType.OnHit, _attacker.Uid, vb);
            APool<VariableBoard>.Pool.Recycle(vb);
            beHurtComp.OnBeHurt(hitDamageInfo);
        }

        private void aoeHit()
        {
            //aoe会根据目标坐标二次筛选
            Battle.UnitManager.Instance.UseFilter(Self, _rangeFilterSetting, ref _aoeTargetIds);

            if (_aoeTargetIds.Count == 0)
            {
                Battle.UnitManager.Instance.RemoveActor(Uid);
                return;
            }

            foreach (var targetUid in _aoeTargetIds)
            {
                var target = Battle.UnitManager.Instance.GetActor(targetUid);
                if (target == null)
                    continue;
                if (!target.Logic.TryGetComponent<BeHurtComp>(out var beHurtComp))
                    continue;
                _hurtComps.Add(beHurtComp);
            }

            foreach (var beHurtComp in _hurtComps)
            {
                hitCounter(beHurtComp);
                var hitDamageInfo = _damage.MakeDamage(_hurtComps.Count, _hitCountDict[beHurtComp], _hitBoxData.CriticalFlag);
                var vb = APool<VariableBoard>.Pool.Rent();
                vb.InitByHitDamageInfo(hitDamageInfo);
                EventManager.Instance.FireEvent(EEventType.OnHit, _attacker.Uid, vb);
                APool<VariableBoard>.Pool.Recycle(vb);
                beHurtComp.OnBeHurt(hitDamageInfo);
            }
        }

        private void hitCounter(BeHurtComp beHurtComp)
        {
            if (_hitCountDict.TryGetValue(beHurtComp, out var count))
            {
                if (count < _hitBoxData.ValidCount)
                {
                    ++count;
                }
            }
            else
            {
                _hitCountDict.Add(beHurtComp, 1);
            }
        }

        public override void Recycle()
        {
            APool<HitBoxLogic>.Pool.Recycle(this);
        }
    }
}