using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle.Event
{
    public static class AttrChangedEventInfo
    {
        /// <summary>
        /// 事件发送者
        /// </summary>
        public static readonly EvtInfoField<int> SourceUnitUid = new ();
        
        /// <summary>
        /// 属性类型
        /// </summary>
        public static readonly EvtInfoField<EAttrType> AttrType = new();

        /// <summary>
        /// 值
        /// </summary>
        public static readonly EvtInfoField<int> Value = new();
    }
}