using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Core
{
	public class Log
	{
		private readonly string _tag;

		private readonly ILogger _logger;

		public bool DebugEnabled
		{
			[CompilerGenerated]
			get
			{
				return true;
			}
		}

		public Log(string tag)
		{
			_tag = tag;
			_logger = UnityEngine.Debug.unityLogger;
			_logger.logEnabled = true;
		}

		public static Log Create(Type type)
		{
			return new Log(type.Name);
		}

		public static Log Create(string tag)
		{
			return new Log(tag);
		}

		public void Debug(object message)
		{
			if (DebugEnabled)
			{
				_logger.Log(_tag, message);
			}
		}

		public void Debug(object message, UnityEngine.Object context)
		{
			if (DebugEnabled)
			{
				_logger.Log(_tag, message, context);
			}
		}

		public void Warning(string message, UnityEngine.Object context, params object[] args)
		{
			if (DebugEnabled)
			{
				_logger.LogWarning(_tag, FormatMessage(message, args), context);
			}
		}

		public void Warning(string message, params object[] args)
		{
			if (DebugEnabled)
			{
				_logger.LogWarning(_tag, FormatMessage(message, args));
			}
		}

		public void Error(object message, UnityEngine.Object context)
		{
			if (DebugEnabled)
			{
				_logger.LogError(_tag, message, context);
			}
		}

		public void Error(object message)
		{
			if (DebugEnabled)
			{
				_logger.LogError(_tag, message);
			}
		}

		private static string FormatMessage(string message, params object[] args)
		{
			if (args.Length > 0)
			{
				message = string.Format(message, args);
			}
			return message;
		}
	}
}
