using System.Reflection;
using System.Threading;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("SettingsRemoteService")]
	public class SettingsRemoteService : RpcService
	{
		public SettingsRemoteService(ClientService client)
			: base(client)
		{
		}

		[Rpc("updateSettingString")]
		public void UpdateSetting(string key, string value, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { key, value }, ct);
		}

		[Rpc("updateSettingBoolean")]
		public void UpdateSetting(string key, bool? value, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { key, value }, ct);
		}

		[Rpc("updateSettingFloat")]
		public void UpdateSetting(string key, float? value, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { key, value }, ct);
		}

		[Rpc("updateSettingInt")]
		public void UpdateSetting(string key, int? value, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[2] { key, value }, ct);
		}

		[Rpc("getSettings")]
		public PlayerSetting[] GetSettings(CancellationToken ct = default(CancellationToken))
		{
			return (PlayerSetting[])Invoke(MethodBase.GetCurrentMethod(), new object[0], ct);
		}

		[Rpc("getSetting")]
		public PlayerSetting GetSetting(string key, CancellationToken ct = default(CancellationToken))
		{
			return (PlayerSetting)Invoke(MethodBase.GetCurrentMethod(), new object[1] { key }, ct);
		}

		[Rpc("updateSettings")]
		public void UpdateSettings(PlayerSetting[] settings, CancellationToken ct = default(CancellationToken))
		{
			Invoke(MethodBase.GetCurrentMethod(), new object[1] { settings }, ct);
		}
	}
}
