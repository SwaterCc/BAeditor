using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public class HitBoxLogic : ActorLogic
    {
        private HitBoxData _hitBoxConfig;
        private float _duration;
        private float _effectDuration;
        private float _intervalDuration;
        private bool _isInEffect;
        private int _count;
        private Dictionary<BeHurtComp, int> _hitCountDict = new();
        private DamageLuaInterface _luaInterface;
        public HitBoxLogic(int uid, ActorLogicData logicData) : base(uid, logicData) { }

        protected override void onInit()
        {
            _duration = 0;
            _intervalDuration = _hitBoxConfig.Interval;
            _count = 0;
            _isInEffect = false;
            _luaInterface = new DamageLuaInterface();
            _luaInterface.Init();
        }

        protected override void registerChildComp() { }

        private List<int> getHitTargets()
        {
            //TODO:要接地图块，筛选器
            switch (_hitBoxConfig.HitType)
            {
                case EHitType.Aoe:

                    break;
                case EHitType.LockTarget:
                    break;
            }

            return null;
        }

        private void hit()
        {
            List<int> targets = getHitTargets();
            if (targets == null) return;

            var damageInfo = new DamageInfo();
            var source = GetAttr<int>(ELogicAttr.Source);
            var attacker = ActorManager.Instance.GetActor(source).ActorLogic;
            damageInfo.DamageConfigId = _hitBoxConfig.DamageConfigId;
            damageInfo.SourceId = source;
            damageInfo.SourceType = GetAttr<int>(ELogicAttr.SourceType);

            foreach (var target in targets)
            {
                var targetLogic = ActorManager.Instance.GetActor(target).ActorLogic;
                var behitComp = targetLogic.GetComponent<BeHurtComp>();
                if (_hitCountDict.TryGetValue(behitComp, out var count))
                {
                    if (count >= _hitBoxConfig.ValidCount)
                    {
                        continue;
                    }
                }
                else
                {
                    _hitCountDict.Add(behitComp, 1);
                }

                //TODO:要接配置
                var res = _luaInterface.GetDamageResults(attacker, targetLogic, damageInfo, new DamageItem());
                behitComp.OnBeHurt(res);
            }
        }

        protected override void onTick(float dt)
        {
            //有效时间
            if (_duration >= _hitBoxConfig.DelayTime && _duration <= _hitBoxConfig.DelayTime + _hitBoxConfig.Duration)
            {
                if (_intervalDuration >= _hitBoxConfig.Interval)
                {
                    _intervalDuration = 0;
                    _effectDuration = 0;
                    ++_count;
                    _isInEffect = true;
                }

                if (_isInEffect)
                {
                    if (_effectDuration <= _hitBoxConfig.EffectTime && _count <= _hitBoxConfig.ValidCount)
                    {
                        _effectDuration += dt;
                        hit();
                    }
                    else
                    {
                        _isInEffect = false;
                    }
                }

                if (!_isInEffect)
                {
                    _intervalDuration += dt;
                }
            }

            _duration += dt;

            if (_duration >= _hitBoxConfig.Duration + _hitBoxConfig.DelayTime) { }
        }

        protected override void onDestroy() { }
    }
}