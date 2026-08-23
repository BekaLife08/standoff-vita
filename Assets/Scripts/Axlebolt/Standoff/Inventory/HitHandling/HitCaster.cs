using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.HitHandling
{
	public class HitCaster
	{
		public enum CasterState
		{
			InitialCast,
			PostCast,
			ReverseCast
		}

		private const float CastDistance = 1000f;

		private const float MinImpulse = 50f;

		public static Vector3 CastHit(Vector3 startPosition, Vector3 direction, WeaponHitParameters parameters, bool isLocal, List<HitCasterResult> hitCasterResultList)
		{
			return CastHit(startPosition, direction, 1000f, parameters, isLocal, hitCasterResultList);
		}

		public static Vector3 CastHit(Vector3 startPosition, Vector3 direction, float castDistance, WeaponHitParameters parameters, bool isLocal, List<HitCasterResult> hitCasterResultList)
		{
			float num = parameters.PenetrationPower;
			int mask = LayerMask.GetMask("PlayerHitbox", "BulletLayer");
			direction.Normalize();
			List<PlayerHitController> list = new List<PlayerHitController>();
			CasterState casterState = CasterState.InitialCast;
			RaycastHit hit = default(RaycastHit);
			Ray ray = default(Ray);
			ray.direction = direction;
			ray.origin = startPosition;
			Ray ray2 = ray;
			while (true)
			{
				RaycastHit hit2;
				bool flag = Cast(ray2, out hit2, castDistance, mask, list, casterState);
				switch (casterState)
				{
				case CasterState.InitialCast:
					if (flag)
					{
						SurfaceType surfaceType2 = SurfaceTypeUtility.FromTag(hit2.collider.tag);
						ray2.origin = hit2.point + direction * 0.0001f;
						ray2.direction = direction;
						if (surfaceType2 == SurfaceType.Character)
						{
							PlayerHitbox component = hit2.collider.gameObject.GetComponent<PlayerHitbox>();
							component.PlayerHitController.DisableBoneHit();
							list.Remove(component.PlayerHitController);
							if (isLocal)
							{
								float lossCoeff = num / parameters.PenetrationPower;
								HitCasterResult hitCasterResult = new HitCasterResult();
								hitCasterResult.BulletHitData = CreateHitData(component, hit2, direction, parameters, lossCoeff);
								hitCasterResult.PlayerHitController = component.PlayerHitController;
								hitCasterResultList.Add(hitCasterResult);
							}
						}
						castDistance = 1000f - Vector3.Distance(startPosition, hit2.point);
						if (HitSurface.IsThinMaterial(surfaceType2))
						{
							casterState = CasterState.InitialCast;
							num -= HitSurface.GetPenetrationLoss(surfaceType2, 0f);
						}
						else
						{
							casterState = CasterState.PostCast;
						}
						hit = hit2;
						DrawHit(parameters.WeaponId, hit, surfaceType2, isLocal);
						if (num <= 0f)
						{
							DisableTriggeredPlayers(list);
							return hit2.point;
						}
						break;
					}
					DisableTriggeredPlayers(list);
					return startPosition + direction.normalized * 1000f;
				case CasterState.PostCast:
				{
					Vector3 vector = (!flag) ? (startPosition + direction.normalized * 1000f) : hit2.point;
					casterState = CasterState.ReverseCast;
					ray2.direction = -direction;
					ray2.origin = vector - direction * 0.0001f;
					castDistance = Vector3.Distance(hit.point, vector);
					break;
				}
				case CasterState.ReverseCast:
				{
					SurfaceType surfaceType = SurfaceTypeUtility.FromTag(hit.collider.tag);
					float thickness = (!flag) ? 0.01f : ((!(hit.collider.gameObject == hit2.collider.gameObject)) ? 0.01f : Vector3.Distance(hit.point, hit2.point));
					float penetrationLoss = HitSurface.GetPenetrationLoss(surfaceType, thickness);
					num -= penetrationLoss;
					if (num > 0f)
					{
						if (flag && hit.collider.gameObject == hit2.collider.gameObject)
						{
							DrawHit(parameters.WeaponId, hit2, surfaceType, isLocal);
						}
						casterState = CasterState.InitialCast;
						ray2.direction = direction;
						ray2.origin = hit.point + direction * 0.0001f;
						castDistance = 1000f - Vector3.Distance(startPosition, hit.point);
						break;
					}
					DisableTriggeredPlayers(list);
					return hit.point;
				}
				}
			}
		}

		private static BulletHitData CreateHitData(PlayerHitbox hitBox, RaycastHit hit, Vector3 direction, WeaponHitParameters parameters, float lossCoeff)
		{
			float num = (float)parameters.GetDamage(hitBox, direction) * lossCoeff;
			float impulse = parameters.Impulse * lossCoeff;
			bool penetrated = lossCoeff < 0.99999f;
			BulletHitData bulletHitData = new BulletHitData();
			bulletHitData.Point = hit.point;
			bulletHitData.Impulse = impulse;
			bulletHitData.Damage = (int)num;
			bulletHitData.ArmorPenetration = parameters.ArmorPenetration;
			bulletHitData.Bone = hitBox.Bone;
			bulletHitData.Penetrated = penetrated;
			return bulletHitData;
		}

		private static void DisableTriggeredPlayers(List<PlayerHitController> triggeredPlayers)
		{
			foreach (PlayerHitController triggeredPlayer in triggeredPlayers)
			{
				triggeredPlayer.DisableBoneHit();
			}
			triggeredPlayers.Clear();
		}

		private static bool Cast(Ray ray, out RaycastHit hit, float castDistance, int layerMask, List<PlayerHitController> triggeredPlayers, CasterState casterState)
		{
			bool flag = Physics.Raycast(ray, out hit, castDistance, layerMask);
			while (flag && casterState == CasterState.InitialCast && hit.collider.CompareTag(Tags.PlayerHitTrigger))
			{
				PlayerHitController component = hit.collider.gameObject.transform.parent.GetComponent<PlayerHitController>();
				component.EnableBoneHit();
				flag = Physics.Raycast(ray, out hit, castDistance, layerMask);
				triggeredPlayers.Add(component);
			}
			return flag;
		}

		private static void DrawHit(WeaponId weaponId, RaycastHit hit, SurfaceType surfaceType, bool isLocal)
		{
			Singleton<SurfaceImpactsEmitter>.Instance.Emit(weaponId, hit, surfaceType, isLocal);
		}
	}
}
