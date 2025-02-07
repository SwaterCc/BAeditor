namespace Hono.Scripts.Battle.Core.Base
{
    public interface IEnableSnapshot<out T>
    {
        public T GetSnapshot();
    }
}