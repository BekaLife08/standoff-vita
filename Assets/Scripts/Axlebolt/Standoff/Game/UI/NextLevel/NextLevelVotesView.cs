using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Axlebolt.Standoff.Game.UI.NextLevel
{
	public class NextLevelVotesView : View
	{
		[NotNull]
		[SerializeField]
		[FormerlySerializedAs("voteViewPrefab")]
		private NextLevelVoteView _voteViewPrefab;

		private NextLevelVoteView[] _items;

		private void Awake()
		{
			_items = new NextLevelVoteView[5];
			for (int i = 0; i < _items.Length; i++)
			{
				_items[i] = Object.Instantiate(_voteViewPrefab, _voteViewPrefab.transform.parent, worldPositionStays: false);
				_items[i].IsOn = false;
			}
			_voteViewPrefab.Hide();
		}

		public void SetVoteCount(int count)
		{
			for (int i = 0; i < _items.Length; i++)
			{
				_items[i].IsOn = (i < count);
			}
		}
	}
}
