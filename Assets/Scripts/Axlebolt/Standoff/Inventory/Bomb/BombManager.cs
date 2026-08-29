using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Player;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Bomb
{
	public class BombManager : ScenePhotonBehavior<BombManager>
	{
		private static readonly Log Log = Log.Create(typeof(BombManager));

		private const int NotInit = -2;

		private const int NotSet = -1;

		private int _bomberViewId = -2;

		private Vector3 _bombPosition;

		private Vector3 _bombDirection;

		private int _sapperViewId = -1;

		private float _defuseDuration;

		private BombSite[] _plantSites;

		private IWeaponryController _weaponryController;

		private IHitController _hitController;

		private IBombListener _bombListener;

		private PlantedBombController _controller;

		private BombParameters _bombParameters;

		private BombExtendedParameters _extendedParameters;

		private bool _startDefuseLock;

		private bool _done;

		private bool _doneViaServer;

		private int BomberId
		{
			[CompilerGenerated]
			get
			{
				return _bomberViewId / PhotonNetwork.MAX_VIEW_IDS;
			}
		}

		public double DefuseStartTime
		{
			get;
			private set;
		}

		public double PlantTime
		{
			get;
			private set;
		}

		public bool IsDefused
		{
			get;
			private set;
		}

		public double DetonatedTime
		{
			get
			{
				if (IsDetonated())
				{
					return PlantTime + (double)_bombParameters.DetonationDuration;
				}
				throw new Exception("Bomb is not detonated");
			}
		}

		public double DefuseTime
		{
			get
			{
				if (IsDefused)
				{
					return DefuseStartTime + (double)_defuseDuration;
				}
				throw new Exception("Bomb is not defused");
			}
		}

		public PhotonPlayer FinalBomber
		{
			get;
			private set;
		}

		public PhotonPlayer FinalSapper
		{
			get;
			private set;
		}

		private void Awake()
		{
			InitWeaponDrop();
		}

		public override void OnInstantiate(object[] data)
		{
			_plantSites = UnityEngine.Object.FindObjectsOfType<BombSite>();
			if (_plantSites.Length == 0)
			{
				throw new Exception("BombSite's not found");
			}
			_bombParameters = (BombParameters)WeaponUtility.LoadWeapon(WeaponId.Bomb);
			_extendedParameters = WeaponUtility.LoadExtendedParameters<BombExtendedParameters>(WeaponId.Bomb);
			InitDetonator();
			PhotonNetworkExtension.MessageListeners.Add(this);
		}

		private void InitDetonator()
		{
			GameObject gameObject = new GameObject("Detonator");
			gameObject.transform.SetParent(base.transform);
			_controller = gameObject.AddComponent<PlantedBombController>();
			_controller.Init(_bombParameters, _extendedParameters);
		}

		public void SetPlayer(GameObject playerObject)
		{
			if (playerObject == null)
			{
				_weaponryController = null;
				_hitController = null;
				return;
			}
			_weaponryController = playerObject.GetRequireComponent<IWeaponryController>();
			_hitController = playerObject.GetRequireComponent<IHitController>();
			if (IsBomberMe())
			{
				TryTakeBomb();
			}
			else if (_weaponryController.HasWeapon(WeaponId.Bomb))
			{
				_weaponryController.ClearSlot(WeaponId.Bomb);
			}
		}

		public void SetBombListener(IBombListener bombListener)
		{
			_bombListener = bombListener;
		}

		public override void OnReturnToPool()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
			ClearInternal();
			UnityEngine.Object.Destroy(_controller.gameObject);
			_controller = null;
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
			_bomberViewId = -2;
			_sapperViewId = -1;
			_done = false;
			_doneViaServer = false;
			IsDefused = false;
			_startDefuseLock = false;
			_controller.Clear();
			_weaponryController = null;
			_hitController = null;
		}

		private void InitWeaponDrop()
		{
			if (ScenePhotonBehavior<WeaponDropManager>.IsInitialized())
			{
				BindWeaponDropManager();
			}
			else
			{
				ScenePhotonBehavior<WeaponDropManager>.ReadyEvent += BindWeaponDropManager;
			}
		}

		private void BindWeaponDropManager()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("BindWeaponDropManager");
			}
			ScenePhotonBehavior<WeaponDropManager>.Instance.WeaponDropEvent += OnWeaponDrop;
			ScenePhotonBehavior<WeaponDropManager>.Instance.WeaponTakeEvent += OnWeaponTake;
		}

		private void OnWeaponDrop(DroppedWeaponController droppedController, WeaponDropData data)
		{
			if (data.WeaponId == WeaponId.Bomb)
			{
				TryDropBomb();
				SetBomber(-1);
				BombIndicator componentInChildren = droppedController.GetComponentInChildren<BombIndicator>();
				componentInChildren.Flare = _extendedParameters.DetonatedIndicatorFlare;
				componentInChildren.Play(_extendedParameters.IndicatorSignalInterval, _extendedParameters.IndicatorBrigthnes, loop: true);
			}
		}

		private void OnWeaponTake(WeaponTakeData takeData, WeaponDropData dropData)
		{
			if (dropData.WeaponId == WeaponId.Bomb)
			{
				if (_bomberViewId != -1)
				{
					throw new Exception("Invalid state, bomber already changed!");
				}
				SetBomber(takeData.ViewId);
				TryTakeBomb();
			}
		}

		public void SetInitBomberIsMe()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("SetInitBomberIsMe");
			}
			if (IsBomberInitialized())
			{
				throw new Exception("Bomber already initialized");
			}
			Vector3 weaponDropPosition = _weaponryController.WeaponDropPosition;
			Vector3 weaponDropDirection = _weaponryController.WeaponDropDirection;
			base.PhotonView.RPC("SetInitBomberIsMeViaServer", PhotonTargets.AllGlobalBuffered, _weaponryController.PhotonView.viewID, weaponDropPosition, weaponDropDirection);
		}

		[PunRPC]
		private void SetInitBomberIsMeViaServer(int bomberViewId, Vector3 position, Vector3 forward)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("SetInitBomberIsMeViaServer");
			}
			if (IsBomberInitialized())
			{
				Log.Warning("Init bomber already initialized!");
				return;
			}
			SetBomber(bomberViewId);
			_bombPosition = position;
			_bombDirection = forward;
			if (IsBomberMe())
			{
				TryTakeBomb();
			}
		}

		public bool IsBomberInitialized()
		{
			return _bomberViewId != -2;
		}

		public override void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
		{
			if (PhotonNetwork.isMasterClient && otherPlayer.IsInactive && BomberId == otherPlayer.ID)
			{
				FixInactiveBomber();
			}
		}

		public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
			if (newMasterClient.IsLocal && IsBomberInactive())
			{
				FixInactiveBomber();
			}
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			if (PhotonNetwork.isMasterClient && BomberId == otherPlayer.ID)
			{
				FixInactiveBomber();
			}
		}

		private void SetBomber(int bomberView)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"BomberId changed to {bomberView}");
			}
			_bomberViewId = bomberView;
		}

		private void FixInactiveBomber()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("FixInactiveBomber");
			}
			IWeaponryController bomberController = GetBomberController();
			Vector3? vector = bomberController?.WeaponDropPosition;
			Vector3 vector2 = (!vector.HasValue) ? _bombPosition : vector.Value;
			Vector3? vector3 = bomberController?.WeaponDropDirection;
			Vector3 vector4 = (!vector3.HasValue) ? _bombDirection : vector3.Value;
			base.PhotonView.RPC("FixInactiveBomberViaServer", PhotonTargets.AllGlobalBuffered, _bomberViewId, vector2, vector4);
		}

		[PunRPC]
		private void FixInactiveBomberViaServer(int bomberViewId, Vector3 position, Vector3 direction)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug($"FixInactiveBomberViaServer position = {position}, direction = {direction}");
			}
			if (_bomberViewId != bomberViewId)
			{
				Log.Warning("DropBombViaServer ignored!");
				return;
			}
			TryDropBomb();
			ScenePhotonBehavior<WeaponDropManager>.Instance.DropInternal(new WeaponDropData
			{
				WeaponId = WeaponId.Bomb,
				Transform = new TransformTRS
				{
					pos = position,
					rot = Vector3.zero,
					scale = Vector3.one
				},
				Direction = direction
			});
		}

		private IWeaponryController GetBomberController()
		{
			if (_bomberViewId == -1)
			{
				return null;
			}
			PhotonView photonView = PhotonView.Find(_bomberViewId);
			return (!(photonView != null)) ? null : photonView.GetRequireComponent<IWeaponryController>();
		}

		public bool CanPlantBomb(BombController controller)
		{
			bool flag = IsInBombSite(controller.Transform);
			if (!flag)
			{
				if (_bombListener != null)
				{
					_bombListener.OnBombPlantError();
				}
			}
			else if (_bombListener != null)
			{
				_bombListener.OnBombPlanting();
			}
			return flag;
		}

		public bool IsInBombSite(Transform playerTransform)
		{
			return _plantSites.Any((BombSite zone) => zone.IsInZone(playerTransform.position));
		}

		public void PlantBomb(Vector3 position, float yRotation)
		{
			Log.Debug("PlantBomb");
			base.PhotonView.RPC("PlantBombViaServer", PhotonTargets.AllGlobalBuffered, _bomberViewId, position, yRotation);
		}

		[PunRPC]
		private void PlantBombViaServer(int bomberViewId, Vector3 position, float yRotation, PhotonMessageInfo messageInfo)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("PlantBombViaServer");
			}
			if (bomberViewId != _bomberViewId)
			{
				Log.Error($"Invalid bomber, expected {_bomberViewId}, actual {bomberViewId}");
				throw new Exception("Invalid bomber");
			}
			PlantedBombController controller = _controller;
			double timestamp = messageInfo.timestamp;
			IBombListener bombListener = _bombListener;
			controller.Plant(position, yRotation, timestamp, bombListener.OnBombBeepSignal);
			FinalBomber = PhotonPlayer.Find(BomberId);
			SetBomber(-1);
			PlantTime = messageInfo.timestamp;
			if (_bombListener != null)
			{
				_bombListener.OnBombPlanted(FinalBomber, _bombParameters.DetonationDuration);
			}
		}

		public bool CanDefuse(float defuseDistance)
		{
			if (!_controller.IsPlanted)
			{
				return false;
			}
			if (_doneViaServer)
			{
				return false;
			}
			float num = Vector3.Distance(_weaponryController.Transform.position, _controller.transform.position);
			return num <= defuseDistance;
		}

		public void StartDefuse(float defuseDistance, float defuseDuration)
		{
			if (!_startDefuseLock && CanDefuse(defuseDistance) && !IsSapperExists())
			{
				_startDefuseLock = true;
				base.PhotonView.RPC("StartDefuseViaServer", PhotonTargets.AllGlobalBuffered, _weaponryController.PhotonView.viewID, _sapperViewId, defuseDuration);
			}
		}

		[PunRPC]
		private void StartDefuseViaServer(int sapperViewId, int currentSapperViewId, float defuseDuration, PhotonMessageInfo messageInfo)
		{
			if (PhotonBehavior.IsLocalMessage(messageInfo))
			{
				_startDefuseLock = false;
				if (_bombListener != null)
				{
					_bombListener.OnDefusingBomb();
				}
			}
			if (!_controller.IsPlanted)
			{
				throw new Exception("Bomb is not planted");
			}
			if (_sapperViewId == currentSapperViewId)
			{
				_sapperViewId = sapperViewId;
				DefuseStartTime = messageInfo.timestamp;
				_defuseDuration = defuseDuration;
			}
		}

		public void BreakDefuse()
		{
			base.PhotonView.RPC("BreakDefuseViaServer", PhotonTargets.AllGlobalBuffered, _weaponryController.PhotonView.viewID);
		}

		[PunRPC]
		private void BreakDefuseViaServer(int sapperViewId)
		{
			if (_sapperViewId == sapperViewId)
			{
				_sapperViewId = -1;
			}
		}

		public bool IsBomber(PhotonPlayer player)
		{
			PhotonView photonView = PhotonView.Find(_bomberViewId);
			return photonView != null && photonView.ownerId == player.ID;
		}

		public bool IsBomber(PhotonView bomberView)
		{
			return bomberView.viewID == _bomberViewId;
		}

		public bool IsBomberMe()
		{
			return _weaponryController != null && _weaponryController.PhotonView.viewID == _bomberViewId;
		}

		public bool IsSapper(PhotonPlayer player)
		{
			PhotonView photonView = PhotonView.Find(_sapperViewId);
			return photonView != null && photonView.ownerId == player.ID;
		}

		public bool IsSapperMe()
		{
			return _weaponryController != null && _weaponryController.PhotonView.viewID == _sapperViewId;
		}

		private bool IsSapperExists()
		{
			if (_sapperViewId == -1)
			{
				return false;
			}
			PhotonView photonView = PhotonView.Find(_sapperViewId);
			return photonView != null && !photonView.owner.IsDead();
		}

		private void TryTakeBomb()
		{
			if (IsBomberMe() && _weaponryController != null)
			{
				WeaponController local = Singleton<WeaponManager>.Instance.GetLocal(WeaponId.Bomb);
				if (!_weaponryController.HasWeapon(WeaponId.Bomb))
				{
					_weaponryController.SetWeapon(local);
				}
			}
		}

		private void TryDropBomb()
		{
			if (IsBomberMe() && _weaponryController != null && _weaponryController.HasWeapon(WeaponId.Bomb))
			{
				_weaponryController.ClearSlot(WeaponId.Bomb);
			}
		}

		private bool IsBomberInactive()
		{
			if (_bomberViewId == -2 || _bomberViewId == -1)
			{
				return false;
			}
			PhotonView photonView = PhotonView.Find(_bomberViewId);
			return photonView == null || photonView.owner.IsInactive;
		}

		private void Update()
		{
			if (!_done && !_doneViaServer && _controller.IsPlanted)
			{
				if (IsDefuseTime(PhotonNetwork.time) && IsSapperExists())
				{
					double num = _controller.PlantTime + (double)_bombParameters.DetonationDuration - (DefuseStartTime + (double)_defuseDuration);
					Done(num > 0.0, num);
				}
				else if (IsDetonateTime(PhotonNetwork.time))
				{
					Done(false, 0.0);
				}
			}
		}

		private bool IsDefuseTime(double serverTime)
		{
			if (_sapperViewId != -1)
			{
				return serverTime - DefuseStartTime >= (double)_defuseDuration;
			}
			return false;
		}

		private bool IsDetonateTime(double serverTime)
		{
			return serverTime - _controller.PlantTime >= (double)_bombParameters.DetonationDuration;
		}

		private void Done(bool defused, double time)
		{
			_done = true;
			int num = defused ? (_sapperViewId / PhotonNetwork.MAX_VIEW_IDS) : 0;
			base.PhotonView.RPC("DoneViaServer", PhotonTargets.AllGlobalBuffered, defused, num, (float)time);
		}

		[PunRPC]
		private void DoneViaServer(bool defused, int finalSapperId, float time, PhotonMessageInfo messageInfo)
		{
			if (_doneViaServer)
			{
				return;
			}
			Log.Debug("DoneViaServer");
			if (!IsDefuseTime(messageInfo.timestamp) && !IsDetonateTime(messageInfo.timestamp))
			{
				if (PhotonBehavior.IsLocalMessage(messageInfo))
				{
					_done = false;
				}
				return;
			}
			_doneViaServer = true;
			IsDefused = defused;
			if (defused)
			{
				Defused(finalSapperId, time);
			}
			else
			{
				Detonated();
			}
		}

		private void Defused(int finalSapperId, float time)
		{
			_controller.Defused();
			FinalSapper = PhotonPlayer.Find(finalSapperId);
			if (_bombListener != null)
			{
				_bombListener.OnDefused(FinalSapper, time);
			}
		}

		private void Detonated()
		{
			_controller.Detonate();
			if (_bombListener != null)
			{
				_bombListener.OnDetonated(FinalBomber);
			}
			if (_weaponryController != null && !_weaponryController.PhotonView.owner.IsDead())
			{
				Hit();
			}
			HitBots();
		}

		private void HitBots()
		{
			if (!PhotonNetwork.offlineMode) return;
			Vector3 bombPosition = GetBombPosition();
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			for (int i = 0; i < playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = playerList[i];
				if (photonPlayer.IsLocal || photonPlayer.IsDead()) continue;
				PlayerController controller = Singleton<PlayerManager>.Instance.GetController(photonPlayer.ID);
				if (controller == null) continue;
				float dist = Vector3.Distance(bombPosition, controller.transform.position);
				if (dist > _bombParameters.DamageRadius) continue;
				float time = dist / _bombParameters.DamageRadius;
				float dmgEval = _bombParameters.DamageCurve.Evaluate(time);
				int damage = (int)(dmgEval * _bombParameters.Damage);
				int currentHealth = photonPlayer.GetHealth();
				int newHealth = currentHealth - damage;
				photonPlayer.SetHealth(newHealth);
				if (newHealth <= 0)
				{
					photonPlayer.SetHealth(0);
				}
			}
		}

		private void Hit()
		{
			Vector3 bombPosition = GetBombPosition();
			float num = Vector3.Distance(bombPosition, _weaponryController.Transform.position);
			if (!(num > _bombParameters.DamageRadius))
			{
				Vector3 vector = bombPosition - _weaponryController.Transform.position;
				float time = num / _bombParameters.DamageRadius;
				float num2 = _bombParameters.DamageCurve.Evaluate(time);
				float num3 = _bombParameters.ArmorPenetrationCurve.Evaluate(time);
				_hitController.Hit(new HitData
				{
					WeaponId = WeaponId.Bomb,
					Direction = vector.normalized,
					Hits = new BulletHitData[1]
					{
						new BulletHitData
						{
							Bone = BipedMap.Bip.Spine2,
							Damage = (int)(num2 * _bombParameters.Damage),
							Impulse = num2 * _bombParameters.Impulse,
							ArmorPenetration = 100f * num3
						}
					}
				});
			}
		}

		public bool IsBombPlanted()
		{
			return _controller.IsPlanted;
		}

		public Vector3 GetBombPosition()
		{
			return GetBombTransform().position;
		}

		public Transform GetBombTransform()
		{
			if (IsBombPlanted())
			{
				return _controller.Transform;
			}
			throw new Exception("Bomb is not planted");
		}

		public bool IsDone()
		{
			return _doneViaServer;
		}

		public bool IsDetonated()
		{
			return IsDone() && !IsDefused;
		}
	}
}
