using System.Text;
using UnityEngine;

public class SupportLogging : MonoBehaviour
{
	public bool LogTrafficStats;

	public void Start()
	{
		if (LogTrafficStats)
		{
			InvokeRepeating("LogStats", 10f, 10f);
		}
	}

	protected void OnApplicationPause(bool pause)
	{
		UnityEngine.Debug.Log("SupportLogger OnApplicationPause: " + pause + " connected: " + PhotonNetwork.connected);
	}

	public void OnApplicationQuit()
	{
		CancelInvoke();
	}

	public void LogStats()
	{
		if (LogTrafficStats)
		{
			UnityEngine.Debug.Log("SupportLogger " + PhotonNetwork.NetworkStatisticsToString());
		}
	}

	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.86.1");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} PeerId: {2} ", PhotonNetwork.networkingPeer.AppId.Substring(0, 8), PhotonNetwork.networkingPeer.AppVersion, PhotonNetwork.networkingPeer.PeerID);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);
		UnityEngine.Debug.Log(stringBuilder.ToString());
	}

	public void OnConnectedToPhoton()
	{
		UnityEngine.Debug.Log("SupportLogger OnConnectedToPhoton().");
		LogBasics();
		if (LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		UnityEngine.Debug.Log("SupportLogger OnFailedToConnectToPhoton(" + cause + ").");
		LogBasics();
	}

	public void OnJoinedLobby()
	{
		UnityEngine.Debug.Log("SupportLogger OnJoinedLobby(" + PhotonNetwork.lobby + ").");
	}

	public void OnJoinedRoom()
	{
		UnityEngine.Debug.Log("SupportLogger OnJoinedRoom(" + PhotonNetwork.room + "). " + PhotonNetwork.lobby + " GameServer:" + PhotonNetwork.ServerAddress);
	}

	public void OnCreatedRoom()
	{
		UnityEngine.Debug.Log("SupportLogger OnCreatedRoom(" + PhotonNetwork.room + "). " + PhotonNetwork.lobby + " GameServer:" + PhotonNetwork.ServerAddress);
	}

	public void OnLeftRoom()
	{
		UnityEngine.Debug.Log("SupportLogger OnLeftRoom().");
	}

	public void OnDisconnectedFromPhoton()
	{
		UnityEngine.Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
	}
}
