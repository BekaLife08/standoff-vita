using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class SafeFloat
	{
		private readonly float _salt;

		private float _value;

		public float Value
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

		public SafeFloat()
		{
			_salt = Random.Range(0, 10);
			_value = _salt;
		}

		public SafeFloat(float value)
			: this()
		{
			Value = value;
		}
	}
}
