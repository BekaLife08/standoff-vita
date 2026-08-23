using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Axlebolt.Standoff.Game.UI
{
	public class KillLogView : View
	{
		private const float HideAfterSec = 3f;

		private const float MaxCount = 9f;

		[NotNull]
		[SerializeField]
		[FormerlySerializedAs("logItemPrefab")]
		private KillLogItemView _logItemViewPrefab;

		private readonly List<KillLogItemView> _pool = new List<KillLogItemView>();

		private void Awake()
		{
			for (int i = 0; (float)i < 9f; i++)
			{
				KillLogItemView killLogItemView = Object.Instantiate(_logItemViewPrefab, _logItemViewPrefab.transform.parent, worldPositionStays: false);
				killLogItemView.Hide();
				_pool.Add(killLogItemView);
			}
			UnityEngine.Object.Destroy(_logItemViewPrefab.gameObject);
		}

		public void LogKill(PhotonPlayer killer, PhotonPlayer assist, PhotonPlayer dead, WeaponParameters weapon, bool headShot, bool penetrated)
		{
			KillLogItemView freeItem = GetFreeItem();
			freeItem.Set(killer, assist, dead, weapon, headShot, penetrated);
			freeItem.Show();
			if (!base.IsVisible)
			{
				Show();
				StartCoroutine(HideItems());
			}
		}

		private IEnumerator HideItems()
		{
			while (true)
			{
				if (_pool[0].IsVisible && Time.time - _pool[0].Time > 3f)
				{
					HideFirstItem();
					continue;
				}
				if (!_pool[0].IsVisible)
				{
					break;
				}
				yield return new WaitForSeconds(Time.time - _pool[0].Time);
			}
			Hide();
		}

		private KillLogItemView GetFreeItem()
		{
			foreach (KillLogItemView item in _pool)
			{
				if (!item.IsVisible)
				{
					return item;
				}
			}
			return HideFirstItem();
		}

		private KillLogItemView HideFirstItem()
		{
			KillLogItemView killLogItemView = _pool[0];
			_pool.Remove(killLogItemView);
			_pool.Add(killLogItemView);
			killLogItemView.Hide();
			killLogItemView.transform.SetSiblingIndex(_pool.Count - 1);
			return killLogItemView;
		}
	}
}
