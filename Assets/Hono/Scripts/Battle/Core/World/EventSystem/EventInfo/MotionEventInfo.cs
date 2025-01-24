namespace Hono.Scripts.Battle.Event
{
    public static class MotionEventInfo
    {
        public static readonly EvtInfoField<int> MotionUid = new();
        public static readonly EvtInfoField<int> MotionCollisionId = new();
    }
}