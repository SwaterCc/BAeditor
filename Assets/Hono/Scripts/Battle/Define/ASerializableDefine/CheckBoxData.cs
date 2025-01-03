#region

using System;
using Hono.Scripts.Battle.Base;
using Sirenix.OdinInspector;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class CheckBoxData
    {
        public ECheckBoxShapeType ShapeType;

        public RefVector3 Rot = new();

        public RefVector3 Offset = new();

        /// <summary>
        /// 矩形长轴（X）
        /// </summary>
        [ShowIf("ShapeType", ECheckBoxShapeType.Cube)]
        public float Length;

        /// <summary>
        /// 矩形宽轴（Z）
        /// </summary>
        [ShowIf("ShapeType", ECheckBoxShapeType.Cube)]
        public float Width;

        /// <summary>
        /// 矩形高（Y）
        /// </summary>
        [HideIf("ShapeType", ECheckBoxShapeType.Sphere)]
        public float Height;

        /// <summary>
        /// 球形半径
        /// </summary>
        [HideIf("ShapeType", ECheckBoxShapeType.Cube)]
        public float Radius;
    }
}