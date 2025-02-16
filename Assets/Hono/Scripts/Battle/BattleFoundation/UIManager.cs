using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hono.Scripts.Battle
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Canvas _mainCanvas;

        /// <summary>
        /// 发送数据给指定UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SendDataToUIPanel<T>() { }

        public async UniTask LoadMainCanvas()
        {
            _mainCanvas =
                await Addressables.LoadAssetAsync<Canvas>("Assets/BattleModel/Prototype/UI/BattleCanvas.prefab");
        }

        public void SetLoadingUI(bool isShow)
        {
            if (_mainCanvas == null) return;
        }

        /// <summary>
        /// 设置战略布置地图
        /// </summary>
        /// <param name="isShow"></param>
        public void SetBattleFieldMap(bool isShow)
        {
            if (_mainCanvas == null) return;
        }

        /// <summary>
        /// 设置战略地图状态
        /// </summary>
        /// <param name="isShow"></param>
        public void SetStrategicMap(bool isShow)
        {
            if (_mainCanvas == null) return;
        }

        public void SetScoreUI(bool isShow)
        {
            if (_mainCanvas == null) return;
        }
    }
}