using Cysharp.Threading.Tasks;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 战争关卡
    /// </summary>
    public class WarLevel : Level
    {
        public override UniTask Load()
        {
            //加载关卡数据
            //场景对象列表
            //任务列表


            return UniTask.DelayFrame(1);
        }
    }
}