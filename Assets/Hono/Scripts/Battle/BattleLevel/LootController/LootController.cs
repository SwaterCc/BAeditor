#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using Random = System.Random;

#endregion

namespace Hono.Scripts.Battle
{
    public class LootController
    {
        private Random _random = new();

        private ELootDropRule _lootDropRule;

        /// <summary>
        ///     规则参数，对应着击杀数量，指定tag，多少秒后
        /// </summary>
        private int _param;

        private LootList _lootList;

        private readonly LootList _default;

        private int _killCount;

        private List<LootSetting> _finalCardList = new(6);
        public List<LootSetting> RougeCardList => _finalCardList;
        public List<int> RougeSkillLearnList => _rougeSkillLearnList;

        private List<int> _rougeSkillLearnList = new()
        {
            999003, 999004, 999007
        };

        public LootController()
        {
            _lootDropRule = ELootDropRule.NoDrop;
            _default = new LootList();
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.SkillLevelUp,
                SkillLevelNum = 1
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.AttrChange,
                AttrType = ELogicAttr.AttrAttackAdd,
                ChangeAttrValue = 40
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.AttrChange,
                AttrType = ELogicAttr.AttrMaxHpPer,
                ChangeAttrValue = 2000
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.AttrChange,
                AttrType = ELogicAttr.AttrCritAdd,
                ChangeAttrValue = 1000
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.AttrChange,
                AttrType = ELogicAttr.AttrCritDamageAdd,
                ChangeAttrValue = 1000
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.AttrChange,
                AttrType = ELogicAttr.AttrSkillCDPCTAdd,
                ChangeAttrValue = 1000
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.AttrChange,
                AttrType = ELogicAttr.AttrMoveSpeedPCTAdd,
                ChangeAttrValue = 1000
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.BuffAdd,
                BuffId = 999203,
                BuffLayer = 1
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.BuffAdd,
                BuffId = 999205,
                BuffLayer = 1
            });
            _default.LootSettings.Add(new LootSetting()
            {
                LootFunctionType = ELootFunctionType.SkillLearn,
            });
        }

        public void SwitchRule(ELootDropRule dropRule, int param)
        {
            _lootDropRule = dropRule;
            _param = param;
        }

        public void SetLootList(ref LootList lootList)
        {
            _lootList = lootList;
        }

        public void CreateLoot(Vector3 pos)
        {
            ActorManager.Instance.CreateLoot((loot) => { loot.SetAttr(ELogicAttr.AttrPosition, pos, false); });
        }

        public void CreateRougeCards(int uid)
        {
            var lootList = _lootList != null && _lootList.LootSettings.Count > 0 ? _lootList : _default;
            _finalCardList.Clear();
            _finalCardList.AddRange(lootList.LootSettings);
            //简单临时处理
            int cardCount = _random.Next(3, 6);
            CommonUtility.Shuffle(ref _finalCardList);
            for (int index = _finalCardList.Count - 1; index > -1; index--)
            {
                if (index > cardCount - 1)
                {
                    _finalCardList.RemoveAt(index);
                }
            }

            ActorManager.Instance.TimeScale = 0;
        }

        public void OnActorDead(Actor actor)
        {
            if (_lootDropRule == ELootDropRule.NoDrop)
                return;

            if (_lootDropRule == ELootDropRule.KillCount)
            {
                ++_killCount;
                if (_killCount == _param)
                {
                    _killCount = 0;
                    CreateLoot(actor.Pos);
                }
            }

            if (_lootDropRule == ELootDropRule.KillSpecialTag)
            {
                if (actor.HasTag(_param))
                {
                    CreateLoot(actor.Pos);
                }
            }
        }
    }
}