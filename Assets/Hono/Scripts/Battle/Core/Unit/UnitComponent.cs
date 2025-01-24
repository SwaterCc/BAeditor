using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle
{
    public abstract class UnitComponent
    {
        private bool _isFirst = true;
        
        public Unit Unit { get; set; }

        public abstract void OnInit();

        public void Tick(float dt)
        {
            if (_isFirst)
            {
                beforeTick();
                _isFirst = false;
            }
            
            onTick(dt);
        }
        
        /// <summary>
        /// 第一次Tick之前运行
        /// </summary>
        protected virtual void beforeTick(){ }

        protected virtual void onTick(float dt) { }

        public void Clear()
        {
            _isFirst = true;
            onClear();
        }
        
        protected abstract void onClear();
    }
}