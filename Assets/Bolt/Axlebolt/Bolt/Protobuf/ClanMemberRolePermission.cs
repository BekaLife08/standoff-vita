using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum ClanMemberRolePermission
	{
		[OriginalName("CHANGE_CLAN_SETTINGS")]
		ChangeClanSettings = 0,
		[OriginalName("ACCEPT_MEMBER")]
		AcceptMember = 1,
		[OriginalName("INVITE_MEMBER")]
		InviteMember = 2,
		[OriginalName("KICK_MEMBER_LESS")]
		KickMemberLess = 3,
		[OriginalName("KICK_MEMBER_EQUAL")]
		KickMemberEqual = 4,
		[OriginalName("ASSIGN_ROLE_LESS")]
		AssignRoleLess = 5,
		[OriginalName("ASSIGN_ROLE_EQUAL")]
		AssignRoleEqual = 6,
		[OriginalName("CREATE_CLAN_BATTLE")]
		CreateClanBattle = 7,
		[OriginalName("JOIN_CLAN_BATTLE")]
		JoinClanBattle = 8
	}
}
