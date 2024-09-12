using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class AttrDefine : SerializedScriptableObject
    {
        
        [TableList]
        public List<AttrDefineItem> AttrDefines = new List<AttrDefineItem>();
        
        public void loadFiled()
        {
            
        }
        
       
    }
}