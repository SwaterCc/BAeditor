using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public class JsonCustomLogic : ActorLogic
    {
        public readonly string JsonKey;

        public JsonCustomLogic(string jsonKey, JsonCustomLogicPrototype prototype)
        {
            JsonKey = jsonKey;
            foreach (var component in prototype.Components)
            {
                addComponent(component.Clone());
            }
        }

        protected override void onInit() { }

        public override void Recycle()
        {
            //特殊池回收
        }
    }
}