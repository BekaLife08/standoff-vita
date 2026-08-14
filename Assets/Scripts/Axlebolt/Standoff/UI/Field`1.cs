namespace Axlebolt.Standoff.UI
{
	public abstract class Field<T> : View
	{
		public delegate void EventHandler(T t);

		private T _value;

		public EventHandler ValueChangedHandler
		{
			get;
			set;
		}

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				SetValue(value);
			}
		}

		protected virtual void SetValue(T value)
		{
			_value = value;
			ValueChangedHandler?.Invoke(_value);
		}
	}
}
