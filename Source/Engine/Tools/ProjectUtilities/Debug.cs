namespace Source.Engine.Tools.ProjectUtilities
{
	public static class Debug
	{
		public static void Log(object message)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		public static void LogWarning(object message)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("WARNING: " + message);
			Console.ResetColor();
		}

		public static void LogError(object message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("ERROR: " + message);
			Console.ResetColor();
		}
	}
}