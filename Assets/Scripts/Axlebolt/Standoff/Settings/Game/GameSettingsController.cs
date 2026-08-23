using Axlebolt.Standoff.Matchmaking;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Settings.Game
{
	public class GameSettingsController : SettingsTabController<GameSettingsController>
	{
		[SerializeField]
		private RegionSelectField _regionField;

		private GameSettings _model;

		public override void OnOpen()
		{
			base.OnOpen();
			_model = GameSettingsManager.Instance.Model;
			_regionField.Value = Regions.GetRegion(_model.Region);
			_regionField.ValueChangedHandler = delegate
			{
				OnDirty();
			};
		}

		protected override IEnumerator ApplyInternal()
		{
			_model.Region = _regionField.Value?.Location;
			yield return GameSettingsManager.Instance.Save(_model);
		}

		public override void ResetDefaults()
		{
			_regionField.Value = null;
		}

		public override void OnClose()
		{
			_regionField.ValueChangedHandler = null;
		}

		public void OpenVk()
		{
			Application.OpenURL("https://vk.com/standoff2_official");
		}

		public void OpenFacebook()
		{
			Application.OpenURL("https://www.facebook.com/Standoff2Official");
		}

		public void OpenTwitter()
		{
			Application.OpenURL("https://twitter.com/so2_official");
		}
	}
}
