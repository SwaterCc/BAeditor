using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 网格单元
    /// </summary>
    public class MapGridCeil
    {
        /// <summary>
        /// 单元格大小
        /// </summary>
        public Rect CeilRect { get; }

        public MapGridCeil(Rect ceilRect)
        {
            CeilRect = ceilRect;
        }


        /// <summary>
        /// 是否在该单元格内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool isInCeil(float x,float y)
        {
            return true;
        }
    }
}