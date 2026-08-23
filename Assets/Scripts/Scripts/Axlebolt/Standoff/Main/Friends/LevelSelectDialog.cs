using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LevelSelectDialog : View
	{
		[SerializeField]
		private Button _applyButton;

		[SerializeField]
		private Button _closeButton;

		[SerializeField]
		private GridLayoutGroup _layout;

		[SerializeField]
		private LevelSelectItemView _itemTemplate;

		private readonly List<LevelSelectItemView> _items = new List<LevelSelectItemView>();

		private readonly HashSet<string> _selectedLevels = new HashSet<string>();

		private Action<string[]> _callback;

		private void Awake()
		{
			_applyButton.onClick.AddListener(Apply);
			_closeButton.onClick.AddListener(Hide);
			_itemTemplate.gameObject.SetActive(false);
		}

		public void Show([NotNull] LevelDefinition[] levels, [NotNull] string[] selectedLevels, [NotNull] Action<string[]> callback)
		{
			if (levels == null)
			{
				throw new ArgumentNullException("levels");
			}
			if (selectedLevels == null)
			{
				throw new ArgumentNullException("selectedLevels");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (_items.Count != 0 || _selectedLevels.Count != 0)
			{
				throw new Exception("Dialog already shown");
			}
			_callback = callback;
			_selectedLevels.UnionWith(selectedLevels);
			if (levels.Length == 3)
			{
				_layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
				_layout.constraintCount = 3;
			}
			foreach (LevelDefinition levelDefinition in levels)
			{
				LevelSelectItemView levelSelectItemView = UnityEngine.Object.Instantiate(_itemTemplate, _layout.transform, false);
				levelSelectItemView.gameObject.SetActive(true);
				levelSelectItemView.SetLevel(levelDefinition, _selectedLevels.Contains(levelDefinition.name), delegate(bool selected)
				{
					if (selected)
					{
						_selectedLevels.Add(levelDefinition.name);
					}
					else
					{
						_selectedLevels.Remove(levelDefinition.name);
					}
				});
				_items.Add(levelSelectItemView);
			}
			base.Show();
		}

		public override void Hide()
		{
			base.Hide();
			foreach (LevelSelectItemView item in _items)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			_items.Clear();
			_selectedLevels.Clear();
		}

		private void Apply()
		{
			_callback(_selectedLevels.ToArray());
			Hide();
		}
	}
}
