using Cysharp.Threading.Tasks;

namespace Hono.Scripts.Battle
{
    public interface IAsyncLoad
    {
        public UniTask AsyncLoad();
        public void OnLoadedFinish();
    }
    
    public interface ILoad
    {
        public void Load();
    }

    public partial class ActorModelController
    {
        public abstract class AShowComponent
        {
            protected ActorModelController ActorModelController;

            protected AShowComponent(ActorModelController modelController)
            {
                ActorModelController = modelController;
            }

            public abstract void Init();

            public virtual void OnDestroy() { }

            /// <summary>
            /// 渲染帧
            /// </summary>
            public void Update(float dt)
            {
                onUpdate(dt);
            }

            protected abstract void onUpdate(float dt);
        }
    }

    public partial class ActorLogic
    {
        public abstract class ALogicComponent{
	        public Actor Actor { get;private set; }
	        public ActorLogic ActorLogic  { get;private set; }

            protected ALogicComponent(ActorLogic logic) {
	            Actor = logic.Actor;
	            ActorLogic = logic;
            }

            public abstract void Init();
            public virtual void UnInit() { }

            /// <summary>
            /// 逻辑帧
            /// </summary>
            public void Tick(float dt)
            {
                onTick(dt);
            }

            protected virtual void onTick(float dt) { }
        }
    }
}