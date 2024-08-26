namespace Battle
{
    //Actor操作中间层
    public class ActorRTState : ITick
    {
        private ActorShow _show;

        private ActorLogic _logic;

        private bool _hasShow;

        private bool _hasLogic;

        private bool _isShowLoadFinish;

        private bool _isLogicLoadFinish;

        private bool _showTickPause;

        private bool _logicTickPause;

        public ActorRTState() : this(null, null, false, false) { }

        public ActorRTState(ActorLogic logic, bool logicTickPause = false) :
            this(null, logic, false, logicTickPause) { }

        public ActorRTState(ActorShow show, ActorLogic logic, bool showTickPause = false, bool logicTickPause = false)
        {
            _hasShow = show == null;
            _show = show;
            _showTickPause = showTickPause && _hasShow;

            _hasLogic = logic != null;
            _logic = logic;
            _logicTickPause = logicTickPause && _hasLogic;
        }

        public void SetShow(ActorShow show, bool tickPause = false)
        {
            if (show == null) return;
            _show ??= show;
            _showTickPause = tickPause;
        }

        public void SetLogic(ActorLogic logic, bool tickPause = false)
        {
            if (logic == null) return;
            _logic ??= logic;
            _logicTickPause = tickPause;
        }

        public void SetShowPause(bool flag)
        {
            _showTickPause = flag;
        }

        public void SetLogicPause(bool flag)
        {
            _logicTickPause = flag;
        }

        public void OnEnterScene()
        {
            
        }

        public void OnExitScene()
        {
            
        }

        public void SetShowAttr()
        {
            
        }

        public void SetLogicAttr()
        {
            
        }
        
        public void Update(float dt)
        {
            if (_hasShow && _showTickPause)
            {
                _show.Update(dt);
            }
        }
        
        public void Tick(float dt)
        {
            //确保逻辑层先执行运算
            if (_hasLogic && _logicTickPause)
            {
                _logic.Tick(dt);
            }
        }
    }
}