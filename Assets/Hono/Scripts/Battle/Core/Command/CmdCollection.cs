using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public interface ICommand : IAPoolObject
    {
        void Do();
        void Undo();
    }

    public class CmdCollection
    {
        private readonly List<ICommand> _commands = new(10);

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="command"></param>
        public void DoCommand(ICommand command)
        {
            command.Do();
            _commands.Add(command);
        }
        
        /// <summary>
        /// 撤销指令,并将其回收
        /// </summary>
        /// <param name="command"></param>
        /// <param name="notUndo">仅执行清理，指令不会撤销</param>
        public void UndoCommand(ICommand command, bool notUndo = false)
        {
            if (_commands.Remove(command))
            {
                if (!notUndo)
                {
                    command.Undo();
                }
            }

            APoolManager.Instance.RecycleAObject(command);
        }

        /// <summary>
        ///  清空指令容器
        /// </summary>
        /// <param name="notUndo">仅执行清理，指令不会撤销</param>
        public void Clear(bool notUndo = false)
        {
            foreach (var command in _commands)
            {
                if (!notUndo)
                {
                    command.Undo();
                }

                APoolManager.Instance.RecycleAObject(command);
            }

            _commands.Clear();
        }
    }
}