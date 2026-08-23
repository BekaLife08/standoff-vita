using Axlebolt.Standoff.Player;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Level
{
	[CreateAssetMenu(fileName = "NewLevel", menuName = "Standoff/Create Level", order = 1)]
	public class LevelDefinition : ScriptableObject
	{
		[SerializeField]
		private string _displayName;

		[SerializeField]
		private Sprite _previewImage;

		[SerializeField]
		private Sprite _fullScreenImage;

		[SerializeField]
		private Sprite _schemaImage;

		[SceneName]
		[SerializeField]
		private string _sceneName;

		[SerializeField]
		private GameModes _gameModes;

		[SerializeField]
		[PlayerArms]
		[Header("Counter Terrorist")]
		[Space]
		private string _ctArms;

		[SerializeField]
		private PlayerCharacters _ctCharacters;

		[PlayerArms]
		[SerializeField]
		[Space]
		[Header("Terrorist")]
		private string _trArms;

		[SerializeField]
		private PlayerCharacters _trCharacters;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return base.name;
			}
		}

		public string DisplayName
		{
			[CompilerGenerated]
			get
			{
				return _displayName;
			}
		}

		public string SceneName
		{
			[CompilerGenerated]
			get
			{
				return _sceneName;
			}
		}

		public string[] GameModes
		{
			[CompilerGenerated]
			get
			{
				return _gameModes.value;
			}
		}

		public string CtArms
		{
			[CompilerGenerated]
			get
			{
				return _ctArms;
			}
		}

		public PlayerCharacters CtCharacters
		{
			[CompilerGenerated]
			get
			{
				return _ctCharacters;
			}
		}

		public string TrArms
		{
			[CompilerGenerated]
			get
			{
				return _trArms;
			}
		}

		public PlayerCharacters TrCharacters
		{
			[CompilerGenerated]
			get
			{
				return _trCharacters;
			}
		}

		public Sprite FullScreenImage
		{
			[CompilerGenerated]
			get
			{
				return _fullScreenImage;
			}
		}

		public Sprite PreviewImage
		{
			[CompilerGenerated]
			get
			{
				return _previewImage;
			}
		}

		public Sprite SchemaImage
		{
			[CompilerGenerated]
			get
			{
				return _schemaImage;
			}
		}
	}
}
