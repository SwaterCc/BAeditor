#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        /// <summary>
        ///     用ConfigId索引,每个buff只会有一个 system comp->数据
        /// </summary>
        public class BuffComp : AComponent
        {
            public Dictionary<int, Buff> Buffs { get; } = new(30);

            public BuffComp(ActorLogic logic) : base(logic) { }

            public override void Init() { }

            public override void Clear() { }

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
                        case EApplicationRequirement.HasTags:
                            if (buffData.FilterTags.Any(tag => !Self.TagCollection.HasTag(tag, ETagSearchRange.All)))
                            {
                                return;
                            }

                            break;
                        case EApplicationRequirement.NoTags:
                            if (buffData.FilterTags.Any(tag => Self.TagCollection.HasTag(tag, ETagSearchRange.All)))
                            {
                                return;
                            }

                            break;
                    }
                }

                if (!Buffs.TryGetValue(buffConfigId, out var buff))
                {
                    buff = APool<Buff>.Pool.Rent();
                    buff.OnRent(ActorLogic, sourceActorId, buffData);
                    Buffs.Add(buff.ConfigId, buff);
                }
                else
                {
                    if (CheckReplace(buff, buffData, sourceActorId))
                    {
                        APool<Buff>.Pool.Recycle(buff);
                        buff = APool<Buff>.Pool.Rent();
                        buff.OnRent(ActorLogic, sourceActorId, buffData);
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
                        return oldBuff.SourceActorUid == sourceId;
                    }
                    case EBuffReplaceRule.SameSourceAdd:
                    {
                        //非同源替换
                        return oldBuff.SourceActorUid != sourceId;
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
                    APool<Buff>.Pool.Recycle(buff);
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
        }
    }
}