using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using UnityEngine;

public class FallingCharacterController : MonoBehaviour, IFallingCharacter
{
	private Vector3 prevPosition;

	private Vector3 velocity;

	public BipedMap GetBipedMap()
	{
		return GetComponent<BipedMap>();
	}

	public Vector3 GetCharacterVelocity()
	{
		return velocity;
	}

	public FallingCharacterConfig GetFallingCharacterConfig()
	{
		return new FallingCharacterConfig();
	}

	public void OnSimulateFalling()
	{
		base.gameObject.SetActive(value: false);
	}

	public int GetPlayerId()
	{
		return 0;
	}

	public string GetName()
	{
		return base.gameObject.name;
	}

	private void Start()
	{
		prevPosition = base.transform.position;
	}

	private void Update()
	{
		velocity = base.transform.position - prevPosition;
		velocity /= Time.deltaTime;
		prevPosition = base.transform.position;
	}
}
