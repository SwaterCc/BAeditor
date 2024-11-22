#region

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle {
	/// <summary>
	///     打击数据
	/// </summary>
	[Serializable]
	public class HitBoxData {
		/// <summary>
		///     伤害盒子类型
		/// </summary>
		[LabelText("打击类型")] public EHitType HitType;

		/// <summary>
		///     打击点最大检测次数
		/// </summary>
		[LabelText("打击点检测次数")] public int MaxCount = 1;

		/// <summary>
		///     打击点对单个目标最大有效次数
		/// </summary>
		[LabelText("打击点对单个目标最大有效次数")] public int ValidCount = 1;

		/// <summary>
		///     第一次触发时间
		/// </summary>
		[LabelText("第一次触发时间")] public float FirstInterval;

		/// <summary>
		///     打击点检测间隔
		/// </summary>
		[LabelText("打击点检测间隔")] public float Interval;

		/// <summary>
		///     单次打击造成几次伤害
		/// </summary>
		[LabelText("单次打击造成几次伤害")] public int OnceHitDamageCount = 1;

		/// <summary>
		///     伤害数据
		/// </summary>
		public int DamageConfigId;

		/// <summary>
		/// 额外的Ability
		/// </summary>
		public List<int> AbilityIds = new();

		/// <summary>
		/// 必定暴击标志
		/// </summary>
		public bool CriticalFlag;
		
		[ShowIf("HitType", EHitType.Aoe)] 
		public FilterSetting FilterSetting = new();
	}
}