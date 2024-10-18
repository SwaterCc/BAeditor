using System;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public static class ParameterParser {
		public static bool Parse<T>(this Parameter parameter, in Ability ability, out T value) {
			value = default;
			if (parameter == null) {
				return false;
			}

			switch (parameter.ParameterType) {
				case EParameterType.Simple:
					value = (T)parameter.Value;
					break;
				case EParameterType.Function:
					if (!parameter.TryCallFunction(ability, out var obj)) {
						return false;
					}

					value = (T)obj;
					return true;
				case EParameterType.Variable:
					value = (T)ability.Variables.Get(parameter.VairableName);
					break;
				case EParameterType.Attr:
					if (typeof(T) == typeof(object)) {
						value = (T)ability.Actor.GetAttrBox(parameter.AttrType);
					}
					else {
						value = ability.Actor.GetAttr<T>(parameter.AttrType);
					}

					break;
			}

			return true;
		}

		public static bool TryCallFunction(this Parameter parameter, in Ability ability, out object value) {
			value = null;
			if (string.IsNullOrEmpty(parameter.FuncName)) {
				Debug.LogError("函数名为空");
				return false;
			}

			var funcInfo = AbilityFuncPreLoader.GetFuncInfo(parameter.FuncName);
			if (funcInfo == null) {
				Debug.LogError($"获取函数失败{parameter.FuncName}");
				return false;
			}

			if (parameter.FuncParams == null && funcInfo.ParamCount > 0) {
				Debug.LogError($"parameter参数列表未初始化 {parameter.FuncName}");
				return false;
			}

			if (funcInfo.ParamCount == 0) {
				Ability.Context.UpdateContext(ability);
				funcInfo.Invoke(null, null);
				Ability.Context.ClearContext();
			}
			else {
				//TODO:有GC问题后续优化
				object[] funcParams = new object[funcInfo.ParamCount];
				for (var index = 0; index < parameter.FuncParams.Count; index++) {
					var funcParam = parameter.FuncParams[index];
					if (funcParam.Parse<object>(ability, out var paramValue)) {
						funcParams[index] = paramValue;
					}
					else {
						return false;
					}
				}

				Ability.Context.UpdateContext(ability);
				value = funcInfo.Invoke(null, funcParams);
				Ability.Context.ClearContext();
			}

			return true;
		}
	}
}