using Axlebolt.Standoff.Player;
using UnityEngine;

public class CharacterSkinSetterTest : MonoBehaviour
{
	public BipedMap sourceSkin;

	public PlayerController playerControl;

	public void SetSkin()
	{
		playerControl.SetCharacterSkin(sourceSkin);
	}
}
