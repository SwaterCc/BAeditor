namespace Battle
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

        /// <summary>
        /// actor类型
        /// </summary>
        public EActorType ActorType;

        /// <summary>
        /// 是否无效
        /// </summary>
        private bool _isDisposable = false;
        
        public bool IsDisposable => _isDisposable;

        /// <summary>
        /// 表现层
        /// </summary>
        private ActorShow _show;

        /// <summary>
        /// 逻辑层
        /// </summary>
        private ActorLogic _logic;

        public ActorLogic ActorLogic => _logic;

        /// <summary>
        /// 当前运行状态
        /// </summary>
        private ActorRTState _rtState;

        /// <summary>
        /// 运行状态
        /// </summary>
        public ActorRTState RTState => _rtState;

        public Actor(int uid, EActorType actorType)
        {
            Uid = uid;
            ActorType = actorType;
        }

        //生命周期
        public void Init(ActorShow show, ActorLogic logic)
        {
            _show?.Init();
            _logic?.Init();
            _rtState = new ActorRTState(show, logic);
        }

        public void AwardAbility(int configId, bool isRunNow)
        {
            _logic?.AbilityController.AwardAbility(Uid, configId, isRunNow);
        }

        public void Destroy() { }
    }
}