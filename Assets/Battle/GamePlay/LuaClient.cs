using Hono.Scripts.Battle.Tools;
using XLua;

namespace Hono.Scripts.Battle {
	public class LuaClient : Singleton<LuaClient> {
		private LuaEnv _luaEnv;

		public void Init() {
			_luaEnv = new LuaEnv();
		}
		
		public LuaEnv GetEnv() {
			return _luaEnv;
		}

		public void Tick() {
			_luaEnv.Tick();
		}

		public void Dispose() {
			_luaEnv.Dispose();
		}
	}
}