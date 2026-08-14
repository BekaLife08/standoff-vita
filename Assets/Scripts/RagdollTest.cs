using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using UnityEngine;
using UnityEngine.UI;

public class RagdollTest : MonoBehaviour
{
	public InputField inputBoneID;

	public InputField inputForce;

	public Transform director;

	private GameObject characterObject;

	private bool isInitialized;

	public RagdollParameters ragdollParameters;

	public BipedMap sourceCharacter;

	public GameObject fallingCharacterPrefab;

	public BipedMap targetCharacter;

	private GameObject fallingCharacter;

	private void Awake()
	{
		RagdollController ragdollController = targetCharacter.gameObject.AddComponent<RagdollController>();
		ragdollController.RagdollParameters = ragdollParameters;
		ragdollController.PreInitialize(sourceCharacter);
	}

	public void Init()
	{
		if (fallingCharacter != null)
		{
			UnityEngine.Object.Destroy(fallingCharacter);
		}
		fallingCharacter = UnityEngine.Object.Instantiate(fallingCharacterPrefab);
		fallingCharacter.SetActive(value: true);
	}

	public void Hit()
	{
		if (!isInitialized)
		{
			isInitialized = true;
		}
		int result = -1;
		int.TryParse(inputBoneID.text, out result);
		float result2 = 0f;
		float.TryParse(inputForce.text, out result2);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
