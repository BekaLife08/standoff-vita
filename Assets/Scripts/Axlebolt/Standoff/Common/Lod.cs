using System;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	[Serializable]
	public class Lod
	{
		[SerializeField]
		private float _width;

		public float Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
			}
		}
	}
}
