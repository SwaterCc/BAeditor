using System;
using System.Collections;
using Battle.Auto;
using Battle.Def;
using UnityEngine;

namespace Battle
{
    public partial class Ability
    {
        /// <summary>
        /// 变量节点，执行变量创建或者指定变量修改
        /// </summary>
        private class AbilityVariableNode : AbilityNode
        {
            private AbilityVariableNodeData _vardata;

            public AbilityVariableNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data)
            {
                _vardata = (AbilityVariableNodeData)data.SpecialNodeData;
            }

            public override void DoJob()
            {
                switch (_vardata.OperationType)
                {
                    case EVariableOperationType.Create:
                        createVariable(GetCollection());
                        break;
                    case EVariableOperationType.Change:
                        updateVariable(GetCollection());
                        break;
                }
            }

            private VariableCollection GetCollection()
            {
                VariableCollection collection = null;
                switch (_vardata.Range)
                {
                    case EVariableRange.Battleground:
                        break;
                    case EVariableRange.Actor:
                        collection = Context.GetActor().GetVariableCollection();
                        break;
                    case EVariableRange.Ability:
                        collection = Context.GetAbility().GetVariableCollection();
                        break;
                }

                return collection;
            }

            private ICValueBox getVariable<T>(T value)
            {
                ICValueBox variable = null;
                if (_vardata.IsFunction)
                {
                    variable = WrapFuncMain.InvokeFunc(_vardata.FuncId, _vardata.Param);
                }
                else if (_vardata.IsAttribute)
                {
                    variable = Context.GetActor().GetAttrBox(_vardata.AttrType).Get();
                }
                else
                {
                    variable = new CValueBox<T>(value);
                }

                return variable;
            }

            /// <summary>
            /// 创建变量,变量不可以直接引用对象，哪怕是列表也必须是新创建的，或者临时变量，如果函数需要对象则提供id去查找
            /// </summary>
            /// <param name="collection"></param>
            /// <exception cref="Exception"></exception>
            private void createVariable(VariableCollection collection)
            {
                ICValueBox v = null;
                switch (_vardata.VariableType)
                {
                    case EVariableType.Int:
                        v = getVariable(_vardata.IntValue);
                        break;
                    case EVariableType.Bool:
                        v = getVariable(_vardata.BoolValue);
                        break;
                    case EVariableType.Float:
                        v = getVariable(_vardata.FloatValue);
                        break;
                    case EVariableType.String:
                        v = getVariable(_vardata.StringValue);
                        break;
                    case EVariableType.List:
                    case EVariableType.Dict:
                        v = getVariable<object>(null);
                        break;
                }

                if (v == null)
                {
                    throw new Exception("逻辑有问题");
                }

                collection.Add(_vardata.Name, v);
            }

            private void changeValue<T>(VariableCollection collection, T value)
            {
                if (collection.TryGetVariable(_vardata.Name, out CValueBox<T> valueBox))
                {
                    if (_vardata.IsFunction)
                    {
                        collection.Remove(_vardata.Name);
                        var newValue = WrapFuncMain.InvokeFunc(_vardata.FuncId, _vardata.Param);
                        collection.Add(_vardata.Name, newValue);
                    }
                    else if (_vardata.IsAttribute)
                    {
                        collection.Remove(_vardata.Name);
                        var newValue = Context.GetActor().GetAttrBox(_vardata.AttrType).Get();
                        collection.Add(_vardata.Name, newValue);
                    }
                    else
                    {
                        valueBox.Set(value);
                    }
                }
                else
                {
                    Debug.LogError("逻辑有问题，改值失败了");
                }
            }

            private void updateVariable(VariableCollection collection)
            {
                switch (_vardata.VariableType)
                {
                    case EVariableType.Int:
                        changeValue(collection, _vardata.IntValue);
                        break;
                    case EVariableType.Bool:
                        changeValue(collection, _vardata.BoolValue);
                        break;
                    case EVariableType.Float:
                        changeValue(collection, _vardata.FloatValue);
                        break;
                    case EVariableType.String:
                        changeValue(collection, _vardata.StringValue);
                        break;
                    case EVariableType.List:
                        changeValue<IList>(collection, null);
                        break;
                    case EVariableType.Dict:
                        changeValue<IDictionary>(collection, null);
                        break;
                }
            }
        }
    }
}