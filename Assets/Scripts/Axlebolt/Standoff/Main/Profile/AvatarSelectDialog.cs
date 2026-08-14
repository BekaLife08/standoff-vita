using System;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using DeadMosquito.AndroidGoodies;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class AvatarSelectDialog : BaseDialog
	{
		private static readonly Log Log = Log.Create(typeof(AvatarSelectDialog));

		[SerializeField]
		private AvatarFromLibraryDialog _libraryView;

		[SerializeField]
		private Button _loadFromGallery;

		[SerializeField]
		private Button _chooseFromLibrary;

		private Action<Texture2D> _callback;

		private Texture2D _avatar;

		public Action LoadFromGalleryHandler { get; set; }

		public Action ChooseFromLibraryHandler { get; set; }

		protected override void Awake()
		{
			base.Awake();
			_loadFromGallery.onClick.AddListener(LoadFromGallery);
			_chooseFromLibrary.onClick.AddListener(ChooseFromLibrary);
			_libraryView.Hide();
			_libraryView.VisibleChanged += delegate(bool isVisible)
			{
				base.gameObject.SetActive(!isVisible);
			};
		}

		public void Show([NotNull] Action<Texture2D> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			_callback = callback;
			Show();
		}

		protected override void OnClose()
		{
			Result(null);
		}

		private void LoadFromGallery()
		{
			if (Application.isEditor)
			{
				Result(null);
				return;
			}
			Log.Debug("NativeGalleryController call");
			AGGallery.PickImageFromGallery(delegate(ImagePickResult selectedImage)
			{
				Texture2D avatar = selectedImage.LoadTexture2D();
				Result(avatar);
				Resources.UnloadUnusedAssets();
			}, delegate(string errorMessage)
			{
				AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage);
			});
		}

		private void ChooseFromLibrary()
		{
			_libraryView.Show(delegate(Sprite avatar)
			{
				if (avatar == null)
				{
					Result(null);
				}
				else
				{
					Texture2D texture2D = new Texture2D((int)avatar.rect.width, (int)avatar.rect.height);
					Color[] pixels = avatar.texture.GetPixels((int)avatar.textureRect.x, (int)avatar.textureRect.y, (int)avatar.textureRect.width, (int)avatar.textureRect.height);
					texture2D.SetPixels(pixels);
					texture2D.Apply();
					Result(texture2D);
				}
			});
		}

		private void Result(Texture2D avatar)
		{
			if (avatar == null)
			{
				Log.Debug("Avatar selection canceled");
				Hide();
				return;
			}
			Log.Debug("Avatar successfully selected");
			TextureUtility.Bilinear(avatar, 192, 192);
			_avatar = avatar;
			Hide();
		}

		public override void Hide()
		{
			if (_callback != null)
			{
				_callback(_avatar);
			}
			_callback = null;
			_avatar = null;
			base.Hide();
		}
	}
}
