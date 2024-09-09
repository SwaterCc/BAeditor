namespace Hono.Scripts.Battle
{
    public interface ITick
    {
        public void Tick(float dt);
    }

    /// <summary>
    /// Actor 实际的行为由Logic+Show构成，用State去管理Logic和Show Actor提供对外的接口
    /// </summary>
    public sealed class Actor
    {
        /// <summary>
        /// 运行时唯一ID
        /// </summary>
        public int Uid { get; }

        /// <summary>
        /// 配置ID
        /// </summary>
        private int _configId;

        public int ConfigId => _configId;

        /// <summary>
        /// 是否无效
        /// </summary>
        private bool _isDisposable = false;

        public bool IsDisposable => _isDisposable;

        /// <summary>
        /// Actor基础数据
        /// </summary>
        public ActorPrototypeTable.ActorPrototypeRow PrototypeData { get; }

        /// <summary>
        /// 表现层
        /// </summary>
        private ActorShow _show;

        /// <summary>
        /// 逻辑层
        /// </summary>
        private ActorLogic _logic;

        public ActorLogic Logic => _logic;

        /// <summary>
        /// 当前运行状态
        /// </summary>
        private ActorRTState _rtState;

        public Actor(int uid)
        {
            Uid = uid;
        }

        public void Setup(ActorShow show, ActorLogic logic)
        {
            _rtState = new ActorRTState(show, logic);
            _logic = logic;
            _show = show;
        }

        public void Init()
        {
            _show?.Init();
            _logic?.Init(_rtState);

            _rtState.SyncTransform();
        }

        public void Tick(float dt)
        {
            _rtState.Tick(dt);
        }
        
        public void Update(float dt)
        {
            _rtState.Update(dt);
        }
        
        public void Destroy()
        {
            _show.Destroy();
            _logic.Destroy();
        }
    }
}