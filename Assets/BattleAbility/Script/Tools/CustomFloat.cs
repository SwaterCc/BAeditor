namespace BattleAbility.Tools
{
    /// <summary>
    /// 自定义Float，用于回避Float精度问题，实际存储内容是整型，赋值时会将float * Precision（10000）后存储
    ///
    /// 1.后续考虑尾数截断
    /// 2.尾数查表
    /// </summary>
    public class CustomFloat
    {
        /// <summary>
        /// 保留精度是4位
        /// </summary>
        private const int Precision = 10000;
        
        
        
    }
}