namespace Hono.Scripts.Battle.Core
{
    public partial class Unit
    {
        /// <summary>
        /// 仅允许应用在Unit及其衍生类上
        /// </summary>
        protected internal interface ILoadableUnit
        {
            /// <summary>
            /// 是否加载完成
            /// </summary>
            public bool IsLoadFinish { get; }
        
            /// <summary>
            /// 是否加载出错
            /// </summary>
            public bool HasLoadError { get; }
        
            /// <summary>
            /// 加载接口
            /// </summary>
            public void Load();
        }
    }
}