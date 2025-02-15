using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;

namespace Hono.Scripts.Battle
{
    public class UIManager : MonoSingleton<UIManager>
    {
        /// <summary>
        /// 发送数据给指定UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SendDataToUIPanel<T>()
        {
            
        }

        public void SetLoadingUI(bool isShow)
        {
            
        }
        
        /// <summary>
        /// 设置战略布置地图
        /// </summary>
        /// <param name="isShow"></param>
        public void SetBattleFieldMap(bool isShow)
        {
            
        }
        
        /// <summary>
        /// 设置战略地图状态
        /// </summary>
        /// <param name="isShow"></param>
        public void SetStrategicMap(bool isShow)
        {
            
        }

        public void SetScoreUI(bool isShow)
        {
            
        }
    }
}