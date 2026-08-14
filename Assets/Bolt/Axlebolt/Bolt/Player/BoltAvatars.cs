using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;
using Google.Protobuf;

namespace Axlebolt.Bolt.Player
{
	public class BoltAvatars
	{
		private const string Folder = "bolt_avatars";

		private const string JpgExtension = "jpg";

		private readonly FriendsRemoteService _friendsRemoteService;

		private readonly string _path;

		public static BoltAvatars Instance { get; internal set; }

		internal BoltAvatars(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			_path = Path.Combine(path, "bolt_avatars");
			if (!Directory.Exists(_path))
			{
				Directory.CreateDirectory(_path);
			}
			_friendsRemoteService = new FriendsRemoteService(BoltApi.Instance.ClientService);
			Logger.Debug(string.Format("BoltAvatars path is {0}", _path));
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void FillCached(BoltPlayer player)
		{
			FillInternal(player, true);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void FillCached(IEnumerable<BoltPlayer> players)
		{
			FillInternal(players, true);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void FillCached(ICollection<BoltPlayer> players)
		{
			FillInternal(players, true);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Fill(BoltPlayer player)
		{
			FillInternal(player, false);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Fill(IEnumerable<BoltPlayer> players)
		{
			FillInternal(players, false);
		}

		private void FillInternal(BoltPlayer player, bool cached)
		{
			FillInternal(new BoltPlayer[1] { player }, cached);
		}

		private void FillInternal(IEnumerable<BoltPlayer> players, bool cached)
		{
			players = players.Where((BoltPlayer player) => !string.IsNullOrEmpty(player.AvatarId)).ToArray();
			FillFromCache(players);
			FillFromServer(players, cached);
		}

		private void FillFromServer(IEnumerable<BoltPlayer> players, bool cached)
		{
			players = players.Where((BoltPlayer player) => !IsCached(player.AvatarId)).ToArray();
			if (!players.Any())
			{
				return;
			}
			Dictionary<string, byte[]> fromServer = GetFromServer(players.Select((BoltPlayer player) => player.AvatarId).ToArray());
			foreach (BoltPlayer player in players)
			{
				player.Avatar = fromServer[player.AvatarId];
				if (player.Avatar.Length == 0)
				{
					player.Avatar = null;
				}
			}
			if (!cached)
			{
				return;
			}
			foreach (BoltPlayer player2 in players)
			{
				SaveToCache(player2.AvatarId, player2.Avatar);
			}
		}

		private void FillFromCache(IEnumerable<BoltPlayer> players)
		{
			foreach (BoltPlayer item in players.Where((BoltPlayer player) => IsCached(player.AvatarId)))
			{
				item.Avatar = GetFromCache(item.AvatarId);
			}
		}

		private Dictionary<string, byte[]> GetFromServer(string[] avatarIds)
		{
			AvatarBinary[] avatars = _friendsRemoteService.GetAvatars(avatarIds);
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			AvatarBinary[] array = avatars;
			foreach (AvatarBinary avatarBinary in array)
			{
				string id = avatarBinary.Id;
				ByteString avatar = avatarBinary.Avatar;
				dictionary[id] = (((object)avatar != null) ? avatar.ToByteArray() : null);
			}
			return dictionary;
		}

		public byte[] GetFromCache(string avatarId)
		{
			try
			{
				string avatarPath = GetAvatarPath(avatarId);
				return File.ReadAllBytes(avatarPath);
			}
			catch (Exception msg)
			{
				Logger.Error(msg);
				return new byte[0];
			}
		}

		public void SaveToCache(string avatarId, byte[] bytes)
		{
			try
			{
				if (bytes != null)
				{
					string avatarPath = GetAvatarPath(avatarId);
					File.WriteAllBytes(avatarPath, bytes);
				}
			}
			catch (Exception msg)
			{
				Logger.Error(msg);
			}
		}

		public bool IsCached(string avatarId)
		{
			return File.Exists(GetAvatarPath(avatarId));
		}

		private string GetAvatarPath(string avatarId)
		{
			return Path.Combine(_path, avatarId + ".jpg");
		}
	}
}
