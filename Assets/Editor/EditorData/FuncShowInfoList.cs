using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class ParamShowInfo
    {
        /// <summary>
        /// 类型，反射产生
        /// </summary>
        [LabelText("参数类型")]
        public string Type;
        
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
        
        [LabelText("参数列表")]
        public List<ParamShowInfo> Params = new List<ParamShowInfo>();
    }
    
    [CreateAssetMenu(menuName = "战斗编辑器/FuncShowInfoList")] 
    public class FuncShowInfoList : SerializedScriptableObject
    {
        public Dictionary<string, List<FuncShowInfo>> FuncList = new Dictionary<string, List<FuncShowInfo>>();

        [Button("刷新列表")]
        public void Refresh()
        {
            
        }
    }
}