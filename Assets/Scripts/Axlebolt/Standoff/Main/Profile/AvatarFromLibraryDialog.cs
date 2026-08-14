using System;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class AvatarFromLibraryDialog : BaseDialog
	{
		private Action<Sprite> _callback;

		[SerializeField]
		private GameObject _avatarsContainer;

		protected override void Awake()
		{
			base.Awake();
			Button[] componentsInChildren = _avatarsContainer.GetComponentsInChildren<Button>();
			Button[] array = componentsInChildren;
			foreach (Button button in array)
			{
				Image image = button.GetRequireComponent<Image>();
				button.onClick.AddListener(delegate
				{
					_callback(image.sprite);
					Hide();
				});
			}
		}

		public void Show([NotNull] Action<Sprite> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			_callback = callback;
			Show();
		}

		public override void Hide()
		{
			base.Hide();
			if (_callback != null)
			{
				_callback(null);
			}
		}
	}
}
