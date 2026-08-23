using UnityEngine;

namespace I2.Loc
{
	public abstract class LocalizeTarget<T> : ILocalizeTarget where T : Object
	{
		public override bool CanLocalize(Localize cmp)
		{
			return (Object)cmp.GetComponent<T>() != (Object)null;
		}

		public override bool FindTarget(Localize cmp)
		{
			cmp.mTarget = ((cmp.mTarget as T) ?? cmp.GetComponent<T>());
			return cmp.mTarget != null;
		}

		public T GetTarget(Localize cmp)
		{
			return cmp.mTarget as T;
		}

		public override bool HasTarget(Localize cmp)
		{
			return (Object)GetTarget(cmp) != (Object)null;
		}

		public override ILocalizeTarget Clone(Localize cmp)
		{
			return MemberwiseClone() as ILocalizeTarget;
		}

		public static H FindInParents<H>(Transform tr) where H : Component
		{
			if (!tr)
			{
				return (H)null;
			}
			H component = tr.GetComponent<H>();
			while (!(Object)component && (bool)tr)
			{
				component = tr.GetComponent<H>();
				tr = tr.parent;
			}
			return component;
		}
	}
}
