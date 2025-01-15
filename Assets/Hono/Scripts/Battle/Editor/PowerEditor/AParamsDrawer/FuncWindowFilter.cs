using System;
using System.Collections.Generic;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// AParam类型显示筛选器
    /// </summary>
    public class FuncWindowFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly List<Type> FilterItems = new(20);
        /// <summary>
        /// 开启反选
        /// </summary>
        public bool Reelection { get; set; }

        /// <summary>
        /// 重置筛选器
        /// </summary>
        public void ResetFilter()
        {
            FilterItems.Clear();
            Reelection = false;
        }

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="funcReturnType"></param>
        /// <returns></returns>
        public bool Filter(Type funcReturnType)
        {
            if (FilterItems.Count <= 0)
            {
                return Reelection;
            }

            var result = FilterItems.Contains(funcReturnType);

            if (Reelection)
            {
                return !result;
            }

            return result;
        }
    }
}