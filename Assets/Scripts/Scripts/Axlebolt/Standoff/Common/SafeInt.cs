using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class SafeInt
	{
		private readonly int _salt;

		private int _value;

		public int Value
		{
			get
			{
				return _value - _salt;
			}
			set
			{
				_value = value + _salt;
			}
		}

		public SafeInt()
		{
			_salt = Random.Range(0, 10);
			_value = _salt;
		}
	}
}
