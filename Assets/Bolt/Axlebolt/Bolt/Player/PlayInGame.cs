using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt.Player
{
	public class PlayInGame
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CGameCode_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CGameVersion_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CLobbyId_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly BoltPhotonGame _003CPhotonGame_003Ek__BackingField;

		public string GameCode
		{
			[CompilerGenerated]
			get
			{
				return _003CGameCode_003Ek__BackingField;
			}
		}

		public string GameVersion
		{
			[CompilerGenerated]
			get
			{
				return _003CGameVersion_003Ek__BackingField;
			}
		}

		public string LobbyId
		{
			[CompilerGenerated]
			get
			{
				return _003CLobbyId_003Ek__BackingField;
			}
		}

		public BoltPhotonGame PhotonGame
		{
			[CompilerGenerated]
			get
			{
				return _003CPhotonGame_003Ek__BackingField;
			}
		}

		public PlayInGame(string gameCode, string gameVersion, string lobbyId, BoltPhotonGame boltPhotonGame)
		{
			if (gameCode == null)
			{
				throw new ArgumentNullException("gameCode");
			}
			if (gameVersion == null)
			{
				throw new ArgumentNullException("gameVersion");
			}
			_003CGameCode_003Ek__BackingField = gameCode;
			_003CGameVersion_003Ek__BackingField = gameVersion;
			_003CLobbyId_003Ek__BackingField = lobbyId;
			_003CPhotonGame_003Ek__BackingField = boltPhotonGame;
		}
	}
}
