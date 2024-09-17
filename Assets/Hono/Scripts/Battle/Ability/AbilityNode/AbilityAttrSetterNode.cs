using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        private class AbilityAttrSetterNode : AbilityNode
        {
            private AttrSetterNodeData _attrNodeData;

            public AbilityAttrSetterNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _attrNodeData = (AttrSetterNodeData)_data;
            }
            public override void DoJob()
            {
                if (!_attrNodeData.Value.Parse(out object value))
                {
                    Debug.LogError($"设置属性失败 {_attrNodeData.LogicAttr}");
                    return;    
                }

                _executor.Ability.Actor.SetAttrBox(_attrNodeData.LogicAttr, value, _attrNodeData.IsTempAttr);
                
                DoChildrenJob();
            }
        }
    }
}