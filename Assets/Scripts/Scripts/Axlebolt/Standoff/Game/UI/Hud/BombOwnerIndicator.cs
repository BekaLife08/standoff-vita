using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Controls;
using Axlebolt.Standoff.Inventory.Bomb;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Hud
{
	public class BombOwnerIndicator : HudComponentView
	{
		private const string Default = "Default";

		private const string BombStart = "BombStart";

		private const string BombSite = "BombSite";

		private Animator _animator;

		private bool _isBombOwner;

		private bool _isInBombSite;

		private void Awake()
		{
			Hide();
			_animator = this.GetRequireComponent<Animator>();
		}

		public override void SetPlayerController(PlayerController playerController)
		{
			if (playerController == null)
			{
				Hide();
			}
		}

		public override void UpdateView(PlayerController playerController)
		{
			if (ScenePhotonBehavior<BombManager>.IsInitialized())
			{
				if (!_isBombOwner && ScenePhotonBehavior<BombManager>.Instance.IsBomber(playerController.PhotonView))
				{
					base.gameObject.SetActive(value: true);
					_animator.SetTrigger("BombStart");
					_isBombOwner = true;
					_isInBombSite = false;
				}
				else if (_isBombOwner && !ScenePhotonBehavior<BombManager>.Instance.IsBomber(playerController.PhotonView))
				{
					Hide();
				}
				if (_isBombOwner && !_isInBombSite && ScenePhotonBehavior<BombManager>.Instance.IsInBombSite(playerController.Transform))
				{
					_animator.SetTrigger("BombSite");
					_isInBombSite = true;
				}
				else if (_isBombOwner && _isInBombSite && !ScenePhotonBehavior<BombManager>.Instance.IsInBombSite(playerController.Transform))
				{
					_animator.SetTrigger("Default");
					_isInBombSite = false;
				}
			}
		}

		public override void Hide()
		{
			base.Hide();
			_isBombOwner = false;
			_isInBombSite = false;
		}
	}
}
