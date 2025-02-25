using Cysharp.Threading.Tasks;

namespace Hono.Scripts.Battle.Core.DerivedLevel
{
    public class NormalLevel : Level
    {
        public override UniTask Load()
        {
            return UniTask.DelayFrame(1);
        }
    }
}