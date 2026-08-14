using System;

namespace Axlebolt.Standoff.Effects
{
	public class MuzzleFlashId : IEquatable<MuzzleFlashId>
	{
		public readonly MuzzleFlashType Type;

		public readonly bool IsLocal;

		public MuzzleFlashId(MuzzleFlashType type, bool isLocal)
		{
			Type = type;
			IsLocal = isLocal;
		}

		public bool Equals(MuzzleFlashId other)
		{
			return Type == other.Type && IsLocal == other.IsLocal;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((MuzzleFlashId)obj);
		}

		public override int GetHashCode()
		{
			return ((int)Type * 397) ^ IsLocal.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}", "Type", Type, "IsLocal", IsLocal);
		}
	}
}
