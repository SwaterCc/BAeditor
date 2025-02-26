namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    ///  触发器 状态：激活，非激活
    ///  功能: 设置检测条件（tags，LevelObjectId）,设置触发时机（进入，离开）
    ///  行为：满足条件后发送自定义事件
    /// </summary>
    public class TriggerBox : LevelObject { }
}