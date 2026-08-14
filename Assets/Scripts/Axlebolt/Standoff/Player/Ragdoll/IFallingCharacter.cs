using UnityEngine;

namespace Axlebolt.Standoff.Player.Ragdoll
{
	public interface IFallingCharacter
	{
		string GetName();

		BipedMap GetBipedMap();

		Vector3 GetCharacterVelocity();

		FallingCharacterConfig GetFallingCharacterConfig();

		void OnSimulateFalling();

		int GetPlayerId();
	}
}
