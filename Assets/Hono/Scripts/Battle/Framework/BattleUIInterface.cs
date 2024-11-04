#region

using System;

#endregion

namespace Hono.Scripts.Battle
{
    public interface ISendToUIData { };

    public interface IUIPassToLogicData { };

    public interface IBattleUIHandle
    {
        public void OnUIInterfaceCall(ISendToUIData sendData, Action<IUIPassToLogicData> onUICloseCallBack);
    }

    public static class BattleUIInterface
    {
        
    }
}