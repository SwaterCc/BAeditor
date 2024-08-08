namespace Battle
{
    public interface IValueSetting
    {
        public object Get();
        public void Set(object value);
    }
    
    public abstract class CValue : IValueSetting
    {
        protected object _value;

        public object Get()
        {
            return _value;
        }

        public void Set(object value)
        {
            _value = value;
        }
    }
}