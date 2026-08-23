using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine;

public class InRoomTime : MonoBehaviour
{
	private int roomStartTimestamp;

	private const string StartTimeKey = "#rt";

	public double RoomTime
	{
		get
		{
			uint roomTimestamp = (uint)RoomTimestamp;
			double num = roomTimestamp;
			return num / 1000.0;
		}
	}

	public int RoomTimestamp => PhotonNetwork.inRoom ? (PhotonNetwork.ServerTimestamp - roomStartTimestamp) : 0;

	public bool IsRoomTimeSet => PhotonNetwork.inRoom && PhotonNetwork.room.CustomProperties.ContainsKey("#rt");

	internal IEnumerator SetRoomStartTimestamp()
	{
		if (!IsRoomTimeSet && PhotonNetwork.isMasterClient)
		{
			if (PhotonNetwork.ServerTimestamp == 0)
			{
				yield return 0;
			}
			ExitGames.Client.Photon.Hashtable startTimeProp = new ExitGames.Client.Photon.Hashtable
			{
				["#rt"] = PhotonNetwork.ServerTimestamp
			};
			PhotonNetwork.room.SetCustomProperties(startTimeProp);
		}
	}

	public void OnJoinedRoom()
	{
		StartCoroutine("SetRoomStartTimestamp");
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		StartCoroutine("SetRoomStartTimestamp");
	}

	public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("#rt"))
		{
			roomStartTimestamp = (int)propertiesThatChanged["#rt"];
		}
	}
}
