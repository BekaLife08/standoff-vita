using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class RadioButtonGroup<T> : Field<T>
	{
		[SerializeField]
		private RadioButtonGroupTemplate _prefab;

		private RadioButtonGroupTemplate[] _items;

		private RadioButtonGroupTemplate _selectedItem;

		private T[] _models = new T[0];

		private Func<T, string> _formatter;

		public T[] Models
		{
			get
			{
				return _models;
			}
			set
			{
				SetModels(value);
			}
		}

		private void SetModels(T[] models)
		{
			_models = models;
			if (_items != null)
			{
				RadioButtonGroupTemplate[] items = _items;
				foreach (RadioButtonGroupTemplate radioButtonGroupTemplate in items)
				{
					UnityEngine.Object.Destroy(radioButtonGroupTemplate.gameObject);
				}
			}
			_items = new RadioButtonGroupTemplate[_models.Length];
			if (_formatter == null)
			{
				InitFormatter();
			}
			_prefab.Hide();
			for (int j = 0; j < _models.Length; j++)
			{
				T model = models[j];
				_items[j] = UnityEngine.Object.Instantiate(_prefab, base.transform, worldPositionStays: false);
				_items[j].Text = _formatter(model);
				_items[j].Selected = false;
				_items[j].ActionHandler = delegate
				{
					SetValue(model);
				};
				_items[j].Show();
			}
		}

		protected virtual void InitFormatter()
		{
			_formatter = ((T m) => m.ToString());
		}

		protected override void SetValue(T value)
		{
			for (int i = 0; i < _models.Length; i++)
			{
				if (object.Equals(_models[i], value))
				{
					if (_selectedItem != null)
					{
						_selectedItem.Selected = false;
					}
					_items[i].Selected = true;
					_selectedItem = _items[i];
					base.SetValue(value);
					return;
				}
			}
			throw new ArgumentException($"{value} incorrect value");
		}

		public void SetFormatter([NotNull] Func<T, string> formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			_formatter = formatter;
		}
	}
}
