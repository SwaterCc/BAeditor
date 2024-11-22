namespace Hono.Scripts.Battle {
	public struct DamageResults {
		/// <summary>
		///     伤害最终值
		/// </summary>
		public int DamageValue;

		/// <summary>
		///     是否暴击
		/// </summary>
		public bool IsCritical;

		/// <summary>
		///     最终冲击力
		/// </summary>
		public int ImpactValue;

		/// <summary>
		///     伤害类型
		/// </summary>
		public EDamageType DamageType;

		/// <summary>
		///     伤害是坏的，demo做一个判断处理敌人的伤害数字不同颜色
		/// </summary>
		public bool IsBad;
	}
}