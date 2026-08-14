using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Winners
{
	public class WinnersView : View
	{
		[SerializeField]
		private Image _backgroundImage1;

		[SerializeField]
		private Image _backgroundImage2;

		[SerializeField]
		private Text _titleText;

		[SerializeField]
		private Color _ctBackground1Color;

		[SerializeField]
		private Color _ctBackground2Color;

		[SerializeField]
		private Color _trBackground1Color;

		[SerializeField]
		private Color _trBackground2Color;

		[SerializeField]
		private Color _ctTitleColor;

		[SerializeField]
		private Color _trTitleColor;

		[SerializeField]
		private WinPlayersView _winPlayers;

		[SerializeField]
		private WinTeamView _winTeamView;

		public void Show([NotNull] PhotonPlayer firstPlace, PhotonPlayer secondPlace, PhotonPlayer thirdPlace)
		{
			ApplyTeam(firstPlace.GetTeam());
			_winTeamView.Hide();
			_winPlayers.Show(firstPlace, secondPlace, thirdPlace);
			Show();
		}

		public void Show(Team winTeam, PhotonPlayer mvpPlayer, string mvpMessage)
		{
			ApplyTeam(winTeam);
			_winPlayers.Hide();
			_winTeamView.Show(winTeam, mvpPlayer, mvpMessage);
			Show();
		}

		private void ApplyTeam(Team team)
		{
			if (team == Team.Ct)
			{
				_backgroundImage1.color = _ctBackground1Color;
				_backgroundImage2.color = _ctBackground2Color;
				_titleText.color = _ctTitleColor;
			}
			else
			{
				_backgroundImage1.color = _trBackground1Color;
				_backgroundImage2.color = _trBackground2Color;
				_titleText.color = _trTitleColor;
			}
		}

		public override void Hide()
		{
			_winPlayers.Hide();
			base.Hide();
		}
	}
}
