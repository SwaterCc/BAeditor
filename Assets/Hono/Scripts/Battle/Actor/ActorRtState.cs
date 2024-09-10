using UnityEngine;

namespace Hono.Scripts.Battle
{
    //Actor操作中间层
    public class ActorRTState : ITick
    {
        private ActorShow _show;

        private ActorLogic _logic;

        private bool _showTickPause;

        private bool _logicTickPause;

        private AttrCollection _attrs;
        

        public void Setup(ActorShow show, ActorLogic logic,AttrCollection attrCollection) {
	        _show = show;
	        _logic = logic;
	        _attrs = attrCollection;
        }

        public void SetShowPause(bool flag)
        {
            _showTickPause = flag;
        }

        public void SetLogicPause(bool flag)
        {
            _logicTickPause = flag;
        }

        public T GetAttr<T>(int attrId) {
	        return _attrs.GetAttr<T>(attrId);
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
            if (_show == null || _showTickPause) return;
            if(!_show.IsModelLoadFinish) return;
            //更新固定值
            SyncTransform();
            _show.Update(dt);
        }
        
        public void Tick(float dt)
        {
            //确保逻辑层先执行运算
            if (_logic == null || _logicTickPause) return;
            _logic.Tick(dt);
        }
    }
}