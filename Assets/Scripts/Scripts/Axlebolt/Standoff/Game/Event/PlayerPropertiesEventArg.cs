using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Standoff.Game.Event
{
	public class PlayerPropertiesEventArg
	{
		public IEnumerable<object> ChangedProperties
		{
			get;
		}

		public PhotonPlayer Player
		{
			get;
		}

		public PlayerPropertiesEventArg([NotNull] IEnumerable<object> changedProperties, [NotNull] PhotonPlayer player)
		{
			if (changedProperties == null)
			{
				throw new ArgumentNullException("changedProperties");
			}
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			ChangedProperties = changedProperties;
			Player = player;
		}

		public bool Contains(object property)
		{
			return ChangedProperties.Contains(property);
		}
	}
}
