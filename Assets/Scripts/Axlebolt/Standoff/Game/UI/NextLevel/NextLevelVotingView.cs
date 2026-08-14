using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.NextLevel
{
	public class NextLevelVotingView : View, IPlayerPropSensitiveView
	{
		private static readonly Log Log = Log.Create(typeof(NextLevelVoteView));

		private static readonly string[] PlayerProps = new string[1]
		{
			"NextLevelVoteView"
		};

		[SerializeField]
		[NotNull]
		[FormerlySerializedAs("timeText")]
		private Text _timeText;

		[NotNull]
		[FormerlySerializedAs("timeText")]
		[SerializeField]
		private Text _headerText;

		[NotNull]
		[SerializeField]
		[FormerlySerializedAs("timeText")]
		private NextLevelView _nextLevelViewPrefab;

		[SerializeField]
		[FormerlySerializedAs("timeText")]
		private Color _defaultColor;

		[SerializeField]
		[FormerlySerializedAs("timeText")]
		private Color _selectedColor;

		[FormerlySerializedAs("timeText")]
		[SerializeField]
		private Color _winnerColor;

		private ViewPool<NextLevelView> _nextLevelsPool;

		private bool _frozen;

		public string Text
		{
			set
			{
				_headerText.text = value;
			}
		}

		public string[] SensitivePlayerProperties
		{
			[CompilerGenerated]
			get
			{
				return PlayerProps;
			}
		}

		public double TimeLeft
		{
			set
			{
				_timeText.text = StringUtils.FormatTwoDigit(value);
			}
		}

		private void Awake()
		{
			_nextLevelsPool = new ViewPool<NextLevelView>(_nextLevelViewPrefab, 5);
		}

		public void Init(LevelDefinition[] levels)
		{
			int num = Math.Min(levels.Length, 5);
			NextLevelView[] items = _nextLevelsPool.GetItems(num);
			for (int i = 0; i < num; i++)
			{
				items[i].Init(levels[i], Vote);
			}
			Refresh();
		}

		public void Refresh()
		{
			if (!_frozen)
			{
				List<int> list = new List<int>();
				NextLevelView[] items = _nextLevelsPool.Items;
				foreach (NextLevelView nextLevelView in items)
				{
					LevelDefinition levelDefinition = nextLevelView.LevelDefinition;
					int item = PhotonNetwork.playerList.Count((PhotonPlayer player) => levelDefinition.name.Equals(player.GetNextLevelVote()));
					list.Add(item);
				}
				Refresh(list.ToArray(), highLightCurrent: true);
			}
		}

		public void Refresh(int[] finalVotes, bool highLightCurrent)
		{
			if (finalVotes.Length != _nextLevelsPool.Items.Length)
			{
				Log.Error($"Invalid finalVotes count, expected {_nextLevelsPool.Items.Length}, actual {finalVotes.Length}");
				return;
			}
			string nextLevelVote = PhotonNetwork.player.GetNextLevelVote();
			for (int i = 0; i < finalVotes.Length; i++)
			{
				NextLevelView nextLevelView = _nextLevelsPool.Items[i];
				LevelDefinition levelDefinition = nextLevelView.LevelDefinition;
				Color color = (!highLightCurrent || !levelDefinition.name.Equals(nextLevelVote)) ? _defaultColor : _selectedColor;
				nextLevelView.NumberOfVotes = finalVotes[i];
				nextLevelView.Color = color;
			}
		}

		public void Frozen(int[] finalVotes)
		{
			_frozen = true;
			Refresh(finalVotes, highLightCurrent: false);
		}

		public void Vote(LevelDefinition levelDefinition)
		{
			if (!_frozen && string.IsNullOrEmpty(PhotonNetwork.player.GetNextLevelVote()))
			{
				PhotonNetwork.player.SetNextLevelVote(levelDefinition.name);
				Log.Debug($"Vote to {levelDefinition.name}");
				Refresh();
			}
		}

		public void HighLightWinner(string winner)
		{
			_nextLevelsPool.Items.First((NextLevelView level) => level.LevelDefinition.name.Equals(winner)).Color = _winnerColor;
		}
	}
}
