using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Settings.Video;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class WeaponDropManager : ScenePhotonBehavior<WeaponDropManager>
	{
		private static readonly Log Log = Log.Create(typeof(WeaponDropManager));

		public const int MaxDropId = 10000;

		public const int PoolSize = 1;

		private static WeaponDropManager _instance;

		private int _dropId;

		private readonly Dictionary<int, DroppedWeaponController> _droppedWeapons = new Dictionary<int, DroppedWeaponController>();

		private DroppedWeaponPool _pool;

		private IWeaponryController _weaponry;

		private bool _inProgress;

		public Func<WeaponId, bool> TakeRule = (WeaponId id) => true;

		public IWeaponDropListener DropListener { get; set; }

		public bool InProgress { get; private set; }

		public event Action<DroppedWeaponController, WeaponDropData> WeaponDropEvent;

		public event Action<WeaponTakeData, WeaponDropData> WeaponTakeEvent;

		public static IEnumerator Init([NotNull] WeaponParameters[] weapons)
		{
			object[] data = ((IEnumerable<WeaponParameters>)weapons).Select((Func<WeaponParameters, object>)((WeaponParameters weapon) => weapon.Id)).ToArray();
			yield return ScenePhotonBehavior<WeaponDropManager>.Init(data);
		}

		public override void OnInstantiate(object[] data)
		{
			if (_pool == null)
			{
				WeaponParameters[] weapons = data.Select((object weaponId) => WeaponUtility.LoadWeapon((WeaponId)weaponId)).ToArray();
				InitInternal(weapons);
			}
		}

		private void InitInternal([NotNull] WeaponParameters[] weapons)
		{
			WeaponDropParameters dropParameters = ResourcesUtility.Load<WeaponDropParameters>("Parameters/WeaponDropParameters");
			_pool = new DroppedWeaponPool(weapons, 1, dropParameters, base.transform);
			VideoSettingsManager.Instance.ModelDetailChanged += OnModelDetailChanged;
			VideoSettingsManager.Instance.ShaderDetailChanged += OnShaderDetailChanged;
		}

		public void SetPlayer(GameObject playerObject)
		{
			_weaponry = ((!(playerObject != null)) ? null : playerObject.GetRequireComponent<IWeaponryController>());
		}

		public override void OnReturnToPool()
		{
			ClearInternal();
		}

		public void Clear()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("Clear");
			}
			if (!PhotonNetwork.isMasterClient)
			{
				throw new Exception("Clear can call only master client");
			}
			base.PhotonView.RPC("ClearViaServer", PhotonTargets.AllGlobalBuffered);
			PhotonNetwork.RemoveRPCs(base.PhotonView);
		}

		[PunRPC]
		private void ClearViaServer()
		{
			ClearInternal();
		}

		private void ClearInternal()
		{
			_dropId = 0;
			foreach (DroppedWeaponController value in _droppedWeapons.Values)
			{
				_pool.ReturnToPool(value);
			}
			_droppedWeapons.Clear();
			InProgress = false;
		}

		private void CheckIsInitialized()
		{
			if (_pool == null)
			{
				throw new Exception(string.Format("{0} not initialized", "WeaponDropManager"));
			}
		}

		private void CheckWeaponry()
		{
			if (_weaponry == null)
			{
				throw new Exception("Player is not set");
			}
			if (_weaponry.PhotonView.viewID <= 0)
			{
				throw new Exception("ViewId is invalid");
			}
		}

		public IEnumerable<DroppedWeaponController> GetDroppedWeapons()
		{
			return _droppedWeapons.Values;
		}

		public DroppedWeaponController GetDroppedWeapon(int dropId)
		{
			return _droppedWeapons[dropId];
		}

		public void Drop([NotNull] WeaponController controller, Vector3 direction)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			CheckIsInitialized();
			CheckWeaponry();
			if (InProgress)
			{
				Log.Warning("Drop/Take already in progress!");
				return;
			}
			WeaponDropData dropData = CreateDropData(controller, direction);
			byte[] array = SerializeDropData(dropData);
			InProgress = true;
			base.PhotonView.RPC("DropViaServer", PhotonTargets.AllGlobalBuffered, array);
		}

		[PunRPC]
		private void DropViaServer(byte[] bytes)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("DropViaServer");
			}
			WeaponDropData weaponDropData = DeserializeDropData(bytes);
			DropInternal(weaponDropData);
			if (!IsMineMessage(weaponDropData.ViewId))
			{
				return;
			}
			if (_weaponry != null && _weaponry.PhotonView.viewID == weaponDropData.ViewId)
			{
				IWeaponDropListener dropListener = DropListener;
				if (dropListener != null)
				{
					dropListener.OnWeaponDrop(_pool.GetWeaponParameters(weaponDropData.WeaponId));
				}
			}
			InProgress = false;
		}

		public void DropImmediately([NotNull] WeaponController controller, Vector3 direction)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}
			CheckIsInitialized();
			CheckWeaponry();
			WeaponDropData dropData = CreateDropData(controller, direction);
			byte[] array = SerializeDropData(dropData);
			base.PhotonView.RPC("DropImmediatelyViaServer", PhotonTargets.AllGlobalBuffered, array);
		}

		[PunRPC]
		private void DropImmediatelyViaServer(byte[] bytes)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("DropImmediatelyViaServer");
			}
			WeaponDropData dropData = DeserializeDropData(bytes);
			DropInternal(dropData);
		}

		private WeaponDropData CreateDropData(WeaponController controller, Vector3 direction)
		{
			WeaponDropData weaponDropData = new WeaponDropData();
			weaponDropData.DropId = _weaponry.PhotonView.ownerId * 10000 + _dropId++;
			weaponDropData.WeaponId = controller.WeaponId;
			weaponDropData.SkinId = controller.SkinId;
			weaponDropData.Direction = direction;
			weaponDropData.Transform = TransformTRS.FromTransform(controller.transform);
			weaponDropData.ViewId = _weaponry.PhotonView.viewID;
			WeaponDropData weaponDropData2 = weaponDropData;
			if (controller is GunController)
			{
				GunController gunController = (GunController)controller;
				weaponDropData2.Capacity = gunController.Capacity;
				weaponDropData2.MagazineCapacity = gunController.MagazineCapacity;
			}
			return weaponDropData2;
		}

		public void DropInternal(WeaponDropData dropData)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Executed Drop {0} ({1}, owner {2}", dropData.DropId, dropData.WeaponId, PhotonBehavior.ToOwnerId(dropData.ViewId)));
			}
			DroppedWeaponController fromPool = _pool.GetFromPool((byte)dropData.WeaponId);
			fromPool.Initialize(dropData.DropId, dropData.SkinId);
			fromPool.gameObject.SetActive(true);
			UpdateMaterial(fromPool);
			fromPool.Drop(dropData);
			_droppedWeapons[fromPool.DropId] = fromPool;
			if (this.WeaponDropEvent != null)
			{
				this.WeaponDropEvent(fromPool, dropData);
			}
		}

		public void Take(int dropId, [NotNull] IWeaponryController weaponry)
		{
			if (weaponry == null)
			{
				throw new ArgumentNullException("weaponry");
			}
			CheckIsInitialized();
			CheckWeaponry();
			if (InProgress)
			{
				Log.Warning("Drop/Take already in progress!");
				return;
			}
			DroppedWeaponController droppedWeapon = GetDroppedWeapon(dropId);
			if (droppedWeapon == null)
			{
				Log.Warning(string.Format("Dropped weapon {0} not found", dropId));
			}
			else if (TakeRule(droppedWeapon.WeaponId))
			{
				_weaponry = weaponry;
				WeaponTakeData weaponTakeData = new WeaponTakeData();
				weaponTakeData.DropId = dropId;
				weaponTakeData.ViewId = weaponry.PhotonView.viewID;
				WeaponTakeData takeData = weaponTakeData;
				InProgress = true;
				base.PhotonView.RPC("TakeViaServer", PhotonTargets.AllGlobalBuffered, SerializeTakeData(takeData));
			}
		}

		[PunRPC]
		private void TakeViaServer(byte[] bytes)
		{
			WeaponTakeData weaponTakeData = DeserializeTakeData(bytes);
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Execute Take RPC {0}, owner {1}", weaponTakeData.DropId, PhotonBehavior.ToOwnerId(weaponTakeData.ViewId)));
			}
			if (!_droppedWeapons.ContainsKey(weaponTakeData.DropId))
			{
				Log.Warning(string.Format("Dropped weapon not found {0}", weaponTakeData.DropId));
				return;
			}
			DroppedWeaponController droppedWeaponController = _droppedWeapons[weaponTakeData.DropId];
			WeaponDropData dropData = droppedWeaponController.DropData;
			_droppedWeapons.Remove(weaponTakeData.DropId);
			_pool.ReturnToPool(droppedWeaponController);
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Take {0} executed", dropData.WeaponId));
			}
			if (IsMineMessage(weaponTakeData.ViewId))
			{
				if (_weaponry != null && _weaponry.PhotonView.viewID == weaponTakeData.ViewId)
				{
					WeaponController weaponController = Singleton<WeaponManager>.Instance.Get(droppedWeaponController.WeaponId, droppedWeaponController.SkinId);
					if (weaponController is GunController)
					{
						GunController gunController = weaponController as GunController;
						gunController.Capacity = dropData.Capacity;
						gunController.MagazineCapacity = dropData.MagazineCapacity;
					}
					_weaponry.SetWeapon(weaponController);
					IWeaponDropListener dropListener = DropListener;
					if (dropListener != null)
					{
						dropListener.OnWeaponTake(_pool.GetWeaponParameters(dropData.WeaponId));
					}
				}
				else
				{
					if (Log.DebugEnabled)
					{
						Log.Debug("ViewId was changed");
					}
					byte[] array = SerializeDropData(dropData);
					base.PhotonView.RPC("DropImmediatelyViaServer", PhotonTargets.AllGlobalBuffered, array);
				}
				InProgress = false;
			}
			if (Log.DebugEnabled)
			{
				Log.Debug("WeaponTakeEvent");
			}
			if (this.WeaponTakeEvent != null)
			{
				this.WeaponTakeEvent(weaponTakeData, dropData);
			}
		}

		private bool IsMineMessage(int viewId)
		{
			return _weaponry != null && PhotonBehavior.ToOwnerId(viewId) == _weaponry.PhotonView.ownerId;
		}

		private static byte[] SerializeDropData(WeaponDropData dropData)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			dropData.Serialize(networkWriter);
			return networkWriter.ToArray();
		}

		private static WeaponDropData DeserializeDropData(byte[] bytes)
		{
			NetworkReader reader = new NetworkReader(bytes);
			WeaponDropData weaponDropData = new WeaponDropData();
			weaponDropData.Deserialize(reader);
			return weaponDropData;
		}

		private static byte[] SerializeTakeData(WeaponTakeData takeData)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			takeData.Serialize(networkWriter);
			return networkWriter.ToArray();
		}

		private static WeaponTakeData DeserializeTakeData(byte[] bytes)
		{
			NetworkReader reader = new NetworkReader(bytes);
			WeaponTakeData weaponTakeData = new WeaponTakeData();
			weaponTakeData.Deserialize(reader);
			return weaponTakeData;
		}

		private void UpdateMaterial(DroppedWeaponController controller)
		{
			Singleton<WeaponManager>.Instance.WeaponMaterialManager.SetMaterial(controller.LodGroup, controller.WeaponId, controller.SkinId);
		}

		private void OnModelDetailChanged()
		{
			_pool.RefreshLods();
		}

		private void OnShaderDetailChanged()
		{
			DroppedWeaponController[] allInstances = _pool.GetAllInstances();
			foreach (DroppedWeaponController controller in allInstances)
			{
				UpdateMaterial(controller);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			VideoSettingsManager.Instance.ModelDetailChanged -= OnModelDetailChanged;
			VideoSettingsManager.Instance.ShaderDetailChanged -= OnShaderDetailChanged;
		}
	}
}
