namespace Battle
{
    //Actor操作中间层
    public class ActorState
    {
          


        public void init()
        {
                
        }
    }
    
    /// <summary>
    /// 理论上是所有游戏对象的基类
    /// </summary>
    public abstract class NewActor
    {
        //基础数据
        //唯一ID
        public int Uid;

        private ActorShow _show;
        private ActorLogic _logic;
        private ActorShow _state;
        
        
        
        //表演层
        public class ActorShow
        {
            
        }
        
        //逻辑层
        public class ActorLogic
        {//逻辑层包含数据,逻辑流程

            private AttrCollection _attrs;
            private AbilityController _controller;
            public void Load() { }
        }
        
        //生命周期
        protected abstract void onInit();
        public void Initialize()
        {
            
            
            onInit();
        }
        
        protected abstract void onEnterScene();
        public void EnterScene()
        {
            
            onEnterScene();
        }

        protected abstract void OnTick(float dt);
        public void Tick(float dt)
        {
            
            OnTick(dt);
        }

        protected abstract void OnExitScene();
        public void ExitScene()
        {
            
            OnExitScene();
        }
    }
}