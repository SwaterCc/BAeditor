namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 脱手打击盒
    /// </summary>
    public class HitBox : Unit, IGPoolObject
    {
        public void Init(Unit attack, HitBoxParams hitBoxParams)
        {
            
        }
        
        protected override void onTick(float dt)
        {
           
        }

      
        public override void Recycle()
        {
            GPool<HitBox>.Pool.Recycle(this);
        }

        public void OnRecycle()
        {
           
        }
    }
}