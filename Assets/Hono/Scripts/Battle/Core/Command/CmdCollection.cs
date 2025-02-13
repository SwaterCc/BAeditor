using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public interface ICommand
    {
        void Do();
        void Undo();
    }

    public class CmdCollection<T> where T : struct , ICommand
    {
        private readonly List<T> _commands = new(10);

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="command"></param>
        public void DoCommand(T command)
        {
            command.Do();
            _commands.Add(command);
        }
        
        /// <summary>
        /// 撤销指令,并将其回收
        /// </summary>
        /// <param name="command"></param>
        /// <param name="notUndo">仅执行清理，指令不会撤销</param>
        public void UndoCommand(T command, bool notUndo = false)
        {
            if (_commands.Remove(command))
            {
                if (!notUndo)
                {
                    command.Undo();
                }
            }
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
            }

            _commands.Clear();
        }
    }
}