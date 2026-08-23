using System;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class PlayerNameChangeDialog : BaseDialog
	{
		[SerializeField]
		private InputField _inputField;

		[SerializeField]
		private Button _okButton;

		public string Name
		{
			get
			{
				return _inputField.text;
			}
			set
			{
				_inputField.text = value;
			}
		}

		public Action ActionHandler { get; set; }

		protected override void Awake()
		{
			base.Awake();
			_okButton.onClick.AddListener(delegate
			{
				Action actionHandler = ActionHandler;
				if (actionHandler != null)
				{
					actionHandler();
				}
				Hide();
			});
		}
	}
}
