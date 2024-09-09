﻿using System;
using UnityEngine;

namespace Hono.Scripts.Battle
{
	[Serializable]
    public abstract class CheckBoxData
    {
        public ECheckBoxShapeType ShapeType;
        public Quaternion Rot = Quaternion.identity;
        //是否应用百分比存储偏移
        public bool OffsetUsePercent = false;
        public Vector3 Offset = Vector3.zero;
        public bool WhenAbilityEndRemove;
    }

	[Serializable]
    public class CheckBoxCube : CheckBoxData
    {
        /// <summary>
        /// 矩形长轴（X）
        /// </summary>
        public float Length;

        /// <summary>
        /// 矩形宽轴（Z）
        /// </summary>
        public float Width;

        /// <summary>
        /// 矩形高（Y）
        /// </summary>
        public float Height;

        public CheckBoxCube() {
	        ShapeType = ECheckBoxShapeType.Cube;
        }
    }

    /// <summary>
    /// 球
    /// </summary>
    [Serializable]
    public class CheckBoxSphere : CheckBoxData
    {
        public float Radius;
        public CheckBoxSphere() {
	        ShapeType = ECheckBoxShapeType.Sphere;
        }
    }

    /// <summary>
    /// 圆柱
    /// </summary>
    [Serializable]
    public class CheckBoxCylinder : CheckBoxData
    {
        public float Radius;
        public float Height;
        public CheckBoxCylinder() {
	        ShapeType = ECheckBoxShapeType.Cylinder;
        }
    }

    /// <summary>
    /// 扇形
    /// </summary>
    [Serializable]
    public class CheckBoxSector : CheckBoxData
    {
        public float Radius;
        public float Height;
        public float Angle;
        public CheckBoxSector() {
	        ShapeType = ECheckBoxShapeType.Sector;
        }
    }
}