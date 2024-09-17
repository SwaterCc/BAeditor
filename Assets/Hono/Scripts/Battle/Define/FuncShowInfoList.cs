using System;
using System.Collections.Generic;
using System.Reflection;
using Hono.Scripts.Battle.Tools;
using Hono.Scripts.Battle.Tools.CustomAttribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class ParamShowInfo
    {
        /// <summary>
        /// 类型，反射产生
        /// </summary>
        [LabelText("参数类型")]
        public string Type;
        
        /// <summary>
        /// 反射产生
        /// </summary>
        [LabelText("参数命名")]
        public string Name;
        
        /// <summary>
        /// 描述，手填字段
        /// </summary>
        [LabelText("参数描述")]
        public string Desc;
    }

    public class FuncShowInfo
    {
        [LabelText("函数名")]
        public string FuncName;
        
        [LabelText("函数描述")]
        public string FuncDesc;
       
        [LabelText("返回值类型")]
        public string ReturnType;
        
        public int OverloadTag;
        
        [LabelText("参数列表")]
        public List<ParamShowInfo> Params = new List<ParamShowInfo>();
    }
    
    [CreateAssetMenu(menuName = "战斗编辑器/FuncShowInfoList")] 
    public class FuncShowInfoList : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "函数名", ValueLabel = "函数信息")]
        public Dictionary<string, List<FuncShowInfo>> FuncDict = new Dictionary<string, List<FuncShowInfo>>();
        
        [Button("刷新列表")]
        public void Refresh()
        {
            FuncDict.Clear();
            Type type = typeof(AbilityFunction);
            
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                AbilityMethod attr = null;
                foreach (var obj in  method.GetCustomAttributes(typeof(AbilityMethod), false))
                {
                    if (obj is AbilityMethod cache)
                    {
                        attr = cache;
                    }
                }

                if (attr == null) continue;
                
                if (!FuncDict.TryGetValue(method.Name, out var funcList))
                {
                    funcList = new List<FuncShowInfo>();
                    FuncDict.Add(method.Name,funcList);
                }

                var funcShowInfo = new FuncShowInfo();
                funcShowInfo.FuncName = method.Name;
                funcShowInfo.ReturnType = method.ReturnType.ToString();
                int idx = 0;
                foreach (var parameter in method.GetParameters())
                {
                    var param = new ParamShowInfo();
                    param.Type = parameter.ParameterType.ToString();
                    param.Name = parameter.Name;
                   
                    funcShowInfo.Params.Add(param);
                }
                funcList.Add(funcShowInfo);
            }
        }
    }
}