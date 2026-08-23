using Axlebolt.Standoff.Player.State;
using System;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement
{
	[Serializable]
	public class PlayerTranslationParameters
	{
		[Serializable]
		public class WalkParameters
		{
			public float walkSpeedMultiplier;
		}

		[Serializable]
		public class CrouchParameters
		{
			public float crouchSpeedMultiplier;

			public CurvedValue standToCrouchCurve;
		}

		[Serializable]
		public class IdleParameters
		{
			public AnimationCurve rotationCurve1;

			public float rotationDuration1;

			public AnimationCurve rotationCurve2;

			public float rotationDuration2;
		}

		[Serializable]
		public class JumpParameters
		{
			public float upwardSpeedDefualt;

			public CurvedValue jumpCurve;

			public CurvedValue landCurve;

			public CurvedValue minSpeedCurve;
		}

		[Serializable]
		public class CharacterColliderParameters
		{
			public Vector3 centerOnStand;

			public float heightOnStand;

			public Vector3 centerOnCrouch;

			public float heightOnCrouch;
		}

		[Serializable]
		public class GeneralCurveTypes
		{
			public CurvedValue curveLinear01;

			public CurvedValue curveLinear11;
		}

		public float speedDefault;

		public float moveDirectionChangeSpeed;

		public float mecanimDirectionChangeSpeed;

		public WalkParameters walkParameters;

		public IdleParameters idleParameters;

		public CrouchParameters crouchParameters;

		public JumpParameters jumpParameters;

		public CharacterColliderParameters characterColliderParameters;

		public GeneralCurveTypes generalCurveTypes;
	}
}
