using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class SkillComp : ALogicComponent
        {
            //管理技能cd
            //管理技能释放前消耗检测
            //管理技能消耗资源
            //管理技能释放打断优先级
            //管理技能升级养成数据
            //技能的内核逻辑依靠Ability
            //Ability可通过该组件获取技能数据

            private Dictionary<int, SkillData> _skillDatas = new Dictionary<int, SkillData>();
            private Dictionary<int, Ability> _skill = new Dictionary<int, Ability>();

            public SkillComp(ActorLogic logic) : base(logic) { }

            public override void Init()
            {
                foreach (var skillId in _actorLogic.LogicData.ownerSkills)
                {
                    var ability = _actorLogic._abilityController.CreateAbility(skillId);
                    
                }
            }

            protected override void onTick(float dt) { }

            public void UseSkill() { }
        }
    }
}