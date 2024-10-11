using Cysharp.Threading.Tasks;

namespace Hono.Scripts.Battle
{
    public class StaticActorShow : ActorShow
    {
        public StaticActorShow(Actor actor) : base(actor) { }

        protected override UniTask loadModel()
        {
            return base.loadModel();
        }
    }
}