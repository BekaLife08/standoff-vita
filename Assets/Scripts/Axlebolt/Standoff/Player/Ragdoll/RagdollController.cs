using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Ragdoll
{
	public class RagdollController : MonoBehaviour
	{
		public enum State
		{
			Fallen,
			Falling,
			NotStated
		}

		private const float MinImpulse = 50f;

		private BipedMap _sourceCharacter;

		private BipedMap _character;

		private BipedMap _fallingCharacter;

		public RagdollParameters RagdollParameters;

		private List<Rigidbody> rigidlist = new List<Rigidbody>();

		private bool _isInitialized;

		private bool _isPreInitialized;

		private Dictionary<BipedMap.Bip, float> _bodyPartsMassMap = new Dictionary<BipedMap.Bip, float>();

		private Dictionary<BipedMap.Bip, Quaternion> _TPoseRotations = new Dictionary<BipedMap.Bip, Quaternion>();

		private float _hitTime;

		private State _state = State.NotStated;

		private FallingCharacterConfig _config;

		private Vector3 _intertialVelocity = Vector3.zero;

		private Vector3 _characterVelocity;

		public BipedMap Character
		{
			[CompilerGenerated]
			get
			{
				return _character;
			}
		}

		public Dictionary<BipedMap.Bip, float> BodyPartsMassMap
		{
			get
			{
				if (!_isPreInitialized)
				{
					UnityEngine.Debug.LogError("Not Preinitialized Yet");
				}
				return _bodyPartsMassMap;
			}
			set
			{
				_bodyPartsMassMap = value;
			}
		}

		public void PreInitialize(BipedMap sourceCharacter)
		{
			_isPreInitialized = true;
			_sourceCharacter = sourceCharacter;
			_character = this.GetRequireComponent<BipedMap>();
			_bodyPartsMassMap = new Dictionary<BipedMap.Bip, float>();
			AssignTargetCharacter();
		}

		public void FinishInitialization()
		{
			foreach (Rigidbody item in rigidlist)
			{
				item.isKinematic = false;
			}
			base.gameObject.SetActive(value: false);
			_isInitialized = true;
		}

		private void AssignBone(GameObject sourceBone, GameObject targetBone, BipedMap.Bip bodyPart)
		{
			AssignBone(sourceBone, targetBone, true, bodyPart);
		}

		private void AssignBone(GameObject sourceBone, GameObject targetBone, bool assignJoint, BipedMap.Bip bodyPart)
		{
			Rigidbody component = sourceBone.GetComponent<Rigidbody>();
			Rigidbody rigidbody = targetBone.AddComponent<Rigidbody>();
			rigidbody.mass = component.mass;
			rigidbody.angularDrag = component.angularDrag;
			rigidbody.drag = component.drag;
			rigidbody.isKinematic = true;
			rigidlist.Add(rigidbody);
			_bodyPartsMassMap[bodyPart] = rigidbody.mass;
			targetBone.transform.localRotation = sourceBone.transform.localRotation;
			Collider component2 = sourceBone.GetComponent<Collider>();
			if (component2 is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)component2;
				CapsuleCollider capsuleCollider2 = targetBone.AddComponent<CapsuleCollider>();
				capsuleCollider2.center = capsuleCollider.center;
				capsuleCollider2.direction = capsuleCollider.direction;
				capsuleCollider2.radius = capsuleCollider.radius;
				capsuleCollider2.height = capsuleCollider.height;
				capsuleCollider2.material = capsuleCollider.material;
			}
			if (component2 is BoxCollider)
			{
				BoxCollider boxCollider = (BoxCollider)component2;
				BoxCollider boxCollider2 = targetBone.AddComponent<BoxCollider>();
				boxCollider2.center = boxCollider.center;
				boxCollider2.size = boxCollider.size;
				boxCollider2.material = boxCollider.material;
			}
			if (assignJoint)
			{
				Quaternion localRotation = targetBone.transform.localRotation;
				CharacterJoint component3 = sourceBone.GetComponent<CharacterJoint>();
				CharacterJoint characterJoint = targetBone.AddComponent<CharacterJoint>();
				characterJoint.autoConfigureConnectedAnchor = component3.autoConfigureConnectedAnchor;
				characterJoint.anchor = component3.anchor;
				characterJoint.axis = component3.axis;
				characterJoint.connectedAnchor = component3.connectedAnchor;
				characterJoint.swingAxis = component3.swingAxis;
				characterJoint.lowTwistLimit = component3.lowTwistLimit;
				characterJoint.twistLimitSpring = component3.twistLimitSpring;
				characterJoint.highTwistLimit = component3.highTwistLimit;
				characterJoint.swingLimitSpring = component3.swingLimitSpring;
				characterJoint.swing1Limit = component3.swing1Limit;
				characterJoint.swing2Limit = component3.swing2Limit;
				characterJoint.enableProjection = component3.enableProjection;
				characterJoint.projectionDistance = component3.projectionDistance;
				characterJoint.projectionAngle = component3.projectionAngle;
				characterJoint.breakForce = component3.breakForce;
				characterJoint.breakTorque = component3.breakTorque;
				characterJoint.enableCollision = component3.enableCollision;
				characterJoint.enablePreprocessing = component3.enablePreprocessing;
			}
			targetBone.layer = sourceBone.layer;
		}

		private void LinkJoints(GameObject connectingJoint, GameObject connectBody)
		{
			connectingJoint.GetComponent<CharacterJoint>().connectedBody = connectBody.GetComponent<Rigidbody>();
		}

		private void AssigneBoneTransformHierarcly(Transform sourceRoot, Transform targetRoot)
		{
			for (int i = 0; i < sourceRoot.childCount; i++)
			{
				targetRoot.GetChild(i).localRotation = sourceRoot.GetChild(i).localRotation;
				AssigneBoneTransformHierarcly(targetRoot.GetChild(i), sourceRoot.GetChild(i));
			}
		}

		private void SaveTPose(BipedMap character)
		{
			_TPoseRotations[BipedMap.Bip.Head] = character.Head.localRotation;
			_TPoseRotations[BipedMap.Bip.Neck] = character.Neck.localRotation;
			_TPoseRotations[BipedMap.Bip.LeftThigh] = character.LeftThigh.localRotation;
			_TPoseRotations[BipedMap.Bip.LeftCalf] = character.LeftCalf.localRotation;
			_TPoseRotations[BipedMap.Bip.LeftFoot] = character.LeftFoot.localRotation;
			_TPoseRotations[BipedMap.Bip.RightThigh] = character.RightThigh.localRotation;
			_TPoseRotations[BipedMap.Bip.RightCalf] = character.RightCalf.localRotation;
			_TPoseRotations[BipedMap.Bip.RightFoot] = character.RightFoot.localRotation;
			_TPoseRotations[BipedMap.Bip.Hip] = character.Hip.localRotation;
			_TPoseRotations[BipedMap.Bip.Spine1] = character.Spine1.localRotation;
			_TPoseRotations[BipedMap.Bip.Spine2] = character.Spine2.localRotation;
			_TPoseRotations[BipedMap.Bip.LeftShoulder] = character.LeftShoulder.localRotation;
			_TPoseRotations[BipedMap.Bip.LeftUpperarm] = character.LeftUpperarm.localRotation;
			_TPoseRotations[BipedMap.Bip.LeftForearm] = character.LeftForearm.localRotation;
			_TPoseRotations[BipedMap.Bip.LeftHand] = character.LeftHand.localRotation;
			_TPoseRotations[BipedMap.Bip.RightShoulder] = character.RightShoulder.localRotation;
			_TPoseRotations[BipedMap.Bip.RightUpperarm] = character.RightUpperarm.localRotation;
			_TPoseRotations[BipedMap.Bip.RightForearm] = character.RightForearm.localRotation;
			_TPoseRotations[BipedMap.Bip.RightHand] = character.RightHand.localRotation;
		}

		private void ResetToTPose()
		{
			_character.Head.localRotation = _TPoseRotations[BipedMap.Bip.Head];
			_character.Neck.localRotation = _TPoseRotations[BipedMap.Bip.Neck];
			_character.LeftThigh.localRotation = _TPoseRotations[BipedMap.Bip.LeftThigh];
			_character.LeftCalf.localRotation = _TPoseRotations[BipedMap.Bip.LeftCalf];
			_character.LeftFoot.localRotation = _TPoseRotations[BipedMap.Bip.LeftFoot];
			_character.RightThigh.localRotation = _TPoseRotations[BipedMap.Bip.RightThigh];
			_character.RightCalf.localRotation = _TPoseRotations[BipedMap.Bip.RightCalf];
			_character.RightFoot.localRotation = _TPoseRotations[BipedMap.Bip.RightFoot];
			_character.Hip.localRotation = _TPoseRotations[BipedMap.Bip.Hip];
			_character.Spine1.localRotation = _TPoseRotations[BipedMap.Bip.Spine1];
			_character.Spine2.localRotation = _TPoseRotations[BipedMap.Bip.Spine2];
			_character.LeftShoulder.localRotation = _TPoseRotations[BipedMap.Bip.LeftShoulder];
			_character.LeftUpperarm.localRotation = _TPoseRotations[BipedMap.Bip.LeftUpperarm];
			_character.LeftForearm.localRotation = _TPoseRotations[BipedMap.Bip.LeftForearm];
			_character.LeftHand.localRotation = _TPoseRotations[BipedMap.Bip.LeftHand];
			_character.RightShoulder.localRotation = _TPoseRotations[BipedMap.Bip.RightShoulder];
			_character.RightUpperarm.localRotation = _TPoseRotations[BipedMap.Bip.RightUpperarm];
			_character.RightForearm.localRotation = _TPoseRotations[BipedMap.Bip.RightForearm];
			_character.RightHand.localRotation = _TPoseRotations[BipedMap.Bip.RightHand];
		}

		private void AssignTargetCharacter()
		{
			SaveTPose(_sourceCharacter);
			AssignBone(_sourceCharacter.Head.gameObject, _character.Head.gameObject, BipedMap.Bip.Head);
			AssignBone(_sourceCharacter.LeftUpperarm.gameObject, _character.LeftUpperarm.gameObject, BipedMap.Bip.LeftUpperarm);
			AssignBone(_sourceCharacter.LeftForearm.gameObject, _character.LeftForearm.gameObject, BipedMap.Bip.LeftForearm);
			AssignBone(_sourceCharacter.RightUpperarm.gameObject, _character.RightUpperarm.gameObject, BipedMap.Bip.RightUpperarm);
			AssignBone(_sourceCharacter.RightForearm.gameObject, _character.RightForearm.gameObject, BipedMap.Bip.RightForearm);
			AssignBone(_sourceCharacter.LeftThigh.gameObject, _character.LeftThigh.gameObject, BipedMap.Bip.LeftThigh);
			AssignBone(_sourceCharacter.LeftCalf.gameObject, _character.LeftCalf.gameObject, BipedMap.Bip.LeftCalf);
			AssignBone(_sourceCharacter.LeftFoot.gameObject, _character.LeftFoot.gameObject, BipedMap.Bip.LeftFoot);
			AssignBone(_sourceCharacter.RightThigh.gameObject, _character.RightThigh.gameObject, BipedMap.Bip.RightThigh);
			AssignBone(_sourceCharacter.RightCalf.gameObject, _character.RightCalf.gameObject, BipedMap.Bip.RightCalf);
			AssignBone(_sourceCharacter.RightFoot.gameObject, _character.RightFoot.gameObject, BipedMap.Bip.RightFoot);
			AssignBone(_sourceCharacter.LeftHand.gameObject, _character.LeftHand.gameObject, BipedMap.Bip.LeftHand);
			AssignBone(_sourceCharacter.RightHand.gameObject, _character.RightHand.gameObject, BipedMap.Bip.RightHand);
			AssignBone(_sourceCharacter.Spine1.gameObject, _character.Spine1.gameObject, BipedMap.Bip.Spine1);
			AssignBone(_sourceCharacter.Spine2.gameObject, _character.Spine2.gameObject, BipedMap.Bip.Spine2);
			AssignBone(_sourceCharacter.Hip.gameObject, _character.Hip.gameObject, false, BipedMap.Bip.Hip);
			AssigneBoneTransformHierarcly(_sourceCharacter.LeftHand, _character.LeftHand);
			AssigneBoneTransformHierarcly(_sourceCharacter.RightHand, _character.RightHand);
			LinkJoints(_character.LeftThigh.gameObject, _character.Hip.gameObject);
			LinkJoints(_character.RightThigh.gameObject, _character.Hip.gameObject);
			LinkJoints(_character.Spine1.gameObject, _character.Hip.gameObject);
			LinkJoints(_character.Spine2.gameObject, _character.Spine1.gameObject);
			LinkJoints(_character.LeftUpperarm.gameObject, _character.Spine2.gameObject);
			LinkJoints(_character.RightUpperarm.gameObject, _character.Spine2.gameObject);
			LinkJoints(_character.Head.gameObject, _character.Spine2.gameObject);
			LinkJoints(_character.LeftForearm.gameObject, _character.LeftUpperarm.gameObject);
			LinkJoints(_character.LeftHand.gameObject, _character.LeftForearm.gameObject);
			LinkJoints(_character.RightForearm.gameObject, _character.RightUpperarm.gameObject);
			LinkJoints(_character.RightHand.gameObject, _character.RightForearm.gameObject);
			LinkJoints(_character.LeftCalf.gameObject, _character.LeftThigh.gameObject);
			LinkJoints(_character.LeftFoot.gameObject, _character.LeftCalf.gameObject);
			LinkJoints(_character.RightCalf.gameObject, _character.RightThigh.gameObject);
			LinkJoints(_character.RightFoot.gameObject, _character.RightCalf.gameObject);
		}

		private void TargetCharacter()
		{
			_character.transform.position = _fallingCharacter.transform.position;
			_character.transform.rotation = _fallingCharacter.transform.rotation;
			_character.Head.localRotation = _fallingCharacter.Head.localRotation;
			_character.LeftUpperarm.localRotation = _fallingCharacter.LeftUpperarm.localRotation;
			_character.LeftForearm.localRotation = _fallingCharacter.LeftForearm.localRotation;
			_character.RightUpperarm.localRotation = _fallingCharacter.RightUpperarm.localRotation;
			_character.RightForearm.localRotation = _fallingCharacter.RightForearm.localRotation;
			_character.RightShoulder.localRotation = _fallingCharacter.RightShoulder.localRotation;
			_character.LeftShoulder.localRotation = _fallingCharacter.LeftShoulder.localRotation;
			_character.LeftThigh.localRotation = _fallingCharacter.LeftThigh.localRotation;
			_character.LeftCalf.localRotation = _fallingCharacter.LeftCalf.localRotation;
			_character.LeftFoot.localRotation = _fallingCharacter.LeftFoot.localRotation;
			_character.RightThigh.localRotation = _fallingCharacter.RightThigh.localRotation;
			_character.RightCalf.localRotation = _fallingCharacter.RightCalf.localRotation;
			_character.RightFoot.localRotation = _fallingCharacter.RightFoot.localRotation;
			_character.LeftHand.localRotation = _fallingCharacter.LeftHand.localRotation;
			_character.RightHand.localRotation = _fallingCharacter.RightHand.localRotation;
			_character.Spine1.localRotation = _fallingCharacter.Spine1.localRotation;
			_character.Spine2.localRotation = _fallingCharacter.Spine2.localRotation;
			_character.Hip.localRotation = _fallingCharacter.Hip.localRotation;
			_character.Hip.localPosition = _fallingCharacter.Hip.localPosition;
		}

		private IEnumerator HitCharacter(HitData hitData)
		{
			if (hitData.Hits.Length == 0)
			{
				UnityEngine.Debug.LogError("BulletHitData can't be empty");
				yield break;
			}
			foreach (Rigidbody item in rigidlist)
			{
				item.velocity = Vector3.zero;
				item.angularVelocity = Vector3.zero;
				item.isKinematic = true;
			}
			yield return new WaitForFixedUpdate();
			float hitImpulse = 0f;
			float addiditveImpulse = 0f;
			BulletHitData[] hits = hitData.Hits;
			foreach (BulletHitData bulletHitData in hits)
			{
				hitImpulse += bulletHitData.Impulse;
			}
			if (hitImpulse < 50f)
			{
				addiditveImpulse = (50f - hitImpulse) / (float)hitData.Hits.Length;
			}
			BulletHitData[] hits2 = hitData.Hits;
			foreach (BulletHitData bulletHitData2 in hits2)
			{
				HitBone(bulletHitData2.Bone, bulletHitData2.Impulse + addiditveImpulse, hitData.Direction);
			}
		}

		private void HitBone(BipedMap.Bip bone, float impulse, Vector3 direction)
		{
			TargetCharacter();
			bone = ReplaceToCorrectBone(bone);
			float num = 0f;
			Rigidbody requireComponent = _character.GetBone(bone).GetRequireComponent<Rigidbody>();
			RagdollParameters.BoneAffectGroup boneAffectGroup = RagdollParameters.boneAffectGroup.Find((RagdollParameters.BoneAffectGroup x) => x.targetBone == bone);
			foreach (Rigidbody item in rigidlist)
			{
				item.velocity = _intertialVelocity;
				item.angularVelocity = Vector3.zero;
				item.isKinematic = false;
			}
			float d = 1f;
			float d2;
			if (boneAffectGroup != null)
			{
				foreach (RagdollParameters.BoneImpulseEffectParameter affectedBone in boneAffectGroup.affectedBones)
				{
					Rigidbody component = _character.GetBone(affectedBone.bone).GetComponent<Rigidbody>();
					num += component.mass * affectedBone.impulseMult;
				}
				num += requireComponent.mass * boneAffectGroup.targetBoneImpulseMult;
				foreach (RagdollParameters.BoneImpulseEffectParameter affectedBone2 in boneAffectGroup.affectedBones)
				{
					Rigidbody component2 = _character.GetBone(affectedBone2.bone).GetComponent<Rigidbody>();
					d2 = impulse * (component2.mass * affectedBone2.impulseMult / num);
					Vector3 a = direction * d2;
					a += Vector3.down * d2;
					component2.AddForce(a, ForceMode.Impulse);
				}
				d = boneAffectGroup.targetBoneImpulseMult;
			}
			else
			{
				num = requireComponent.mass;
			}
			d2 = impulse * (requireComponent.mass / num);
			requireComponent.AddForce(direction * d2 * d, ForceMode.Impulse);
		}

		private static BipedMap.Bip ReplaceToCorrectBone(BipedMap.Bip bone)
		{
			if (bone == BipedMap.Bip.Neck)
			{
				bone = BipedMap.Bip.Head;
			}
			if (bone == BipedMap.Bip.LeftShoulder || bone == BipedMap.Bip.RightShoulder)
			{
				bone = BipedMap.Bip.Spine2;
			}
			return bone;
		}

		public void Simulate(IFallingCharacter fallingCharacter, HitData hitData)
		{
			_fallingCharacter = fallingCharacter.GetBipedMap();
			_characterVelocity = fallingCharacter.GetCharacterVelocity();
			fallingCharacter.OnSimulateFalling();
			ResetToTPose();
			base.gameObject.SetActive(value: true);
			TargetCharacter();
			float num = Vector3.Angle(hitData.Direction, _characterVelocity);
			_intertialVelocity = _characterVelocity * RagdollParameters.inertialVelocityDistribution.Evaluate(num / 180f);
			StartCoroutine(HitCharacter(hitData));
			_config = fallingCharacter.GetFallingCharacterConfig();
			_state = State.Falling;
			_hitTime = Time.time;
		}

		private void FixedUpdate()
		{
			if (_isInitialized && _character != null)
			{
				Rigidbody component = _character.LeftUpperarm.GetComponent<Rigidbody>();
				Rigidbody component2 = _character.LeftForearm.GetComponent<Rigidbody>();
				Rigidbody component3 = _character.RightUpperarm.GetComponent<Rigidbody>();
				Rigidbody component4 = _character.RightForearm.GetComponent<Rigidbody>();
				component.AddForce(Vector3.down * component.mass * 2f);
				component2.AddForce(Vector3.down * component2.mass * 2f);
				component4.AddForce(Vector3.down * component4.mass * 2f);
				component3.AddForce(Vector3.down * component3.mass * 2f);
			}
		}

		private void Update()
		{
			if (_config != null && _config.autoDestroyTime > 0f && (_state == State.Falling || _state == State.Fallen) && Time.time - _hitTime > _config.autoDestroyTime)
			{
				Singleton<RagdollManager>.Instance.ReturnPool(this);
				_state = State.NotStated;
			}
		}
	}
}
