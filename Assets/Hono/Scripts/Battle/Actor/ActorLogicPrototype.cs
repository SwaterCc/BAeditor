using System;
using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public class JsonCustomLogicPrototype
    {
        public List<AComponent> Components = new();
    }
    
    public partial class Actor
    {
        public interface IAComponentCtorData { }

        private static class ActorLogicPrototype
        {
            private static readonly Dictionary<string, JsonCustomLogicPrototype> _prototypesData = new();

            //自定义逻辑特殊池
            private static readonly Dictionary<string, Queue<JsonCustomLogic>> _customPool = new();
            
            public static ActorLogic RentLogic(Actor actor)
            {
                ActorLogic logic;
                if (actor.ActorType < EActorType.NoCustomBegin)
                {
                    JsonCustomLogic jsonLogic = null;
                    //Json自定义Logic
                    var jsonKey = actor.ActorTableRow.PrototypeJsonName;
                    if (!_customPool.TryGetValue(jsonKey, out var pool))
                    {
                        jsonLogic = new JsonCustomLogic(jsonKey,_prototypesData[jsonKey]);
                        _customPool.Add(jsonKey, new Queue<JsonCustomLogic>(10));
                    }
                    else
                    {
                        if (!pool.TryDequeue(out jsonLogic))
                        {
                            jsonLogic = new JsonCustomLogic(jsonKey,_prototypesData[jsonKey]);
                        }
                    }
                    logic = jsonLogic;
                }
                else
                {
                    //固定类型
                    switch (actor.ActorType)
                    {
                        case EActorType.BattleLevelController:
                            logic = APool<BattleController>.Pool.Rent();
                            break;
                        case EActorType.Bullet:
                            logic = APool<BulletLogic>.Pool.Rent();
                            break;
                        case EActorType.HitBox:
                            logic = APool<HitBoxLogic>.Pool.Rent();
                            break;
                        case EActorType.Loot:
                            logic = APool<LootLogic>.Pool.Rent();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                logic.Self = actor;
                actor.Logic = logic;

                return logic;
            }
        }
    }
}