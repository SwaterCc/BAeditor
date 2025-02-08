using System;
using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle.AbilityFramework
{
    public partial class Ability
    {
        private class AMsgSendNode : ANode<MsgSendNodeData>, IGPoolObject
        {
            public override void DoJob()
            {
                var msgKey = Data.msgKey;
                var actorUid = ParseInt(Data.actorUid);
                var board = GPool<VariableBoard>.Pool.Rent();
                for (int i = 0; i < Data.values.Count; i++)
                {
                    var type = Data.values[i].GetParamType();
                    if (type == null)
                    {
                        return;
                    }

                    var key = Data.msgParamKeys[i];

                    setVariable(type, board, key, Data.values[i]);
                }

                MessageManager.Instance.SendMessage(actorUid, msgKey, board);
                GPool<VariableBoard>.Pool.Recycle(board);
            }

            private void setVariable(Type varType, VariableBoard board, string key, AParams aParams)
            {
                if (varType == typeof(RefInt))
                {
                    var value = ParseInt(aParams);
                    board.Set(key, value);
                }
                else if (varType == typeof(RefFloat))
                {
                    var value = ParseFloat(aParams);
                    board.Set(key, value);
                }
                else if (varType == typeof(RefBoolean))
                {
                    var value = ParseBoolean(aParams);
                    board.Set(key, value);
                }
                else if (varType == typeof(RefVector3))
                {
                    var value = ParseVector3(aParams);
                    board.Set(key, value);
                }
                else
                {
                    var value = ParseRef(aParams);
                    board.SetRef(key, value);
                }
            }


            public override void Recycle()
            {
                GPool<AMsgSendNode>.Pool.Recycle(this);
            }
        }
    }
}