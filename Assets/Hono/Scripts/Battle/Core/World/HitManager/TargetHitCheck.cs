namespace Hono.Scripts.Battle.Core
{
    public class TargetHitCheck : HitCheck
    {
        private void singleHit()
        {
            if (!_target.Logic.TryGetComponent(out BeHurtComp beHurtComp)) return;
            hitCounter(beHurtComp);
            var hitDamageInfo = _damage.MakeDamage(1, _hitCountDict[beHurtComp], _hitBoxData.CriticalFlag);
            var vb = GPool<VariableBoard>.Pool.Rent();
            vb.InitByHitDamageInfo(hitDamageInfo);
            EventManager.Instance.FireEvent(EEventType.OnHit, _attacker.Uid, vb);
            GPool<VariableBoard>.Pool.Recycle(vb);
            beHurtComp.OnBeHurt(hitDamageInfo);
        }
    }
}