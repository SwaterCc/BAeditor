using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle
{
    public class MonsterGeneratorModel : SceneActorModel
    {
        protected override void onInit()
        {
            ActorType = EActorType.MonsterGenerator;
        }
    }
}