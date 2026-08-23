using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Networking;
using UnityEngine;
using UnityEngine.Networking;

public class TestPlayerSync : MonoBehaviour
{
	public Axlebolt.Standoff.Player.PlayerController sourcePlayer;

	public CharacterPlayer targetPlayer1;

	public CharacterPlayer targetPlayer2;

	public CharacterPlayer targetPlayer3;

	public CharacterPlayer targetPlayer4;

	public CharacterPlayer targetPlayer5;

	public int syncRate = 10;

	private float syncTime;

	public float syncInterval
	{
		get
		{
			return 1f / (float)syncRate;
		}
		set
		{
		}
	}

	private PlayerSnapshot GetSnapshot()
	{
		NetworkWriter networkWriter = new NetworkWriter();
		sourcePlayer.GetSnapshot().Serialize(networkWriter);
		byte[] buffer = networkWriter.ToArray();
		NetworkReader reader = new NetworkReader(buffer);
		PlayerSnapshot playerSnapshot = new PlayerSnapshot();
		playerSnapshot.Deserialize(reader);
		return playerSnapshot;
	}

	private void Update()
	{
		if (Time.time - syncTime > syncInterval)
		{
			syncTime = Time.time;
			PlayerSnapshot snapshot = GetSnapshot();
			targetPlayer1.AddSnapshot(snapshot);
			targetPlayer2.AddSnapshot(snapshot);
			targetPlayer3.AddSnapshot(snapshot);
			targetPlayer4.AddSnapshot(snapshot);
			targetPlayer5.AddSnapshot(snapshot);
		}
	}
}
