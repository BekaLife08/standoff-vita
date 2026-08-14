using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Inventory.Bomb;
using ExitGames.Client.Photon;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Defuse
{
	public class DefuseBombPlantedState : IGameState
	{
		private readonly float _delayAfterDefuse;

		public byte Id
		{
			[CompilerGenerated]
			get
			{
				return 40;
			}
		}

		public DefuseBombPlantedState(float delayAfterDefuse)
		{
			_delayAfterDefuse = delayAfterDefuse;
		}

		public void Setup(GameController gameController)
		{
		}

		public Hashtable Init()
		{
			return new Hashtable();
		}

		public void Enter()
		{
			ScenePhotonBehavior<GameStateHelper>.Instance.ClearRpc();
		}

		public YieldInstruction Instruction()
		{
			return new WaitForEndOfFrame();
		}

		public void Update()
		{
		}

		public bool CheckExit()
		{
			if (ScenePhotonBehavior<BombManager>.Instance.IsDone())
			{
				if (ScenePhotonBehavior<BombManager>.Instance.IsDefused)
				{
					return PhotonNetwork.time - ScenePhotonBehavior<BombManager>.Instance.DefuseTime >= (double)_delayAfterDefuse;
				}
				return true;
			}
			if (PhotonNetwork.playerList.IsAllDead(Team.Ct))
			{
				return ScenePhotonBehavior<GameStateHelper>.Instance.ServerCallback();
			}
			return false;
		}

		public void Exit()
		{
		}

		public void Complete()
		{
		}

		public bool CanSpawnPlayer()
		{
			return false;
		}
	}
}
