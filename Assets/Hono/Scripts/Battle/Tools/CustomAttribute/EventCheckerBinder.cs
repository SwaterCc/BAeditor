using System;

namespace Hono.Scripts.Battle.Tools.CustomAttribute
{
   
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class EventCheckerBinder : System.Attribute
    {
        public string CreateFunc;
        
        public EventCheckerBinder(string createFunc)
        {
            CreateFunc = createFunc;
        }
    }
  
}