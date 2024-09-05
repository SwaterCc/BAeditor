using UnityEngine;

namespace Hono.Scripts.Battle
{
    //Actor操作中间层
    public class ActorRTState : ITick
    {
        private Actor _actor;
        
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
            SetShow(show, showTickPause);
            SetLogic(logic, logicTickPause);
        }

        public void SetShow(ActorShow show, bool tickPause = false)
        {
            _showTickPause = tickPause;
            _hasShow = show != null;
            if (!_hasShow) return;
            _show = show;
        }

        public void SetLogic(ActorLogic logic, bool tickPause = false)
        {
            _logicTickPause = tickPause;
            _hasLogic = logic != null;
            if (!_hasLogic) 
                return;
            _logic = logic;
        }

        public void SetShowPause(bool flag)
        {
            _showTickPause = flag;
        }

        public void SetLogicPause(bool flag)
        {
            _logicTickPause = flag;
        }

        public void SetShowAttr()
        {
            
        }

        public void SetLogicAttr() {
	        
        }

        public void SyncTransform()
        {
            var pos = _logic.GetAttr<Vector3>(ELogicAttr.AttrPosition);
            var rot = _logic.GetAttr<Quaternion>(ELogicAttr.AttrRot);

            if (_show.Model != null)
            {
                _show.Model.transform.localPosition = pos;
                _show.Model.transform.localRotation = rot;
            }
        }
        
        public void Update(float dt)
        {
            if (!_hasShow || !_showTickPause) return;
            
            //更新固定值
            SyncTransform();
            _show.Update(dt);
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