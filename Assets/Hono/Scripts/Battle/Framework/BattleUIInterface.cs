using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public interface ISendToUIData { };

    public interface IUIPassData { };

    public class BattleUIHandle : MonoBehaviour
    {
        public void OnUIInterfaceCalled(ISendToUIData sendData, Action<IUIPassData> onUICloseCallBack) { }
    };

    public static class BattleUIInterface
    {
        public static bool CallUI(string uiName, ISendToUIData sendData = null,
            Action<IUIPassData> onUICloseCallBack = null)
        {
            var battleUIHandles = Object.FindObjectsByType<BattleUIHandle>(FindObjectsSortMode.InstanceID);
            if (battleUIHandles == null || battleUIHandles.Length == 0)
            {
                return false;
            }

            bool sendMsgAny = false;
            foreach (var handle in battleUIHandles)
            {
                if (handle.name == uiName)
                {
                    handle.OnUIInterfaceCalled(sendData, onUICloseCallBack);
                    sendMsgAny = true;
                }
            }

            return sendMsgAny;
        }
    }
}