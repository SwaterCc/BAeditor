using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Hono.Scripts.Battle
{
    public class ConditionFuncType
    {
        //[ValueDropdown("GetIntEnumValues")]
        public int typeId = 1;

        /*private static IEnumerable<ValueDropdownItem<int>> GetIntEnumValues()
        {
            yield return new ValueDropdownItem<int>("属性检查,0=", 1);
            yield return new ValueDropdownItem<int>("Tag检查 param[0]=目标,param[1+]=tagIDs", 2);
            yield return new ValueDropdownItem<int>("怪物类型", 3);
            yield return new ValueDropdownItem<int>("距离比较", 4);
            yield return new ValueDropdownItem<int>("BUFFLayer", 5);
            yield return new ValueDropdownItem<int>("伤害来源", 6);
            yield return new ValueDropdownItem<int>("命中数量 param[0]=比较值,param[1]=比较符", 7);

        }*/
    }

    public class DamageFuncInfo
    {
        public List<ConditionFuncType> ConditionIds = new List<ConditionFuncType>();
        public List<List<int>> ConditionParams = new List<List<int>>();

        //[SerializeField,Tooltip("Apply数值的方法，可以在lua中自定义计算方法，通常保持默认即可")]
        public string ValueFuncName = "normal";

        //[SerializeField,Tooltip("Apply的具体数值，在lua中根据自定义方法生效")]
        public List<int> ValueParams = new List<int> { 0 };
    }
}