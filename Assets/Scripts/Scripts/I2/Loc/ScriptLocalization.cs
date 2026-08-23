using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{
		public static class Alert
		{
			public static string BombArea => Get("Alert/BombArea");

			public static string BombHasBeenPlanted => Get("Alert/BombHasBeenPlanted");

			public static string YouDropped => Get("Alert/YouDropped");

			public static string YouTake => Get("Alert/YouTake");
		}

		public static class AudioSettings
		{
			public static string Title => Get("AudioSettings/Title");

			public static string Volume => Get("AudioSettings/Volume");
		}

		public static class Auth
		{
			public static string Connecting => Get("Auth/Connecting");

			public static string ConnectionError => Get("Auth/ConnectionError");

			public static string GooglePlayerServerError => Get("Auth/GooglePlayerServerError");
		}

		public static class AvatarSelectView
		{
			public static string ChangeAvatar => Get("AvatarSelectView/ChangeAvatar");

			public static string ChooseFromLibrary => Get("AvatarSelectView/ChooseFromLibrary");

			public static string LoadFromGallery => Get("AvatarSelectView/LoadFromGallery");

			public static string SelectAvatar => Get("AvatarSelectView/SelectAvatar");
		}

		public static class Clan
		{
			public static string UnderConstruction => Get("Clan/UnderConstruction");
		}

		public static class Common
		{
			public static string Accept => Get("Common/Accept");

			public static string Apply => Get("Common/Apply");

			public static string Attention => Get("Common/Attention");

			public static string Back => Get("Common/Back");

			public static string Button => Get("Common/Button");

			public static string Buy => Get("Common/Buy");

			public static string Cancel => Get("Common/Cancel");

			public static string Close => Get("Common/Close");

			public static string Confirmation => Get("Common/Confirmation");

			public static string Cost => Get("Common/Cost");

			public static string DoYouWantApplyChanges => Get("Common/DoYouWantApplyChanges");

			public static string DoubleTap => Get("Common/DoubleTap");

			public static string Error => Get("Common/Error");

			public static string Find => Get("Common/Find");

			public static string GetIt => Get("Common/GetIt");

			public static string IncompatibleVersion => Get("Common/IncompatibleVersion");

			public static string ListIsEmpty => Get("Common/ListIsEmpty");

			public static string Loading => Get("Common/Loading");

			public static string Name => Get("Common/Name");

			public static string No => Get("Common/No");

			public static string NotSelected => Get("Common/NotSelected");

			public static string Ok => Get("Common/Ok");

			public static string Play => Get("Common/Play");

			public static string PleaseWait => Get("Common/PleaseWait");

			public static string Reject => Get("Common/Reject");

			public static string RequestFailed => Get("Common/RequestFailed");

			public static string Retry => Get("Common/Retry");

			public static string SavingChanges => Get("Common/SavingChanges");

			public static string Select => Get("Common/Select");

			public static string ServerConnectionFailed => Get("Common/ServerConnectionFailed");

			public static string Success => Get("Common/Success");

			public static string WriteMessage => Get("Common/WriteMessage");

			public static string Yes => Get("Common/Yes");

			public static string You => Get("Common/You");
		}

		public static class ControlSettings
		{
			public static string Acceleration => Get("ControlSettings/Acceleration");

			public static string ButtonsPosition => Get("ControlSettings/ButtonsPosition");

			public static string Crosshair => Get("ControlSettings/Crosshair");

			public static string Customize => Get("ControlSettings/Customize");

			public static string FireButtonLabel => Get("ControlSettings/FireButtonLabel");

			public static string JoysticType => Get("ControlSettings/JoysticType");

			public static string JoystickTypeFixed => Get("ControlSettings/JoystickTypeFixed");

			public static string JoystickTypeFloating => Get("ControlSettings/JoystickTypeFloating");

			public static string JoystickTypeFreeTouch => Get("ControlSettings/JoystickTypeFreeTouch");

			public static string JumpTypeLable => Get("ControlSettings/JumpTypeLable");

			public static string ScopeSensitivity => Get("ControlSettings/ScopeSensitivity");

			public static string Sensitivity => Get("ControlSettings/Sensitivity");

			public static string Title => Get("ControlSettings/Title");
		}

		public static class DateTime
		{
			public static string April => Get("DateTime/April");

			public static string August => Get("DateTime/August");

			public static string December => Get("DateTime/December");

			public static string February => Get("DateTime/February");

			public static string January => Get("DateTime/January");

			public static string July => Get("DateTime/July");

			public static string June => Get("DateTime/June");

			public static string March => Get("DateTime/March");

			public static string May => Get("DateTime/May");

			public static string November => Get("DateTime/November");

			public static string October => Get("DateTime/October");

			public static string September => Get("DateTime/September");

			public static string Today => Get("DateTime/Today");

			public static string Yesterday => Get("DateTime/Yesterday");
		}

		public static class Defuse
		{
			public static string MvpDefusingBomb => Get("Defuse/MvpDefusingBomb");

			public static string MvpMostEliminations => Get("Defuse/MvpMostEliminations");

			public static string MvpPlantingBomb => Get("Defuse/MvpPlantingBomb");
		}

		public static class DefuseKit
		{
			public static string DefusingProgressName => Get("DefuseKit/DefusingProgressName");
		}

		public static class Dialogs
		{
			public static string CheckInternetConnection => Get("Dialogs/CheckInternetConnection");

			public static string ConnectionFailed => Get("Dialogs/ConnectionFailed");

			public static string ConnectionHasBeenLost => Get("Dialogs/ConnectionHasBeenLost");

			public static string ExitGameConfirmation => Get("Dialogs/ExitGameConfirmation");

			public static string FindingGame => Get("Dialogs/FindingGame");

			public static string LoadingPlayerProfile => Get("Dialogs/LoadingPlayerProfile");

			public static string Processing => Get("Dialogs/Processing");

			public static string RequestFailed => Get("Dialogs/RequestFailed");

			public static string SelectGameMode => Get("Dialogs/SelectGameMode");

			public static string TryingToReconnect => Get("Dialogs/TryingToReconnect");
		}

		public static class FriendActions
		{
			public static string AddFriend => Get("FriendActions/AddFriend");

			public static string Block => Get("FriendActions/Block");

			public static string Ignore => Get("FriendActions/Ignore");

			public static string InviteToLobby => Get("FriendActions/InviteToLobby");

			public static string JoinGameOrLobby => Get("FriendActions/JoinGameOrLobby");

			public static string KickLobbyMember => Get("FriendActions/KickLobbyMember");

			public static string OpenProfile => Get("FriendActions/OpenProfile");

			public static string RemoveFriend => Get("FriendActions/RemoveFriend");

			public static string RevokeInvite => Get("FriendActions/RevokeInvite");

			public static string RevokeLobbyInvite => Get("FriendActions/RevokeLobbyInvite");

			public static string SendMessage => Get("FriendActions/SendMessage");

			public static string Unblock => Get("FriendActions/Unblock");
		}

		public static class Friends
		{
			public static string BlockedException => Get("Friends/BlockedException");

			public static string BlockedTab => Get("Friends/BlockedTab");

			public static string DeleteConfirmDialog => Get("Friends/DeleteConfirmDialog");

			public static string EnterPlayerNameOrId => Get("Friends/EnterPlayerNameOrId");

			public static string FindFriendsTab => Get("Friends/FindFriendsTab");

			public static string FriendsTab => Get("Friends/FriendsTab");

			public static string IgnoredRequests => Get("Friends/IgnoredRequests");

			public static string NewRequests => Get("Friends/NewRequests");

			public static string OutgoingRequests => Get("Friends/OutgoingRequests");

			public static string RequestsTab => Get("Friends/RequestsTab");

			public static string Unblocked => Get("Friends/Unblocked");

			public static string YouAreFriend => Get("Friends/YouAreFriend");
		}

		public static class GameExitMessages
		{
			public static string Disconnected => Get("GameExitMessages/Disconnected");

			public static string ReconnectFailed => Get("GameExitMessages/ReconnectFailed");
		}

		public static class GameMode
		{
			public static string DeathMatch => Get("GameMode/DeathMatch");

			public static string DeathMatchDescription => Get("GameMode/DeathMatchDescription");

			public static string Defuse => Get("GameMode/Defuse");

			public static string DefuseDescription => Get("GameMode/DefuseDescription");
		}

		public static class GameSettings
		{
			public static string OurSocials => Get("GameSettings/OurSocials");

			public static string Title => Get("GameSettings/Title");
		}

		public static class GameState
		{
			public static string StartingGame => Get("GameState/StartingGame");

			public static string WaitingPlayers => Get("GameState/WaitingPlayers");

			public static string WarmUp => Get("GameState/WarmUp");
		}

		public static class Gun
		{
			public static string AccurateRange => Get("Gun/AccurateRange");

			public static string Ammunition => Get("Gun/Ammunition");

			public static string ArmorPenetration => Get("Gun/ArmorPenetration");

			public static string Damage => Get("Gun/Damage");

			public static string FireRate => Get("Gun/FireRate");

			public static string MovementRate => Get("Gun/MovementRate");

			public static string PenetrationPower => Get("Gun/PenetrationPower");

			public static string RecoilControl => Get("Gun/RecoilControl");
		}

		public static class Inventory
		{
			public static string UnderConstruction => Get("Inventory/UnderConstruction");
		}

		public static class LevelLoadingView
		{
			public static string InitWaiting => Get("LevelLoadingView/InitWaiting");

			public static string Loading => Get("LevelLoadingView/Loading");

			public static string Settings => Get("LevelLoadingView/Settings");
		}

		public static class LevelSelectDialog
		{
			public static string Title => Get("LevelSelectDialog/Title");
		}

		public static class Lobby
		{
			public static string AllMaps => Get("Lobby/AllMaps");

			public static string ConfirmJoinAnotherLobby => Get("Lobby/ConfirmJoinAnotherLobby");

			public static string ConfirmLeaveLobby => Get("Lobby/ConfirmLeaveLobby");

			public static string ConnectedToLobby => Get("Lobby/ConnectedToLobby");

			public static string CreateLobby => Get("Lobby/CreateLobby");

			public static string CreatingLobby => Get("Lobby/CreatingLobby");

			public static string FriendsOnlyLobbyType => Get("Lobby/FriendsOnlyLobbyType");

			public static string FriendsOnlyTypeDescription => Get("Lobby/FriendsOnlyTypeDescription");

			public static string GameIsClosed => Get("Lobby/GameIsClosed");

			public static string GameIsFull => Get("Lobby/GameIsFull");

			public static string IncompatibleVersion => Get("Lobby/IncompatibleVersion");

			public static string JoinAnotherLobby => Get("Lobby/JoinAnotherLobby");

			public static string JoinFailed => Get("Lobby/JoinFailed");

			public static string JoiningGame => Get("Lobby/JoiningGame");

			public static string JoiningLobby => Get("Lobby/JoiningLobby");

			public static string LeaveLobby => Get("Lobby/LeaveLobby");

			public static string LobbyInviteDialogTitle => Get("Lobby/LobbyInviteDialogTitle");

			public static string LobbyIsFull => Get("Lobby/LobbyIsFull");

			public static string LobbyJoinRestricted => Get("Lobby/LobbyJoinRestricted");

			public static string LobbyNotFound => Get("Lobby/LobbyNotFound");

			public static string MatchmakingCanceled => Get("Lobby/MatchmakingCanceled");

			public static string NewPlayerInvited => Get("Lobby/NewPlayerInvited");

			public static string NewPlayerJoined => Get("Lobby/NewPlayerJoined");

			public static string OwnerWasChanged => Get("Lobby/OwnerWasChanged");

			public static string PlayerKicked => Get("Lobby/PlayerKicked");

			public static string PlayerLeft => Get("Lobby/PlayerLeft");

			public static string PlayerRefused => Get("Lobby/PlayerRefused");

			public static string PlayerRevoked => Get("Lobby/PlayerRevoked");

			public static string PrivateLobbyDescription => Get("Lobby/PrivateLobbyDescription");

			public static string PrivateLobbyType => Get("Lobby/PrivateLobbyType");

			public static string PublicLobbyDescription => Get("Lobby/PublicLobbyDescription");

			public static string PublicLobbyType => Get("Lobby/PublicLobbyType");

			public static string SelectLobbyType => Get("Lobby/SelectLobbyType");

			public static string StartGame => Get("Lobby/StartGame");

			public static string YouHaveBeenKicked => Get("Lobby/YouHaveBeenKicked");
		}

		public static class Medals
		{
			public static string ExclusiveSkins => Get("Medals/ExclusiveSkins");

			public static string MedalsLabel => Get("Medals/MedalsLabel");

			public static string NoAdsLabel => Get("Medals/NoAdsLabel");

			public static string Purchased => Get("Medals/Purchased");

			public static string Purchasing => Get("Medals/Purchasing");

			public static string RemoveAdsLabel => Get("Medals/RemoveAdsLabel");

			public static string SpecialOfferDescription => Get("Medals/SpecialOfferDescription");

			public static string ThanksFor => Get("Medals/ThanksFor");

			public static string Unavailable => Get("Medals/Unavailable");
		}

		public static class Messages
		{
			public static string SelectDialogLabel => Get("Messages/SelectDialogLabel");

			public static string Title => Get("Messages/Title");
		}

		public static class NextLevelVote
		{
			public static string Title => Get("NextLevelVote/Title");

			public static string VoteEndsIn => Get("NextLevelVote/VoteEndsIn");
		}

		public static class Notifications
		{
			public static string FriendshipRequest => Get("Notifications/FriendshipRequest");

			public static string LobbyInvite => Get("Notifications/LobbyInvite");
		}

		public static class PauseMenu
		{
			public static string BackToGame => Get("PauseMenu/BackToGame");

			public static string ChangeTeam => Get("PauseMenu/ChangeTeam");

			public static string ExitGame => Get("PauseMenu/ExitGame");

			public static string Settings => Get("PauseMenu/Settings");

			public static string Title => Get("PauseMenu/Title");
		}

		public static class Play
		{
			public static string SpecialOffer => Get("Play/SpecialOffer");
		}

		public static class PlayerStats
		{
			public static string Accuracy => Get("PlayerStats/Accuracy");

			public static string Assists => Get("PlayerStats/Assists");

			public static string Deaths => Get("PlayerStats/Deaths");

			public static string GamesPlayed => Get("PlayerStats/GamesPlayed");

			public static string Headshots => Get("PlayerStats/Headshots");

			public static string HeadshotsPercentage => Get("PlayerStats/HeadshotsPercentage");

			public static string Hits => Get("PlayerStats/Hits");

			public static string Hours => Get("PlayerStats/Hours");

			public static string Kills => Get("PlayerStats/Kills");

			public static string PlayTime => Get("PlayerStats/PlayTime");

			public static string Shots => Get("PlayerStats/Shots");

			public static string Title => Get("PlayerStats/Title");
		}

		public static class PlayerStatus
		{
			public static string Away => Get("PlayerStatus/Away");

			public static string InGame => Get("PlayerStatus/InGame");

			public static string InLobby => Get("PlayerStatus/InLobby");

			public static string Offline => Get("PlayerStatus/Offline");

			public static string Online => Get("PlayerStatus/Online");

			public static string Playing => Get("PlayerStatus/Playing");
		}

		public static class Profile
		{
			public static string EnterYouName => Get("Profile/EnterYouName");
		}

		public static class Regions
		{
			public static string Asia => Get("Regions/Asia");

			public static string Europe => Get("Regions/Europe");

			public static string Usa => Get("Regions/Usa");
		}

		public static class SearchGame
		{
			public static string CurrentOnline => Get("SearchGame/CurrentOnline");

			public static string Ping => Get("SearchGame/Ping");

			public static string Region => Get("SearchGame/Region");
		}

		public static class SelectregionDialog
		{
			public static string Title => Get("SelectregionDialog/Title");
		}

		public static class SelectRegionDialog
		{
			public static string Description => Get("SelectRegionDialog/Description");
		}

		public static class Settings
		{
			public static string Disabled => Get("Settings/Disabled");

			public static string High => Get("Settings/High");

			public static string Low => Get("Settings/Low");

			public static string Medium => Get("Settings/Medium");

			public static string ResetToDefaults => Get("Settings/ResetToDefaults");

			public static string VeryHigh => Get("Settings/VeryHigh");

			public static string VeryLow => Get("Settings/VeryLow");
		}

		public static class Statistics
		{
			public static string Assist => Get("Statistics/Assist");

			public static string Death => Get("Statistics/Death");

			public static string Draw => Get("Statistics/Draw");

			public static string Kills => Get("Statistics/Kills");

			public static string Money => Get("Statistics/Money");

			public static string Name => Get("Statistics/Name");

			public static string Ping => Get("Statistics/Ping");

			public static string Score => Get("Statistics/Score");

			public static string Winner => Get("Statistics/Winner");
		}

		public static class Team
		{
			public static string CounterTerrorists => Get("Team/CounterTerrorists");

			public static string Terrorists => Get("Team/Terrorists");
		}

		public static class TeamDeathMatch
		{
			public static string WeaponBuyTitle => Get("TeamDeathMatch/WeaponBuyTitle");
		}

		public static class TeamSelect
		{
			public static string Select => Get("TeamSelect/Select");

			public static string Title => Get("TeamSelect/Title");
		}

		public static class Tutorial
		{
			public static string AimingZone => Get("Tutorial/AimingZone");

			public static string DoubleTapJump => Get("Tutorial/DoubleTapJump");

			public static string MovementZone => Get("Tutorial/MovementZone");

			public static string Recharge => Get("Tutorial/Recharge");

			public static string ShootingZone => Get("Tutorial/ShootingZone");
		}

		public static class VideoSettings
		{
			public static string AntiAliasingMode => Get("VideoSettings/AntiAliasingMode");

			public static string EffectDetail => Get("VideoSettings/EffectDetail");

			public static string ModelDetail => Get("VideoSettings/ModelDetail");

			public static string ShaderDetail => Get("VideoSettings/ShaderDetail");

			public static string ShadowDetail => Get("VideoSettings/ShadowDetail");

			public static string TextureDetail => Get("VideoSettings/TextureDetail");

			public static string Title => Get("VideoSettings/Title");
		}

		public static class WeaponCategories
		{
			public static string Grenade => Get("WeaponCategories/Grenade");

			public static string Heavy => Get("WeaponCategories/Heavy");

			public static string MachineGun => Get("WeaponCategories/MachineGun");

			public static string Pistols => Get("WeaponCategories/Pistols");

			public static string Rifles => Get("WeaponCategories/Rifles");
		}

		public static class WeaponSelect
		{
			public static string Ammo => Get("WeaponSelect/Ammo");

			public static string Cost => Get("WeaponSelect/Cost");

			public static string InventoryTitle => Get("WeaponSelect/InventoryTitle");

			public static string Weaponry => Get("WeaponSelect/Weaponry");
		}

		public static class WhoKill
		{
			public static string DamageGiven => Get("WhoKill/DamageGiven");

			public static string DamageTaken => Get("WhoKill/DamageTaken");

			public static string KilledYou => Get("WhoKill/KilledYou");
		}

		public static class Winners
		{
			public static string CounterTerroristsWin => Get("Winners/CounterTerroristsWin");

			public static string FirstPlace => Get("Winners/FirstPlace");

			public static string SecondPlace => Get("Winners/SecondPlace");

			public static string TerroristsWin => Get("Winners/TerroristsWin");

			public static string ThirdPlace => Get("Winners/ThirdPlace");
		}

		public static string Arial => Get("Arial");

		public static string BebasNeue_Regular => Get("BebasNeue Regular");

		public static string EurostileOT_Medium => Get("EurostileOT-Medium");

		public static string Headshots => Get("Headshots");

		public static string Get(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
		{
			return LocalizationManager.GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);
		}
	}
}
