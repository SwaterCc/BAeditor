#region

using UnityEngine;
using UnityEngine.Serialization;

#endregion

namespace Hono.Scripts.Battle
{
    public class BulletData : ASerializableData, IIncludeAbility
    {
        /// <summary>
        /// 子弹有效半径
        /// </summary>
        public float hitRadius;
        /// <summary>
        /// 忽略所有不是target的对象
        /// </summary>
        public bool ignoreAllNotTarget;
        /// <summary>
        /// 非目标碰撞条件
        /// </summary>
        public ConditionFilterSetting notTargetHitCondition = new();
        /// <summary>
        /// 速度
        /// </summary>
        public float speed;
        /// <summary>
        /// 加速度
        /// </summary>
        public float acceleration;
        /// <summary>
        /// 转向速度(-1为秒转)
        /// </summary>
        public float rotSpeed;
        /// <summary>
        /// 子弹生命时长
        /// </summary>
        public float lifeTime;
        /// <summary>
        /// 最小命中间隔
        /// </summary>
        public float minHitInterval;
        /// <summary>
        /// 最大命中次数
        /// </summary>
        public int maxHitCount;
        /// <summary>
        /// 是否用key值
        /// </summary>
        public bool isUseVFXKeyModel;
        /// <summary>
        /// 飞行时特效
        /// </summary>
        public string flyVFXStr;
        /// <summary>
        /// 子弹销毁时特效
        /// </summary>
        public string hitVFXStr;
        /// <summary>
        /// 子弹Ability数据
        /// </summary>
        [SerializeField]
        private AbilityData bulletAbility;
        public AbilityData BulletAbility => bulletAbility;

        public void AddAbilityData()
        {
            if (bulletAbility == null)
            {
                bulletAbility = CreateInstance<AbilityData>();
                bulletAbility.name = "bulletAbility"; // 设置子资产名称
                bulletAbility.fileName = fileName;
                bulletAbility.id = id;
                bulletAbility.path = path;
                bulletAbility.abilityBelongType = EAbilityBelongType.Bullet;
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.AddObjectToAsset(bulletAbility, this);
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
#endif
            }
        }
    }
}