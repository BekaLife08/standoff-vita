using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonNetworkExtension
{
	public static HashSet<IPunCallbacks> MessageListeners = new HashSet<IPunCallbacks>();

	public static void SendMonoMessageFast(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<IPunCallbacks> hashSet = new HashSet<IPunCallbacks>(MessageListeners);
		object callParameter = (parameters == null || parameters.Length != 1) ? parameters : parameters[0];
		switch (methodString)
		{
		case PhotonNetworkingMessage.OnConnectedToMaster:
			foreach (IPunCallbacks messageListener in hashSet)
			{
				Safe(delegate
				{
					messageListener.OnConnectedToMaster();
				});
			}
			break;
		case PhotonNetworkingMessage.OnConnectedToPhoton:
			foreach (IPunCallbacks messageListener2 in hashSet)
			{
				Safe(delegate
				{
					messageListener2.OnConnectedToPhoton();
				});
			}
			break;
		case PhotonNetworkingMessage.OnConnectionFail:
			foreach (IPunCallbacks messageListener3 in hashSet)
			{
				Safe(delegate
				{
					messageListener3.OnConnectionFail((DisconnectCause)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnCreatedRoom:
			foreach (IPunCallbacks messageListener5 in hashSet)
			{
				Safe(delegate
				{
					messageListener5.OnCreatedRoom();
				});
			}
			break;
		case PhotonNetworkingMessage.OnCustomAuthenticationFailed:
			foreach (IPunCallbacks messageListener7 in hashSet)
			{
				Safe(delegate
				{
					messageListener7.OnCustomAuthenticationFailed(callParameter as string);
				});
			}
			break;
		case PhotonNetworkingMessage.OnCustomAuthenticationResponse:
			foreach (IPunCallbacks messageListener11 in hashSet)
			{
				Safe(delegate
				{
					messageListener11.OnCustomAuthenticationResponse((Dictionary<string, object>)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnDisconnectedFromPhoton:
			foreach (IPunCallbacks messageListener13 in hashSet)
			{
				Safe(delegate
				{
					messageListener13.OnDisconnectedFromPhoton();
				});
			}
			break;
		case PhotonNetworkingMessage.OnFailedToConnectToPhoton:
			foreach (IPunCallbacks messageListener15 in hashSet)
			{
				Safe(delegate
				{
					messageListener15.OnFailedToConnectToPhoton((DisconnectCause)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnJoinedLobby:
			foreach (IPunCallbacks messageListener17 in hashSet)
			{
				Safe(delegate
				{
					messageListener17.OnJoinedLobby();
				});
			}
			break;
		case PhotonNetworkingMessage.OnJoinedRoom:
			foreach (IPunCallbacks messageListener20 in hashSet)
			{
				Safe(delegate
				{
					messageListener20.OnJoinedRoom();
				});
			}
			break;
		case PhotonNetworkingMessage.OnLeftLobby:
			foreach (IPunCallbacks messageListener22 in hashSet)
			{
				Safe(delegate
				{
					messageListener22.OnLeftLobby();
				});
			}
			break;
		case PhotonNetworkingMessage.OnLeftRoom:
			foreach (IPunCallbacks messageListener24 in hashSet)
			{
				Safe(delegate
				{
					messageListener24.OnLeftRoom();
				});
			}
			break;
		case PhotonNetworkingMessage.OnLobbyStatisticsUpdate:
			foreach (IPunCallbacks messageListener26 in hashSet)
			{
				Safe(delegate
				{
					messageListener26.OnLobbyStatisticsUpdate();
				});
			}
			break;
		case PhotonNetworkingMessage.OnMasterClientSwitched:
			foreach (IPunCallbacks messageListener28 in hashSet)
			{
				Safe(delegate
				{
					messageListener28.OnMasterClientSwitched((PhotonPlayer)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnOwnershipRequest:
			foreach (IPunCallbacks messageListener29 in hashSet)
			{
				Safe(delegate
				{
					messageListener29.OnOwnershipRequest((object[])callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonCreateRoomFailed:
			foreach (IPunCallbacks messageListener27 in hashSet)
			{
				Safe(delegate
				{
					messageListener27.OnPhotonCreateRoomFailed((object[])callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged:
			foreach (IPunCallbacks messageListener25 in hashSet)
			{
				Safe(delegate
				{
					messageListener25.OnPhotonCustomRoomPropertiesChanged((Hashtable)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonInstantiate:
			foreach (IPunCallbacks messageListener23 in hashSet)
			{
				Safe(delegate
				{
					messageListener23.OnPhotonInstantiate((PhotonMessageInfo)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonJoinRoomFailed:
			foreach (IPunCallbacks messageListener21 in hashSet)
			{
				Safe(delegate
				{
					messageListener21.OnPhotonJoinRoomFailed((object[])callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonMaxCccuReached:
			foreach (IPunCallbacks messageListener19 in hashSet)
			{
				Safe(delegate
				{
					messageListener19.OnPhotonMaxCccuReached();
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonPlayerConnected:
			foreach (IPunCallbacks messageListener18 in hashSet)
			{
				Safe(delegate
				{
					messageListener18.OnPhotonPlayerConnected((PhotonPlayer)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonPlayerDisconnected:
			foreach (IPunCallbacks messageListener16 in hashSet)
			{
				Safe(delegate
				{
					messageListener16.OnPhotonPlayerDisconnected((PhotonPlayer)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged:
			foreach (IPunCallbacks messageListener14 in hashSet)
			{
				Safe(delegate
				{
					messageListener14.OnPhotonPlayerPropertiesChanged((object[])callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonRandomJoinFailed:
			foreach (IPunCallbacks messageListener12 in hashSet)
			{
				Safe(delegate
				{
					messageListener12.OnPhotonRandomJoinFailed((object[])callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonSerializeView:
			UnityEngine.Debug.LogError("PhotonNetworkingMessage.OnPhotonSerializeView unsupported");
			break;
		case PhotonNetworkingMessage.OnReceivedRoomListUpdate:
			foreach (IPunCallbacks messageListener10 in hashSet)
			{
				Safe(delegate
				{
					messageListener10.OnReceivedRoomListUpdate();
				});
			}
			break;
		case PhotonNetworkingMessage.OnUpdatedFriendList:
			foreach (IPunCallbacks messageListener9 in hashSet)
			{
				Safe(delegate
				{
					messageListener9.OnUpdatedFriendList();
				});
			}
			break;
		case PhotonNetworkingMessage.OnWebRpcResponse:
			foreach (IPunCallbacks messageListener8 in hashSet)
			{
				Safe(delegate
				{
					messageListener8.OnWebRpcResponse((OperationResponse)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnPhotonPlayerActivityChanged:
			foreach (IPunCallbacks messageListener6 in hashSet)
			{
				Safe(delegate
				{
					messageListener6.OnPhotonPlayerActivityChanged((PhotonPlayer)callParameter);
				});
			}
			break;
		case PhotonNetworkingMessage.OnOwnershipTransfered:
			foreach (IPunCallbacks messageListener4 in hashSet)
			{
				Safe(delegate
				{
					messageListener4.OnOwnershipTransfered((object[])callParameter);
				});
			}
			break;
		default:
			UnityEngine.Debug.LogError($"Unsupported {methodString}");
			break;
		}
	}

	private static void Safe(Action action)
	{
		try
		{
			action();
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
	}
}
