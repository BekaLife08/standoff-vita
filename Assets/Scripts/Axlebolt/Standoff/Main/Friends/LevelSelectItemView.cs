using System;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	[RequireComponent(typeof(Button))]
	public class LevelSelectItemView : View
	{
		[SerializeField]
		private Text _levelNameText;

		[SerializeField]
		private Image _levelPreviewImage;

		[SerializeField]
		private Image _detailsImage;

		[SerializeField]
		private Color _selectedColor;

		[SerializeField]
		private Color _unselectedColor;

		[SerializeField]
		private Image _checkmarkImage;

		private Action<bool> _callback;

		private void Awake()
		{
			this.GetRequireComponent<Button>().onClick.AddListener(OnSelected);
		}

		private void OnSelected()
		{
			bool flag = !_checkmarkImage.gameObject.activeSelf;
			_checkmarkImage.gameObject.SetActive(flag);
			_detailsImage.color = ((!flag) ? _unselectedColor : _selectedColor);
			if (_callback != null)
			{
				_callback(flag);
			}
		}

		public void SetLevel([NotNull] LevelDefinition levelDefinition, bool selected, [NotNull] Action<bool> callback)
		{
			if (levelDefinition == null)
			{
				throw new ArgumentNullException("levelDefinition");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			_levelNameText.text = levelDefinition.DisplayName;
			_levelPreviewImage.sprite = levelDefinition.PreviewImage;
			_checkmarkImage.gameObject.SetActive(selected);
			_detailsImage.color = ((!selected) ? _unselectedColor : _selectedColor);
			_callback = callback;
		}
	}
}
