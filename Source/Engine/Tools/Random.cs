namespace Source.Engine.Tools
{
	public class Random
	{
		public static int Range(int min, int max)
		{
			return System.Random.Shared.Next(min, max);
		}
	}
}
