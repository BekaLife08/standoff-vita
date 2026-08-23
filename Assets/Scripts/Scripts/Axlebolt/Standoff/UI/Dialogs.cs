using Axlebolt.Standoff.Common;
using I2.Loc;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class Dialogs : MonoBehaviour
	{
		private class OkButton : DialogButton
		{
			public string Name
			{
				[CompilerGenerated]
				get
				{
					return ScriptLocalization.Common.Ok;
				}
			}
		}

		private class CancelButton : DialogButton
		{
			public string Name
			{
				[CompilerGenerated]
				get
				{
					return ScriptLocalization.Common.Cancel;
				}
			}
		}

		private static Dialogs _instance;

		public static readonly DialogButton Ok = new OkButton();

		public static readonly DialogButton Cancel = new CancelButton();

		[SerializeField]
		private Dialog _dialogPrefab;

		private static Dialogs Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject original = ResourcesUtility.Load<GameObject>("UI/Dialogs");
					_instance = UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity).GetRequireComponent<Dialogs>();
				}
				return _instance;
			}
		}

		public static Dialog Create()
		{
			return Instance.CreateDialog();
		}

		public static Dialog Create(string title, string contentText, params DialogButton[] buttons)
		{
			Dialog dialog = Create();
			dialog.Title = title;
			dialog.ContentText = contentText;
			dialog.AddButtons(buttons);
			return dialog;
		}

		public static Dialog Create(string title, Sprite content, params DialogButton[] buttons)
		{
			Dialog dialog = Create();
			dialog.Title = title;
			dialog.ContentSprite = content;
			dialog.AddButtons(buttons);
			return dialog;
		}

		public static Dialog Message([NotNull] string title, [NotNull] string message, Action okAction = null)
		{
			if (title == null)
			{
				throw new ArgumentNullException("title");
			}
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			Dialog dialog = Create(title, message, new DialogButton(ScriptLocalization.Common.Ok));
			dialog.Show(delegate
			{
				okAction?.Invoke();
			});
			return dialog;
		}

		public static Dialog Confirm([NotNull] string title, [NotNull] string message)
		{
			return Create(title, message, Ok, Cancel);
		}

		private void Awake()
		{
			_dialogPrefab.gameObject.SetActive(value: false);
		}

		private Dialog CreateDialog()
		{
			Dialog dialog = UnityEngine.Object.Instantiate(_dialogPrefab, base.transform, worldPositionStays: false);
			dialog.gameObject.SetActive(value: true);
			dialog.gameObject.SetActive(value: false);
			return dialog;
		}

		private void OnDestroy()
		{
			_instance = null;
		}
	}
}
