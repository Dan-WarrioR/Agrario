namespace Source.Engine.Tools.ProjectUtilities
{
	public class CustomRandom
	{
		public static int Range(int min, int max)
		{
			return Random.Shared.Next(min, max);
		}

		public static byte RangeByte(byte min, byte max)
		{
			return (byte)Random.Shared.Next(min, max);
		}

		public static float NextSingle()
		{
			return Random.Shared.NextSingle();
		}
	}
}