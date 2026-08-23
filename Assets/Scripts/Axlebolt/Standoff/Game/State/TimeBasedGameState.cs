using ExitGames.Client.Photon;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public abstract class TimeBasedGameState : IGameState
	{
		protected readonly double Duration;

		public abstract byte Id
		{
			get;
		}

		protected double TimeLeft
		{
			[CompilerGenerated]
			get
			{
				return GameStateHelper.GetTimeLeft(Duration);
			}
		}

		protected TimeBasedGameState(double duration)
		{
			Duration = duration;
		}

		public abstract void Setup(GameController gameController);

		public virtual Hashtable Init()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.SetTime(PhotonNetwork.time);
			return hashtable;
		}

		public abstract void Enter();

		public virtual YieldInstruction Instruction()
		{
			return new WaitForSeconds(0.2f);
		}

		public abstract void Update();

		public virtual bool CheckExit()
		{
			return GameStateHelper.GetTimePassed() > Duration;
		}

		public abstract void Exit();

		public abstract void Complete();

		public abstract bool CanSpawnPlayer();
	}
}
