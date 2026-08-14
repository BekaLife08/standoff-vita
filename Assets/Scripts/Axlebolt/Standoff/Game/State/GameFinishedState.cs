using ExitGames.Client.Photon;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public class GameFinishedState : IGameState
	{
		public byte Id
		{
			[CompilerGenerated]
			get
			{
				return byte.MaxValue;
			}
		}

		public void Setup([NotNull] GameController gameController)
		{
		}

		public Hashtable Init()
		{
			return new Hashtable();
		}

		public void Enter()
		{
		}

		public YieldInstruction Instruction()
		{
			throw new NotImplementedException();
		}

		public void Update()
		{
			throw new NotImplementedException();
		}

		public bool CheckExit()
		{
			throw new NotImplementedException();
		}

		public void Exit()
		{
			throw new NotImplementedException();
		}

		public void Complete()
		{
			throw new NotImplementedException();
		}

		public bool CanSpawnPlayer()
		{
			throw new NotImplementedException();
		}
	}
}
