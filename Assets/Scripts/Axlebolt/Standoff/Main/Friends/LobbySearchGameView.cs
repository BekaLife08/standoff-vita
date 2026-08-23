using Axlebolt.Standoff.Matchmaking;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Friends
{
	public class LobbySearchGameView : SearchGameView
	{
		[SerializeField]
		private GameObject[] _hideItems;

		public override void Show()
		{
			base.Show();
			GameObject[] hideItems = _hideItems;
			foreach (GameObject gameObject in hideItems)
			{
				gameObject.gameObject.SetActive(false);
			}
		}

		public override void Hide()
		{
			base.Hide();
			GameObject[] hideItems = _hideItems;
			foreach (GameObject gameObject in hideItems)
			{
				gameObject.gameObject.SetActive(true);
			}
		}

		public void SetIsLobbyOwner(bool isLobbyOwner)
		{
			base.CancelButton.gameObject.SetActive(isLobbyOwner);
		}
	}
}
