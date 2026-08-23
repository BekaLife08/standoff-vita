namespace Axlebolt.Standoff.Inventory.Bomb
{
	public interface IBombListener
	{
		void OnBombPlantError();

		void OnBombPlanting();

		void OnBombPlanted(PhotonPlayer bomber, float detonationTime);

		void OnBombBeepSignal();

		void OnDetonated(PhotonPlayer bomber);

		void OnDefusingBomb();

		void OnDefused(PhotonPlayer sapper, float time);
	}
}
