using System;

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
   
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EventCheckerBinder : System.Attribute
    {
        public string CreateFunc;

        public Type EventInfoType;
        
        public EventCheckerBinder(string createFunc,Type eventInfoType)
        {
            CreateFunc = createFunc;
            EventInfoType = eventInfoType;
        }
    }
  
}