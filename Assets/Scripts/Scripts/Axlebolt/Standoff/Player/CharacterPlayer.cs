using Axlebolt.Networking;
using Axlebolt.Standoff.Player.Networking;
using Axlebolt.Standoff.Player.Occlusion;
using System.Collections;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	[RequireComponent(typeof(PlayerController))]
	public class CharacterPlayer : ObjectPlayer
	{
		private readonly PlayerInterpolator _playerInterpolator = new PlayerInterpolator();

		private PlayerController _playerController;

		private PlayerOcclusionController _occlusionController;

		private bool _isCharacterVisible;

		public override void PreInitialize()
		{
			base.PreInitialize();
			_playerController = this.GetRequireComponent<PlayerController>();
			_occlusionController = this.GetRequireComponent<PlayerOcclusionController>();
			_occlusionController.OnOcclusionBecameInvisible += OnOcclusionBecameInvisible;
			_occlusionController.OnOcclusionBecameVisible += OnOcclusionBecameVisible;
		}

		public override void Initialize()
		{
			base.Initialize();
			_playerInterpolator.ObsrvationState = ObservationState.Visible;
		}

		public override void Clear()
		{
			base.Clear();
			_isCharacterVisible = false;
		}

		public override Interpolator GetInterpolator()
		{
			return _playerInterpolator;
		}

		protected override void SetSnapshot(ObjectSnapshot snapshot)
		{
			_playerController.SetSnapsot((PlayerSnapshot)snapshot);
			if (!_isCharacterVisible)
			{
				_isCharacterVisible = true;
				StartCoroutine(EnableCharacter());
			}
		}

		private void OnOcclusionBecameInvisible()
		{
			_playerInterpolator.ObsrvationState = ObservationState.Invisible;
		}

		private void OnOcclusionBecameVisible()
		{
			_playerInterpolator.ObsrvationState = ObservationState.Visible;
			Evaluate();
		}

		private IEnumerator EnableCharacter()
		{
			yield return null;
			_playerController.SetCharacterVisible(isEnabled: true);
		}
	}
}
