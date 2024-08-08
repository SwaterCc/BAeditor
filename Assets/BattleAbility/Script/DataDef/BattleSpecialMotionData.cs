using Battle.Def;

namespace BattleAbility
{
    /// <summary>
    /// 通用位移组件，可以移动指定对象
    /// </summary>
    public class BattleSpecialMotionData
    {
        public int MotionDataUid;
        
        public EBattleSpecialMotionType MotionType;
        
        public long BeginTime;
        
        public long Duration;
        
        public long EndTime;
    }
    
    
    
}