namespace Axlebolt.Networking
{
	public abstract class Interpolator
	{
		public abstract ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress);
	}
}
