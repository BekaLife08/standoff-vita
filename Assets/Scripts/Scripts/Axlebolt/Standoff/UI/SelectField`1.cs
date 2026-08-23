using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public abstract class SelectField<T> : Field<T>
	{
		[SerializeField]
		private Button _leftButton;

		[SerializeField]
		private Button _rightButton;

		[SerializeField]
		private Image _image;

		private int _selectedIndex = -1;

		public T[] Models
		{
			get;
			set;
		}

		public Image Image
		{
			[CompilerGenerated]
			get
			{
				return _image;
			}
		}

		private void Awake()
		{
			_leftButton.onClick.AddListener(OnPrevious);
			_rightButton.onClick.AddListener(OnNext);
		}

		protected override void SetValue(T value)
		{
			base.SetValue(value);
			Format(_image, value);
			UpdateSelectedIndex();
		}

		private void UpdateSelectedIndex()
		{
			_selectedIndex = -1;
			for (int i = 0; i < Models.Length; i++)
			{
				if (object.Equals(Models[i], base.Value))
				{
					_selectedIndex = i;
				}
			}
			if (_selectedIndex == -1)
			{
				throw new Exception($"Invalid value {base.Value}");
			}
		}

		private void OnPrevious()
		{
			_selectedIndex--;
			if (_selectedIndex < 0)
			{
				_selectedIndex = Models.Length - 1;
			}
			SetValue(Models[_selectedIndex]);
		}

		private void OnNext()
		{
			_selectedIndex++;
			if (_selectedIndex >= Models.Length)
			{
				_selectedIndex = 0;
			}
			SetValue(Models[_selectedIndex]);
		}

		protected abstract void Format(Image image, T value);
	}
}
