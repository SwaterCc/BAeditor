using System;
using Hono.Scripts.Battle.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hono.Scripts.Battle
{
    public interface ISendToUIData { };

    public interface IUIPassData { };

    public abstract class BattleUIHandle : MonoBehaviour
    {
        public ISendToUIData Data;
        private Action<IUIPassData> _onUICloseCallBack;
            
        public virtual void OnUIInterfaceCalled(ISendToUIData sendData, Action<IUIPassData> onUICloseCallBack )
        {
            Data = sendData;
            _onUICloseCallBack = onUICloseCallBack;
            gameObject.SetActive(true);
        }

        protected abstract IUIPassData returnPassData();

        protected abstract void onActiveFalse();
        
        public void OnDisable()
        {
            onActiveFalse();
            _onUICloseCallBack.Invoke(returnPassData());
        }
    }
    
    public static class UIInterface
    {
        public static bool CallUI<T>(ISendToUIData sendData = null,
            Action<IUIPassData> onUICloseCallBack = null) where T : BattleUIHandle
        {
            var canvas = Object.FindObjectOfType<Canvas>();

            if (canvas == null)
            {
                Debug.LogError("找不到 Canvas");
                return false;
            }
            
            bool sendMsgAny = false;
            foreach (Transform child in canvas.transform)
            {
                if (child.gameObject.TryGetComponent<T>(out var handle))
                {
                    handle.OnUIInterfaceCalled(sendData, onUICloseCallBack);
                    sendMsgAny = true;
                }
            }
            
            if (!sendMsgAny)
            {
                Debug.LogError($"找不到指定UI name : {nameof(T)}");
            }
            
            return sendMsgAny;
        }

        public static void ShowDamage(Vector3 pos, DamageResults damageResults)
        {
            //BattlePanel.ShowDamage(target.Pos,res);
        }
    }
}