using Hono.Scripts.Battle.Core;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public class ElementLink
    {
        private readonly EAttrType _elementAtk;
        private readonly EAttrType _elementHp;
        private readonly EAttrType _elementMaxHp;
        private readonly int _mainBuff;
        private int _linkTag;

        /// <summary>
        /// 元素绑定
        /// </summary>
        /// <param name="elementAtk"></param>
        /// <param name="elementHp"></param>
        /// <param name="elementMaxHp"></param>
        /// <param name="mainBuff"></param>
        public ElementLink(EAttrType elementAtk, EAttrType elementHp, EAttrType elementMaxHp, int mainBuff)
        {
            _elementAtk = elementAtk;
            _elementHp = elementHp;
            _elementMaxHp = elementMaxHp;
            _mainBuff = mainBuff;
        }

        public void Process(UnitProxy attacker, CombatComp target, DamageTable.DamageRow damageRow)
        {
            //该伤害是否为对应元素
            if (damageRow.ElementsDamage[0] != _linkTag)
            {
                return;
            }

            var atk = attacker.GetAttr(_elementAtk) + damageRow.ElementsDamage[1];
            var hp = target.Unit.GetAttr(_elementHp);
            var maxHp = target.Unit.GetAttr(_elementMaxHp);

            var per = 1;

            //异常期间不累计异常
            if (target.HasBuff(_mainBuff))
            {
                atk = 0;
            }

            atk += per;

            var lastHp = hp - atk;
            if (lastHp <= 0 && !damageRow.DisableElementBoom)
            {
                Debug.LogError($"element {_elementHp} boom");
                target.AddBuff(attacker.Uid, _mainBuff);
                target.Unit.SetAttr(_elementHp, maxHp);
            }
            else
            {
                Debug.LogError($"element {_elementHp} -> {lastHp}");
                target.Unit.SetAttr(_elementHp, Mathf.Min(lastHp, 0));
            }
        }
    }
}