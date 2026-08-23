using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Photon
{
	public class PhotonConnectionFailedException : PhotonException
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly DisconnectCause _003CDisconnectCause_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly bool _003CHasCause_003Ek__BackingField;

		public DisconnectCause DisconnectCause
		{
			[CompilerGenerated]
			get
			{
				return _003CDisconnectCause_003Ek__BackingField;
			}
		}

		public bool HasCause
		{
			[CompilerGenerated]
			get
			{
				return _003CHasCause_003Ek__BackingField;
			}
		}

		public PhotonConnectionFailedException()
		{
			_003CHasCause_003Ek__BackingField = false;
		}

		public PhotonConnectionFailedException(DisconnectCause disconnectCause)
		{
			_003CDisconnectCause_003Ek__BackingField = disconnectCause;
			_003CHasCause_003Ek__BackingField = true;
		}
	}
}
