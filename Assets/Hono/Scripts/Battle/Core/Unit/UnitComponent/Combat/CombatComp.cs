using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 战斗组件
    /// </summary>
    public class CombatComp : UnitComponent
    {
        /// <summary>
        /// 技能列表
        /// </summary>
        public Dictionary<int, Skill> Skills { get; } = new(10);
        
        /// <summary>
        /// Buff列表
        /// </summary>
        public Dictionary<int> Buffs { get; } = new(10);
        
        void GetBuffLayer(Unit unit,int buffId,)

        public override void Init()
        {
            
        }
        //技能不做目标选择，技能仅接受目标，目标的选择来自上层传入，选择的方式由技能配置（方向（子弹用），坐标（AOE），目标(单体技能或AOE)，无（逻辑里自选））
        //提供接口获取目标返回坐标，获取目标返回UID，获取目标返回方向向量
        //
        
        #region Buff接口
        public void AddBuff(int sourceActorId, int buffConfigId, int buffLayer = 1)
        {
            var buffData = AssetManager.Instance.GetData<BuffData>(buffConfigId);

            if (buffData == null)
            {
                Debug.LogError($"Id {buffConfigId} BuffData is null");
                return;
            }

            if (buffData.FilterTags.Count > 0)
            {
                switch (buffData.AddRule)
                {
                    case EAddBlockRule.HasTags:
                        if (buffData.FilterTags.Any(tag => !Unit.Tags.HasTag(tag)))
                        {
                            return;
                        }

                        break;
                    case EAddBlockRule.NoTags:
                        if (buffData.FilterTags.Any(tag => Unit.Tags.HasTag(tag)))
                        {
                            return;
                        }

                        break;
                }
            }

            if (!Buffs.TryGetValue(buffConfigId, out var buff))
            {
                buff = GPool<Buff>.Pool.Rent();
                buff.OnRent(Unit, sourceActorId, buffData);
                Buffs.Add(buff.ConfigId, buff);
            }
            else
            {
                if (CheckReplace(buff, buffData, sourceActorId))
                {
                    GPool<Buff>.Pool.Recycle(buff);
                    buff = GPool<Buff>.Pool.Rent();
                    buff.OnRent(Unit, sourceActorId, buffData);
                    Buffs[buffConfigId] = buff;
                }
                else
                {
                    buff.AddLayer(buffLayer);
                }
            }
        }

        private bool CheckReplace(Buff oldBuff, BuffData newBuffData, int sourceId)
        {
            switch (newBuffData.ReplaceRule)
            {
                case EBuffReplaceRule.SameSourceReplace:
                {
                    //同源替换
                    return oldBuff.SourceUnitUid == sourceId;
                }
                case EBuffReplaceRule.SameSourceAdd:
                {
                    //非同源替换
                    return oldBuff.SourceUnitUid != sourceId;
                }
                case EBuffReplaceRule.Add:
                {
                    //不替换
                    return false;
                }
                case EBuffReplaceRule.OnlyOne:
                {
                    //全替换
                    return true;
                }
            }

            Debug.LogError("不应该走到这里");
            return false;
        }

        public void RemoveBuff(int buffConfigId)
        {
            if (Buffs.TryGetValue(buffConfigId, out var buff))
            {
                Buffs.Remove(buffConfigId);
                GPool<Buff>.Pool.Recycle(buff);
            }
        }

        public int GetBuffLayer(int configId)
        {
            if (!Buffs.TryGetValue(configId, out var buff))
            {
                return -1;
            }

            return buff.LayerCount;
        }

        public int GetBuffSource(int configId)
        {
            if (!Buffs.TryGetValue(configId, out var buff))
            {
                return -1;
            }

            return buff.SourceActorUid;
        }
        #endregion
        
        protected override void onClear() { }
    }
}