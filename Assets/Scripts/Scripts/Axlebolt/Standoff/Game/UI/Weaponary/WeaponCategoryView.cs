using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	[RequireComponent(typeof(Button), typeof(Image))]
	public class WeaponCategoryView : View
	{
		[NotNull]
		[SerializeField]
		private Text _nameText;

		[SerializeField]
		[NotNull]
		private Sprite _notSelectedSprite;

		[NotNull]
		[SerializeField]
		private Sprite _selectedSprite;

		private Button _button;

		private Image _image;

		private bool _selected;

		public Action OnSelected
		{
			get;
			set;
		}

		public string Name
		{
			set
			{
				_nameText.text = value;
			}
		}

		public bool IsSelected
		{
			get
			{
				return _selected;
			}
			set
			{
				if (_selected != value)
				{
					_selected = value;
					if (_selected)
					{
						_image.sprite = _selectedSprite;
						OnSelected();
					}
					else
					{
						_image.sprite = _notSelectedSprite;
					}
				}
			}
		}

		private void Awake()
		{
			_button = GetComponent<Button>();
			_image = GetComponent<Image>();
			_button.onClick.AddListener(delegate
			{
				IsSelected = true;
			});
			_image.sprite = _notSelectedSprite;
			_selected = false;
		}
	}
}
