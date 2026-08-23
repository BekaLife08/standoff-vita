using Axlebolt.Standoff.Player.Networking;
using Photon;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player
{
	[RequireComponent(typeof(CharacterPlayer))]
	[RequireComponent(typeof(PlayerController))]
	public class NetworkController : PunBehaviour, IPunObservable, IPlayerComponent
	{
		private CharacterPlayer _characterPlayer;

		private PlayerController _playerController;

		private PhotonView _photonView;

		private void Awake()
		{
			_characterPlayer = this.GetRequireComponent<CharacterPlayer>();
			_playerController = this.GetRequireComponent<PlayerController>();
			_photonView = this.GetRequireComponent<PhotonView>();
		}

		public void PreInitialize()
		{
		}

		public void Initialize()
		{
		}

		public override void OnPhotonInstantiate(PhotonMessageInfo info)
		{
			if (_photonView.isMine)
			{
				_photonView.synchronization = ViewSynchronization.Off;
			}
		}

		public void OnInstantiated()
		{
			if (_photonView.isMine)
			{
				_photonView.synchronization = ViewSynchronization.Unreliable;
			}
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				PlayerSnapshot snapshot = _playerController.GetSnapshot();
				NetworkWriter networkWriter = new NetworkWriter();
				snapshot.Serialize(networkWriter);
				byte[] obj = networkWriter.AsArray();
				stream.SendNext(obj);
			}
			else
			{
				byte[] buffer = (byte[])stream.ReceiveNext();
				NetworkReader reader = new NetworkReader(buffer);
				PlayerSnapshot playerSnapshot = new PlayerSnapshot();
				playerSnapshot.Deserialize(reader);
				_characterPlayer.AddSnapshot(playerSnapshot);
			}
		}
	}
}
