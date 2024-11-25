namespace Hono.Scripts.Battle.Event
{
    public class MotionEventInfo : IEventInfo
    {
        public int MotionUid;

        public int MotionCollisionId;

        public void Clear()
        {
            MotionUid = 0;
            MotionCollisionId = 0;
        }
    }
}