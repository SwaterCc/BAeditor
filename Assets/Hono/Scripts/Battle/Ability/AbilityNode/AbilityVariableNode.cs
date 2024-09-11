
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 变量节点，执行变量创建或者指定变量修改
        /// </summary>
        private class AbilityVariableNode : AbilityNode
        {
            private readonly VariableNodeData _varData;

            public AbilityVariableNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _varData = data.VariableNodeData;
            }

            public override void DoJob()
            {
                if (!_varData.VarParams.TryCallFunc(out var variableBox))
                {
	                Debug.LogError($"函数执行失败 Name {_varData.Name}");
                }
                if (!_varData.ActorUid.TryCallFunc(out var actorUid))
                {
	                Debug.LogError($"函数执行失败 Name {_varData.Name}");
                }
                if (!_varData.AbilityUid.TryCallFunc(out var abilityUid))
                {
	                Debug.LogError($"函数执行失败 Name {_varData.Name}");
                }
                
                AbilityFunction.SetVariableByUid(_varData.Range, (int)actorUid, (int)abilityUid,
	                _varData.Name, variableBox);
            }
        }
    }
}