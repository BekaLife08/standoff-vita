using Axlebolt.Standoff.Bots;
using Axlebolt.Standoff.Player.Aim;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Ragdoll
{
	public class PlayerRagdollController : MonoBehaviour, IFallingCharacter
	{
		private PlayerController _playerController;

		private AimController _aimController;

		private FallingCharacterConfig _config = new FallingCharacterConfig();

		public FallingCharacterConfig Config
		{
			get
			{
				return _config;
			}
			set
			{
				_config = value;
			}
		}

		public void PreInitialize()
		{
			_playerController = GetComponent<PlayerController>();
			_aimController = _playerController.AimController;
		}

		public FallingCharacterConfig GetFallingCharacterConfig()
		{
			return _config;
		}

		public string GetName()
		{
			return base.gameObject.name;
		}

		public BipedMap GetBipedMap()
		{
			return _playerController.BipedMap;
		}

		public Vector3 GetCharacterVelocity()
		{
			return _playerController.MovementController.Velocity;
		}

		public int GetPlayerId()
		{
			return _playerController.PlayerId;
		}

		public void OnSimulateFalling()
		{
			BotAI botAI = GetComponent<BotAI>();
			if (botAI != null)
			{
				PlayerController playerController = GetComponent<PlayerController>();
				if (playerController != null)
				{
					playerController.SetCharacterVisible(isEnabled: false);
					return;
				}
			}
			base.gameObject.SetActive(value: false);
		}
	}
}
