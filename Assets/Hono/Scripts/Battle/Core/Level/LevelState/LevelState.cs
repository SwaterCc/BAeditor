namespace Hono.Scripts.Battle.Core {
	public interface ILevelMode {
		public void Start();
		public void Tick();
		public void Exit();
	}
}