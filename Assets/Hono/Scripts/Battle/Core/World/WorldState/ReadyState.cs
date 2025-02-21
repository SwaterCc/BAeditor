using Hono.Scripts.Battle.Event;

namespace Hono.Scripts.Battle.Core
{
    public partial class WorldInstance
    {
        private class ReadyState : WorldState
        {
            private readonly EventListener _eventListener = new(EEventType.WarBegin);
            public ReadyState(WorldInstance worldInstance) : base(worldInstance, EWorldState.Ready) { }

            protected override void OnEnter()
            {
                if ((EBattleModeType)WorldInstance._sceneRow.BattleType != EBattleModeType.War)
                {
                    WorldInstance._nextState = EWorldState.Gaming;
                    return;
                }

                //打开布阵地图
                UIManager.Instance.SetBattleFieldMap(true);
                EventManager.Instance.AddWorldListener(_eventListener);
            }

            protected override void OnTick(float dt)
            {
                //点击进入游戏后，加载主角，小兵等对象
                
                
                //加载完成后
                WorldInstance._nextState = EWorldState.Gaming;
            }

            protected override void OnExit()
            {
                EventManager.Instance.RemoveWorldListener(_eventListener);
            }
        }
    }
}