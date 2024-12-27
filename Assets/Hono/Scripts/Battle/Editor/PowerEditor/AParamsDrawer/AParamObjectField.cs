using Hono.Scripts.Battle.Base;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// 专门用来处理仅为object类型的参数
    /// </summary>
    public class AParamObjectField : AParamsField
    {
        public AParamObjectField(AParams aParams, string label) : base(aParams, label) { }
        public override void Draw()
        {
            throw new System.NotImplementedException();
        }
    }
}