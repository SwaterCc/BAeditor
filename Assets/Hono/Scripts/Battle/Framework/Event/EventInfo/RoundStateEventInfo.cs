namespace Hono.Scripts.Battle.Event
{
    public class RoundStateEventInfo : IEventInfo
    {
        public int CurRoundCount;
        public int RoundScore;

        public void Clear()
        {
            CurRoundCount = 0;
            RoundScore = 0;
        }
    }
}