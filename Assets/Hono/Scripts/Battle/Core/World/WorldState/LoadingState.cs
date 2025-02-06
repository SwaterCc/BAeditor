namespace Hono.Scripts.Battle.Core
{
    public partial class World
    {
        private class LoadingState : WorldState
        {
            public LoadingState(World world) : base(world, EWorldState.Loading) { }
            protected override void OnEnter()
            {
                //进入加载场景
            }

            protected override void OnTick(float dt)
            {
               //加载地图
               //创建地图对象
               //任务流加载
               //加载完成后进入准备状态
               World._nextState = EWorldState.Ready;
            }

            protected override void OnExit()
            {
                //退出加载场景
            }
        }
    }
}