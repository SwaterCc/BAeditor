using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 打击数据
    /// </summary>
    [Serializable] 
    public class HitBoxData
    {
        /// <summary>
        /// 伤害盒子类型
        /// </summary>
        public EHitType HitType;
        
        /// <summary>
        /// 打击点最大检测次数
        /// </summary>
        [Tooltip("打击点最大检测次数")]
        public int MaxCount = 1;

        /// <summary>
        /// 打击点对单个目标最大有效次数
        /// </summary>
        [Tooltip("打击点对单个目标最大有效次数")]
        public int ValidCount;
        
        /// <summary>
        /// 第一次触发时间
        /// </summary>
        [Tooltip("第一次触发时间")]
        public float FirstInterval;
        
        /// <summary>
        /// 打击点检测间隔
        /// </summary>
        [Tooltip("打击点检测间隔")]
        public float Interval;
        
        /// <summary>
        /// 单次打击造成几次伤害
        /// </summary>
        [Tooltip("单次打击造成几次伤害")]
        public int OnceHitDamageCount = 1;
        
        [OnValueChanged("OnEnumChanged")]
	    public ECheckBoxShapeType ShapeType;
        
	    /// <summary>
	    /// 伤害数据
	    /// </summary>
	    public int DamageConfigId;
	    
        /// <summary>
        /// Aoe打击点AABB盒子数据
        /// </summary>
        [OdinSerialize]
        public CheckBoxData AoeData;

        public bool UseCustomFilter;
        
        [ShowIf("UseCustomFilter")]
        public FilterSetting FilterSetting;
        
        private void OnEnumChanged()
        {
	        switch (ShapeType) {
		        case ECheckBoxShapeType.Cube:
			        AoeData = new CheckBoxCube(ShapeType);
			        AoeData.ShapeType = ECheckBoxShapeType.Cube;
			        break;
		        case ECheckBoxShapeType.Sphere:
			        AoeData = new CheckBoxSphere(ShapeType);
			        AoeData.ShapeType = ECheckBoxShapeType.Sphere;
			        break;
		        case ECheckBoxShapeType.Cylinder:
		        case ECheckBoxShapeType.Sector:
			        AoeData.ShapeType = ECheckBoxShapeType.Cylinder;
			        AoeData = new CheckBoxCylinder(ShapeType);
			        break;
	        }
        }
    }
}