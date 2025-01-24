namespace Source.Engine.Tools
{
	public class Random
	{
		public static int Range(int min, int max)
		{
			return System.Random.Shared.Next(min, max);
		}

		public static float NextSingle()
		{
			return System.Random.Shared.NextSingle();
		}
	}
}