using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStd_Prefab : LocalizeTarget<GameObject>
	{
		static LocalizeTarget_UnityStd_Prefab()
		{
			AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTarget_UnityStd_Prefab());
		}

		public override string GetName()
		{
			return "Prefab";
		}

		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			GameObject target = GetTarget(cmp);
			primaryTerm = target.name;
			secondaryTerm = null;
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			GameObject target = GetTarget(cmp);
			if (!target || !(target.name == mainTranslation))
			{
				GameObject gameObject = target;
				GameObject gameObject2 = cmp.FindTranslatedObject<GameObject>(mainTranslation);
				if ((bool)gameObject2)
				{
					cmp.mTarget = Object.Instantiate(gameObject2);
					Transform transform = target.transform;
					Transform transform2 = (!gameObject) ? gameObject2.transform : gameObject.transform;
					transform.SetParent(cmp.transform);
					transform.localScale = transform2.localScale;
					transform.localRotation = transform2.localRotation;
					transform.localPosition = transform2.localPosition;
				}
				if ((bool)gameObject)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}

		public override bool CanLocalize(Localize cmp)
		{
			return cmp.transform.childCount > 0;
		}
	}
}
