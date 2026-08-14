using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	[CreateAssetMenu(fileName = "AvatarConfig", menuName = "Standoff/Create Avatar Config")]
	public class AvatarConfig : ScriptableObject
	{
		[SerializeField]
		private Texture2D _emptyAvatar;

		public Texture2D EmptyAvatar
		{
			[CompilerGenerated]
			get
			{
				return _emptyAvatar;
			}
		}
	}
}
