using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.NextLevel
{
	[RequireComponent(typeof(Button), typeof(Image))]
	public class NextLevelView : View
	{
		[NotNull]
		[FormerlySerializedAs("levelNameText")]
		[SerializeField]
		private Text _levelNameText;

		[FormerlySerializedAs("votesView")]
		[NotNull]
		[SerializeField]
		private NextLevelVotesView _votesViewView;

		private Image _image;

		private Action<LevelDefinition> _voteCallback;

		public int NumberOfVotes
		{
			set
			{
				_votesViewView.SetVoteCount(value);
			}
		}

		public Color Color
		{
			set
			{
				_image.color = value;
			}
		}

		public LevelDefinition LevelDefinition
		{
			get;
			private set;
		}

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(Vote);
			_image = this.GetRequireComponent<Image>();
		}

		public void Init([NotNull] LevelDefinition levelDefinition, [NotNull] Action<LevelDefinition> voteCallback)
		{
			if (levelDefinition == null)
			{
				throw new ArgumentNullException("levelDefinition");
			}
			if (voteCallback == null)
			{
				throw new ArgumentNullException("voteCallback");
			}
			LevelDefinition = levelDefinition;
			_voteCallback = voteCallback;
			_levelNameText.text = levelDefinition.DisplayName;
		}

		private void Vote()
		{
			if (LevelDefinition == null)
			{
				throw new InvalidOperationException("View not initialized, levelDefiniton is null");
			}
			_voteCallback(LevelDefinition);
		}
	}
}
