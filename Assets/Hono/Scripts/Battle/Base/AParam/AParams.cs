#region
using System;
using System.Collections.Generic;
#endregion

namespace Hono.Scripts.Battle.Base {
	/// <summary>
	/// 编辑器数据
	/// </summary>
	[Serializable]
	public class AParams {
		/// <summary>
		/// 参数类型
		/// </summary>
		public EParamType paramType;

		/// <summary>
		/// 函数名
		/// </summary>
		public string funcName;
		/// <summary>
		/// 函数参数
		/// </summary>
		public List<AParams> funcParams;
		
		/// <summary>
		/// 变量名
		/// </summary>
		public string variableName;

		/// <summary>
		/// 属性类型
		/// </summary>
		public EAttrType attrType;
		
		/// <summary>
		/// SimpaleValue
		/// </summary>
		public IARef value;//基础数值，

		public AParams() { }

		/// <summary>
		///     拷贝构造函数
		/// </summary>
		/// <param name="aParams"></param>
		public AParams(in AParams aParams) {
			if (aParams == null)
				throw new ArgumentNullException(nameof(aParams));

			paramType = aParams.paramType;
			funcName = aParams.funcName;

			// 深拷贝 FuncParam 列表
			if (aParams.funcParams != null) {
				funcParams = new List<AParams>();
				foreach (var param in aParams.funcParams) {
					funcParams.Add(new AParams(param));
				}
			}

			value = aParams.value.DeepCopy();
			variableName = aParams.variableName;
			attrType = aParams.attrType;
		}

		/// <summary>
		/// 复制
		/// </summary>
		public void CopyTo(AParams copy) {
			var temp = new AParams(copy);

			paramType = temp.paramType;
			funcName = temp.funcName;
			funcParams = temp.funcParams;
			value = temp.value;
			variableName = temp.variableName;
			attrType = temp.attrType;
		}
		
		public override string ToString() {
			string desc = "";

			switch (paramType) {
				case EParamType.Simple:
					desc = value == null ? "null" : value.ToString();
					break;
				case EParamType.Function:
					if (string.IsNullOrEmpty(funcName)) {
						return "函数未初始化";
					}

					desc = "" + funcName + "(";
					for (var index = 0; index < funcParams.Count; index++) {
						var parameter = funcParams[index];
						desc += parameter;
						if (index != funcParams.Count - 1) {
							desc += ",";
						}
					}

					desc += ")";
					break;
				case EParamType.Variable:
					desc = string.IsNullOrEmpty(variableName) ? "未设置变量名" : "变量：" + variableName;
					break;
				case EParamType.Attr:
					desc = "属性：" + attrType;
					break;
			}

			return desc;
		}
	}
}