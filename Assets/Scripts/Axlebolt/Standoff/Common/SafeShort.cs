using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class SafeShort
	{
		private readonly int _salt;

		private int _value;

		public short Value
		{
			get
			{
				return (short)(_value - _salt);
			}
			set
			{
				_value = value + _salt;
			}
		}

		public SafeShort()
		{
			_salt = (short)Random.Range(0, 10);
			_value = _salt;
		}

		public SafeShort(short value)
			: this()
		{
			Value = value;
		}
	}
}
