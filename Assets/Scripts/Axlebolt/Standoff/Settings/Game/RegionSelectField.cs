using Axlebolt.Standoff.Matchmaking;
using Axlebolt.Standoff.UI;
using I2.Loc;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Settings.Game
{
	public class RegionSelectField : Field<PhotonServer>
	{
		[SerializeField]
		private RegionSelectDialog _regionSelectDialog;

		[SerializeField]
		private Text _label;

		[SerializeField]
		private Button _selectButton;

		private string _pattern;

		private void Awake()
		{
			_pattern = _label.text;
			_regionSelectDialog.SaveSelection = false;
			_selectButton.onClick.AddListener(delegate
			{
				StartCoroutine(ShowDialogCoroutine());
			});
		}

		private IEnumerator ShowDialogCoroutine()
		{
			yield return _regionSelectDialog.ShowAndWait();
			if (_regionSelectDialog.Server != null)
			{
				SetValue(_regionSelectDialog.Server);
			}
		}

		protected override void SetValue(PhotonServer value)
		{
			base.SetValue(value);
			_label.text = string.Format(_pattern, ScriptLocalization.SearchGame.Region, (value == null) ? ScriptLocalization.Common.NotSelected : value.GetDisplayName());
		}
	}
}
