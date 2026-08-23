using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Photon
{
	public class PhotonJoinRoomException : PhotonException
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly short _003CCode_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CDescription_003Ek__BackingField;

		public short Code
		{
			[CompilerGenerated]
			get
			{
				return _003CCode_003Ek__BackingField;
			}
		}

		public string Description
		{
			[CompilerGenerated]
			get
			{
				return _003CDescription_003Ek__BackingField;
			}
		}

		public PhotonJoinRoomException(short code, string description)
			: base(string.Format("{0} ({1})", code, description))
		{
			_003CCode_003Ek__BackingField = code;
			_003CDescription_003Ek__BackingField = description;
		}
	}
}
