namespace Hono.Scripts.Battle {
	public interface ICommand {
		void Undo();
	}
	
	/// <summary>
	/// Set指令缓存Handle接口
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICommandCollection {
		public void AddCommand(ICommand command);
		public void RemoveCommand(ICommand command);
		public void OnCommandChanged();
	}

	/// <summary>
	/// 属性改变记录，目前仅记录值类型修改，引用类型的修改需要自己单独继承实现
	/// </summary>
	public struct AttrCommand<T> : ICommand{
		
		/// <summary>
		/// 修改的值
		/// </summary>
		public T Value;
		
		private ICommandCollection _collection;
		
		public AttrCommand(ICommandCollection collection, T value) {
			Value = value;
			_collection = collection;
			_collection.AddCommand(this);
			_collection.OnCommandChanged();
		}
		
		public void Undo() {
			_collection.RemoveCommand(this);
			_collection.OnCommandChanged();
		}
	}
}