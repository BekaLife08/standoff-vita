namespace Axlebolt.Networking
{
	public class DataCompressor
	{
		public short CompressFloat(float value)
		{
			return (short)(value * 1000f);
		}

		public float DecompresssFloat(short compressedValue)
		{
			return (float)compressedValue / 1000f;
		}
	}
}
