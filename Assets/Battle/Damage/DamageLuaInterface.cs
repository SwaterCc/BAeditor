using Battle.Damage;
using Battle.Tools;

namespace Battle.GamePlay
{
    public static class DamageLuaInterface
    {
        public static DamageResults GetDamageResults(ActorLogic attacker,ActorLogic target,DamageInfo damageInfo)
        {
            //调用lua
            
            //处理小公式，计算出最终加伤桶，乘伤桶，减伤桶，随机暴击爆伤
            //...

            //调用大公式
            //...

            return new DamageResults();
        }
    }
}