using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncResult
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly bool _003CSuccess_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly Exception _003CException_003Ek__BackingField;

		public bool Success
		{
			[CompilerGenerated]
			get
			{
				return _003CSuccess_003Ek__BackingField;
			}
		}

		public Exception Exception
		{
			[CompilerGenerated]
			get
			{
				return _003CException_003Ek__BackingField;
			}
		}

		internal AsyncResult()
		{
			_003CSuccess_003Ek__BackingField = true;
		}

		internal AsyncResult([NotNull] Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			_003CSuccess_003Ek__BackingField = false;
			_003CException_003Ek__BackingField = exception;
		}
	}
}
