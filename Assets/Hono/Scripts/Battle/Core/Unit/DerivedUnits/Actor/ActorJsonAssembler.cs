using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// 解析出的结构数据
    /// </summary>
    public class ActorAssembleInfo
    {
        /// <summary>
        /// 组件工厂对象
        /// </summary>
        public readonly List<IUnitComponentFactory> Factories = new();

        public ActorAssembleInfo(string json)
        {
            
        }
    }
    
    /// <summary>
    /// 组装器
    /// </summary>
    public static class ActorJsonAssembler
    {
        private static readonly Dictionary<string, ActorAssembleInfo> JsonParseInfos = new();

        /// <summary>
        /// 打包时json数据会写为静态数据
        /// </summary>
        private static string[] _jsonName = { };
        /// <summary>
        /// 打包时json数据会写为静态数据
        /// </summary>
        private static string[] _jsonText = { };
        
        /// <summary>
        /// 初始化解析器
        /// </summary>
        public static void Init()
        {
            for (int i = 0; i < _jsonName.Length; i++)
            {
                var assemble = new ActorAssembleInfo(_jsonText[i]);
                JsonParseInfos.Add(_jsonName[i],assemble);
            }
        }
        
        public static ActorAssembleInfo GetActorAssembleInfo(string jsonKey)
        {
            return JsonParseInfos[jsonKey];
        }
    }
}