using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
	[Serializable]
    public struct CheckBoxData {
	    [LabelText("检测形状类型")]
	    public ECheckBoxShapeType ShapeType;
        
	    [LabelText("旋转")]
	    public Quaternion Rot;
	    
	    [LabelText("偏移")]
        public Vector3 Offset;
      
	    [LabelText("是否使用百分比存储偏移")]
        public bool OffsetUsePercent;
        
        /// <summary>
        /// 矩形使用长轴（X）
        /// </summary>
        
        [LabelText("矩形X轴")]
        public float Length;

        /// <summary>
        /// 矩形使用宽轴（Z）
        /// </summary>
        [LabelText("矩形Z轴")]
        public float Width;

        /// <summary>
        /// 通用高（Y）
        /// </summary>
        public float Height;

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius;
        
        /// <summary>
        /// 角度
        /// </summary>
        public float Angle;
    }
}