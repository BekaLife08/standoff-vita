using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axlebolt.Standoff.Main.Inventory
{
	public class InventoryManager : Singleton<InventoryManager>
	{
		private Dictionary<InventoryItemId, BadgeDefintion> _badgeDefinitions;

		private Dictionary<InventoryItemId, SkinDefinition> _skinDefinitions;

		public void Init()
		{
			if (_badgeDefinitions != null)
			{
				throw new Exception(string.Format("{0} already initialized", "InventoryManager"));
			}
			Singleton<InAppManager>.Instance.Init();
			InventoryItemDefintion[] source = ResourcesUtility.LoadAll<InventoryItemDefintion>("Inventory");
			_badgeDefinitions = (from item in source
				where item is BadgeDefintion
				select item).Cast<BadgeDefintion>().ToDictionary((BadgeDefintion def) => def.Id, (BadgeDefintion def) => def);
			_skinDefinitions = (from item in source
				where item is SkinDefinition
				select item).Cast<SkinDefinition>().ToDictionary((SkinDefinition def) => def.Id, (SkinDefinition def) => def);
		}

		private void FailIfNotInitialized()
		{
			if (_badgeDefinitions == null)
			{
				throw new Exception(string.Format("{0} is not initialized", "InventoryManager"));
			}
		}

		public BadgeDefintion GetBadgeItemDefinition(InventoryItemId id)
		{
			FailIfNotInitialized();
			return _badgeDefinitions[id];
		}

		public bool HasInventoryItemById(InventoryItemId id)
		{
			FailIfNotInitialized();
			return Singleton<InAppManager>.Instance.IsInventoryItemBought(id);
		}

		public InventoryItemId GetMainBadgeId()
		{
			FailIfNotInitialized();
			InventoryItemId result = InventoryItemId.None;
			InventoryItemId[] array = new InventoryItemId[5]
			{
				InventoryItemId.MedalAssistanceBronze,
				InventoryItemId.MedalAssistanceSilver,
				InventoryItemId.MedalAssistanceGold,
				InventoryItemId.MedalAssistancePlatinum,
				InventoryItemId.MedalAssistanceBrilliant
			};
			InventoryItemId[] array2 = array;
			foreach (InventoryItemId inventoryItemId in array2)
			{
				if (!HasInventoryItemById(inventoryItemId))
				{
					break;
				}
				result = inventoryItemId;
			}
			return result;
		}

		public InventoryItemId GetSelectedSkinId(WeaponId weaponId)
		{
			switch (weaponId)
			{
			case WeaponId.Deagle:
				return HasInventoryItemById(InventoryItemId.MedalAssistanceSilver) ? InventoryItemId.DeagleCaptainMorgan : InventoryItemId.None;
			case WeaponId.M4:
				return HasInventoryItemById(InventoryItemId.MedalAssistanceGold) ? InventoryItemId.M4Predator : InventoryItemId.None;
			case WeaponId.AWM:
				return HasInventoryItemById(InventoryItemId.MedalAssistancePlatinum) ? InventoryItemId.AwmSport : InventoryItemId.None;
			case WeaponId.Knife:
				return HasInventoryItemById(InventoryItemId.MedalAssistanceBrilliant) ? InventoryItemId.KnifeBlueBlood : InventoryItemId.None;
			default:
				return InventoryItemId.None;
			}
		}

		public BadgeDefintion GetMainBadgeDefinition()
		{
			FailIfNotInitialized();
			return GetBadgeItemDefinition(GetMainBadgeId());
		}

		public SkinDefinition GetSkinDefinition(InventoryItemId id)
		{
			FailIfNotInitialized();
			if (!_skinDefinitions.ContainsKey(id))
			{
				throw new Exception($"SkinDefinition with id {id} not found");
			}
			return _skinDefinitions[id];
		}

		public SkinDefinition[] GetSkinDefinitions()
		{
			FailIfNotInitialized();
			return _skinDefinitions.Values.ToArray();
		}
	}
}
