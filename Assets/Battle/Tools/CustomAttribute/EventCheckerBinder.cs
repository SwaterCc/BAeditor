using System;
using System.Reflection;
using Battle.Event;

namespace Battle.Tools.CustomAttribute
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