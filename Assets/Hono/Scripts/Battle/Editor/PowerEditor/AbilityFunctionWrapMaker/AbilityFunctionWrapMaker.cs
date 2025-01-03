using System.IO;
using System.Text;

namespace Hono.Scripts.Battle.Editor.PowerEditor
{
    public class AbilityFunctionWrapMaker
    {
        /// <summary>
        /// 函数包装注册模板
        /// </summary>
        private string _registerTemplate;

        /// <summary>
        /// 函数包装模板
        /// </summary>
        private string _funcWrapTemplate;

        public AbilityFunctionWrapMaker()
        {
            using (StreamReader reader =
                new StreamReader("Assets/Hono/Scripts/Battle/Editor/PowerEditor/AbilityFunctionWrapMaker/AbilityFunctionRegisterTemplate", Encoding.Default))
            {
                _registerTemplate = reader.ReadToEnd();
            }
            
            using (StreamReader reader =
                new StreamReader("Assets/Hono/Scripts/Battle/Editor/PowerEditor/AbilityFunctionWrapMaker/AbilityFunctionWrapTemplate", Encoding.Default))
            {
                _funcWrapTemplate = reader.ReadToEnd();
            }
        }
        
        
        public void Make()
        {
            
        }
    }
    
    //操控不同类型的单位，独立单位->巨像，器械，完全特化的3c，完全不同的操作模式，镜头视角，ui界面，
    //移动，攻击之间的关系，攻击可在移动时释放，攻击不可被移动打断-》没有攻击状态-》idle，休眠，唤醒，
    //
}