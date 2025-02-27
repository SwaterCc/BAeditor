namespace Hono.Scripts.Battle.Event
{
    public static class DeadEventInfoKey
    {
        /// <summary>
        /// 事件来源
        /// </summary>
        public static readonly EvtInfoField<int> DeadUnitUid = new();
    }
}