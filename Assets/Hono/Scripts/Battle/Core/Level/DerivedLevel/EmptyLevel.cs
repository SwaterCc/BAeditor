using Cysharp.Threading.Tasks;

namespace Hono.Scripts.Battle.Core
{
    public class EmptyLevel : Level
    {
        public override UniTask Load()
        {
            return UniTask.DelayFrame(1);
        }
    }
}