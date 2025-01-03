#region

using Hono.Scripts.Battle.Event;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class MpComp : AComponent
        {
            private UseSkillChecker _useSkillChecker;
            private HitEventChecker _hitEventChecker;
            private HitEventChecker _beHitEventChecker;

            private const int MpRecCastBase = 180;
            private const int MpRecBehit = 1000;
            private const int MpRecKilled = 400;

            public MpComp(ActorLogic logic) : base(logic) { }

            public override void Init()
            {
                _useSkillChecker =
                    new UseSkillChecker(EBattleEventType.OnSkillUseSuccess, Self, -1, AttackChangeMp);
                _hitEventChecker = new HitEventChecker(EBattleEventType.OnHit,     Self, -1, -1, KillChangeMp);
                _beHitEventChecker = new HitEventChecker(EBattleEventType.OnBeHit, Self, -1, -1, BeHitChangeMp);

                _useSkillChecker.Register();
                _beHitEventChecker.Register();
                _hitEventChecker.Register();
            }

            public override void Clear()
            {
                _useSkillChecker.UnRegister();
                _beHitEventChecker.UnRegister();
                _hitEventChecker.UnRegister();
            }

            private void AttackChangeMp(IEventInfo info)
            {
                var useSkillInfo = (UsedSkillEventInfo)info;
                var skillData = AssetManager.Instance.GetData<SkillData>(useSkillInfo.SkillId);
                if (skillData == null) return;
                if (skillData.skillType == ESkillType.UltimateSkill) return;

                var maxMp = Self.GetAttr(EAttrType.AttrMaxMp);
                var curMp = Self.GetAttr(EAttrType.AttrMp);
                var mpRecCastAdd = Self.GetAttr(EAttrType.AttrMpRecCastAdd);
                var mpRecCastPer = Self.GetAttr(EAttrType.AttrMpRecCastPer);
                var allRecAdd = Self.GetAttr(EAttrType.AttrMpRecAllAdd);
                var value = ((MpRecCastBase + mpRecCastAdd) * (10000 + mpRecCastPer) / 10000) *
                            ((10000 + allRecAdd) / 10000);
                curMp = Mathf.Clamp(curMp + value, 0, maxMp);
                Self.SetAttr(EAttrType.AttrMp, curMp, false);
            }

            private void BeHitChangeMp(IEventInfo info)
            {
                var hitDamageInfo = (HitDamageInfo)info;
                if (hitDamageInfo.FinalDamageValue < 0) return;
                var maxMp = Self.GetAttr(EAttrType.AttrMaxMp);
                var curMp = Self.GetAttr(EAttrType.AttrMp);
                var maxHp = Self.GetAttr(EAttrType.AttrMaxHp);

                var mpRecBehitAdd = Self.GetAttr(EAttrType.AttrMpRecBehitAdd);
                var mpRecBehitPer = Self.GetAttr(EAttrType.AttrMpRecBehitPer);
                var allRecAdd = Self.GetAttr(EAttrType.AttrMpRecAllAdd);
                var baseValue = (int)((hitDamageInfo.FinalDamageValue / maxHp) * MpRecBehit);
                var value = ((baseValue + mpRecBehitAdd) * (10000 + mpRecBehitPer) / 10000) *
                            ((10000 + allRecAdd) / 10000);
                curMp = Mathf.Clamp(curMp + value * 5, 0, maxMp);
                Self.SetAttr(EAttrType.AttrMp, curMp, false);
            }

            public void KillChangeMp(IEventInfo info)
            {
                var hitDamageInfo = (HitDamageInfo)info;
                if (!hitDamageInfo.IsKillTarget) return;

                var maxMp = Self.GetAttr(EAttrType.AttrMaxMp);
                var curMp = Self.GetAttr(EAttrType.AttrMp);
                var mpRecBehitAdd = Self.GetAttr(EAttrType.AttrMpRecKilledAdd);
                var mpRecBehitPer = Self.GetAttr(EAttrType.AttrMpRecKilledPer);
                var allRecAdd = Self.GetAttr(EAttrType.AttrMpRecAllAdd);
                var value = (MpRecKilled + mpRecBehitAdd) * ((10000 + mpRecBehitPer) / 10000) *
                            ((10000 + allRecAdd) / 10000);
                curMp = Mathf.Clamp(curMp + value, 0, maxMp);
                Self.SetAttr(EAttrType.AttrMp, curMp, false);
            }
        }
    }
}